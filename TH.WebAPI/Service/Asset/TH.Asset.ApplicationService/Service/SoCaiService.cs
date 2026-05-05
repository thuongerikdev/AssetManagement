using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Common;
using TH.Asset.Dtos;
using TH.Asset.Infrastructure.Database;
using TH.Constant;

namespace TH.Asset.ApplicationService.Service
{
    public interface ISoCaiService
    {
        Task<ResponseDto<List<SoCaiTomTatResponse>>> GetTomTatAsync(DateTime? fromDate, DateTime? toDate);
        Task<ResponseDto<SoCaiChiTietResponse>> GetChiTietAsync(string maTaiKhoan, DateTime? fromDate, DateTime? toDate);
    }

    public class SoCaiService : AssetServiceBase, ISoCaiService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<SoCaiService> _logger;

        public SoCaiService(ILogger<SoCaiService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // ==========================================
        // 1. TỔNG HỢP SỐ DƯ TẤT CẢ TÀI KHOẢN
        // ==========================================
        public async Task<ResponseDto<List<SoCaiTomTatResponse>>> GetTomTatAsync(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var from = fromDate ?? new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var to = (toDate ?? new DateTime(DateTime.UtcNow.Year, 12, 31, 23, 59, 59, DateTimeKind.Utc)).Date.AddDays(1).AddSeconds(-1);
                to = DateTime.SpecifyKind(to, DateTimeKind.Utc);
                from = DateTime.SpecifyKind(from.Date, DateTimeKind.Utc);

                // Lấy tất cả chi tiết chứng từ đã hoàn thành kèm ngày lập
                var allEntries = await _dbContext.chiTietChungTus
                    .Include(ct => ct.ChungTu)
                    .Where(ct => ct.ChungTu != null &&
                                 (ct.ChungTu.TrangThai == "hoan_thanh" || ct.ChungTu.TrangThai == "da_khoa"))
                    .Select(ct => new
                    {
                        ct.TaiKhoanNo,
                        ct.TaiKhoanCo,
                        ct.SoTien,
                        ct.ChungTu!.NgayLap,
                        ct.ChungTu.Id
                    })
                    .ToListAsync();

                // Thu thập tất cả TK có phát sinh
                var allAccountCodes = allEntries
                    .SelectMany(e => new[] { e.TaiKhoanNo, e.TaiKhoanCo })
                    .Where(tk => tk != null)
                    .Distinct()
                    .ToList();

                if (!allAccountCodes.Any())
                    return ResponseConst.Success("Không có dữ liệu phát sinh.", new List<SoCaiTomTatResponse>());

                // Lấy thông tin tài khoản
                var accountInfos = await _dbContext.taiKhoanKeToans
                    .Where(tk => allAccountCodes.Contains(tk.MaTaiKhoan))
                    .ToDictionaryAsync(tk => tk.MaTaiKhoan, tk => new { tk.TenTaiKhoan, tk.LoaiTaiKhoan });

                var result = new List<SoCaiTomTatResponse>();

                foreach (var tkCode in allAccountCodes.OrderBy(x => x))
                {
                    if (tkCode == null) continue;

                    // Số dư đầu kỳ: tổng phát sinh trước fromDate
                    var noDauKy = allEntries
                        .Where(e => e.TaiKhoanNo == tkCode && e.NgayLap.HasValue && e.NgayLap.Value < from)
                        .Sum(e => e.SoTien ?? 0);
                    var coDauKy = allEntries
                        .Where(e => e.TaiKhoanCo == tkCode && e.NgayLap.HasValue && e.NgayLap.Value < from)
                        .Sum(e => e.SoTien ?? 0);
                    var soDuDauKy = noDauKy - coDauKy;

                    // Phát sinh trong kỳ
                    var phatSinhNo = allEntries
                        .Where(e => e.TaiKhoanNo == tkCode && e.NgayLap.HasValue && e.NgayLap.Value >= from && e.NgayLap.Value <= to)
                        .Sum(e => e.SoTien ?? 0);
                    var phatSinhCo = allEntries
                        .Where(e => e.TaiKhoanCo == tkCode && e.NgayLap.HasValue && e.NgayLap.Value >= from && e.NgayLap.Value <= to)
                        .Sum(e => e.SoTien ?? 0);

                    var soLuong = allEntries
                        .Where(e => (e.TaiKhoanNo == tkCode || e.TaiKhoanCo == tkCode) && e.NgayLap.HasValue && e.NgayLap.Value >= from && e.NgayLap.Value <= to)
                        .Select(e => e.Id)
                        .Distinct()
                        .Count();

                    // Chỉ hiển thị TK có phát sinh trong kỳ hoặc có số dư
                    if (phatSinhNo == 0 && phatSinhCo == 0 && soDuDauKy == 0) continue;

                    accountInfos.TryGetValue(tkCode, out var info);

                    result.Add(new SoCaiTomTatResponse
                    {
                        maTaiKhoan = tkCode,
                        tenTaiKhoan = info?.TenTaiKhoan,
                        loaiTaiKhoan = info?.LoaiTaiKhoan,
                        soDuDauKy = soDuDauKy,
                        phatSinhNo = phatSinhNo,
                        phatSinhCo = phatSinhCo,
                        soDuCuoiKy = soDuDauKy + phatSinhNo - phatSinhCo,
                        soLuongButToan = soLuong
                    });
                }

                return ResponseConst.Success("Lấy tổng hợp sổ cái thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tổng hợp sổ cái.");
                return ResponseConst.Error<List<SoCaiTomTatResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 2. CHI TIẾT SỔ CÁI THEO TÀI KHOẢN
        // ==========================================
        public async Task<ResponseDto<SoCaiChiTietResponse>> GetChiTietAsync(string maTaiKhoan, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var from = fromDate ?? new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var to = (toDate ?? new DateTime(DateTime.UtcNow.Year, 12, 31, 23, 59, 59, DateTimeKind.Utc)).Date.AddDays(1).AddSeconds(-1);
                to = DateTime.SpecifyKind(to, DateTimeKind.Utc);
                from = DateTime.SpecifyKind(from.Date, DateTimeKind.Utc);

                var tkInfo = await _dbContext.taiKhoanKeToans
                    .FirstOrDefaultAsync(tk => tk.MaTaiKhoan == maTaiKhoan);

                if (tkInfo == null)
                    return ResponseConst.Error<SoCaiChiTietResponse>(404, $"Không tìm thấy tài khoản {maTaiKhoan}.");

                // Toàn bộ entries liên quan đến tài khoản này (đã hoàn thành)
                var relatedEntries = await _dbContext.chiTietChungTus
                    .Include(ct => ct.ChungTu)
                    .Where(ct => ct.ChungTu != null &&
                                 (ct.ChungTu.TrangThai == "hoan_thanh" || ct.ChungTu.TrangThai == "da_khoa") &&
                                 (ct.TaiKhoanNo == maTaiKhoan || ct.TaiKhoanCo == maTaiKhoan))
                    .Select(ct => new
                    {
                        ct.Id,
                        ct.TaiKhoanNo,
                        ct.TaiKhoanCo,
                        ct.SoTien,
                        ct.MoTa,
                        ct.ChungTu!.Id as ChungTuId,
                        ct.ChungTu.MaChungTu,
                        ct.ChungTu.NgayLap,
                        ct.ChungTu.LoaiChungTu
                    })
                    .OrderBy(ct => ct.NgayLap)
                    .ThenBy(ct => ct.ChungTuId)
                    .ToListAsync();

                // Số dư đầu kỳ (trước fromDate)
                var noDauKy = relatedEntries
                    .Where(e => e.TaiKhoanNo == maTaiKhoan && e.NgayLap.HasValue && e.NgayLap.Value < from)
                    .Sum(e => e.SoTien ?? 0);
                var coDauKy = relatedEntries
                    .Where(e => e.TaiKhoanCo == maTaiKhoan && e.NgayLap.HasValue && e.NgayLap.Value < from)
                    .Sum(e => e.SoTien ?? 0);
                var soDuDauKy = noDauKy - coDauKy;

                // Lọc các bút toán trong kỳ
                var inPeriod = relatedEntries
                    .Where(e => e.NgayLap.HasValue && e.NgayLap.Value >= from && e.NgayLap.Value <= to)
                    .ToList();

                var butToans = new List<SoCaiButToanResponse>();
                decimal runningBalance = soDuDauKy;

                foreach (var e in inPeriod)
                {
                    decimal no = e.TaiKhoanNo == maTaiKhoan ? (e.SoTien ?? 0) : 0;
                    decimal co = e.TaiKhoanCo == maTaiKhoan ? (e.SoTien ?? 0) : 0;
                    runningBalance += no - co;

                    butToans.Add(new SoCaiButToanResponse
                    {
                        chungTuId = e.ChungTuId,
                        maChungTu = e.MaChungTu,
                        ngayHachToan = e.NgayLap,
                        dienGiai = e.MoTa ?? e.MaChungTu,
                        phatSinhNo = no,
                        phatSinhCo = co,
                        soDuLuyKe = runningBalance,
                        loaiChungTu = e.LoaiChungTu?.ToString()
                    });
                }

                var totalNo = inPeriod.Where(e => e.TaiKhoanNo == maTaiKhoan).Sum(e => e.SoTien ?? 0);
                var totalCo = inPeriod.Where(e => e.TaiKhoanCo == maTaiKhoan).Sum(e => e.SoTien ?? 0);

                var response = new SoCaiChiTietResponse
                {
                    maTaiKhoan = maTaiKhoan,
                    tenTaiKhoan = tkInfo.TenTaiKhoan,
                    loaiTaiKhoan = tkInfo.LoaiTaiKhoan,
                    soDuDauKy = soDuDauKy,
                    phatSinhNo = totalNo,
                    phatSinhCo = totalCo,
                    soDuCuoiKy = soDuDauKy + totalNo - totalCo,
                    butToans = butToans
                };

                return ResponseConst.Success($"Lấy sổ cái tài khoản {maTaiKhoan} thành công.", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết sổ cái. TK: {MaTK}", maTaiKhoan);
                return ResponseConst.Error<SoCaiChiTietResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}
