using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Common;
using TH.Asset.Domain.Entities;
using TH.Asset.Domain.Enums;
using TH.Asset.Dtos;
using TH.Asset.Infrastructure.Database;
using TH.Constant;

namespace TH.Asset.ApplicationService.Service
{
    public interface IThanhLyTaiSanService
    {
        Task<ResponseDto<bool>> CreateThanhLyTaiSanAsync(CreateThanhLyTaiSanRequestDto request);
        Task<ResponseDto<bool>> UpdateThanhLyTaiSanAsync(UpdateThanhLyTaiSanRequestDto request);
        Task<ResponseDto<bool>> DeleteThanhLyTaiSanAsync(int id);
        Task<ResponseDto<List<ThanhLyTaiSanResponse>>> GetAllThanhLyTaiSanAsync();
        Task<ResponseDto<ThanhLyTaiSanResponse>> GetThanhLyTaiSanByIdAsync(int id);
        Task<ResponseDto<List<ThanhLyTaiSanResponse>>> GetByTaiSanIdAsync(int taiSanId);
    }

    public class ThanhLyTaiSanService : AssetServiceBase, IThanhLyTaiSanService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<ThanhLyTaiSanService> _logger;

        public ThanhLyTaiSanService(ILogger<ThanhLyTaiSanService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // ==========================================
        // 1. TẠO MỚI PHIẾU THANH LÝ
        // ==========================================
        public async Task<ResponseDto<bool>> CreateThanhLyTaiSanAsync(CreateThanhLyTaiSanRequestDto request)
        {
            // 1. Tạo Execution Strategy để PostgreSQL hỗ trợ Transaction an toàn
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            // 2. Bọc toàn bộ logic vào strategy.ExecuteAsync
            return await strategy.ExecuteAsync(async () =>
            {
                // Bắt đầu Transaction bên trong delegate
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var taiSan = await _dbContext.taiSans.FirstOrDefaultAsync(x => x.Id == request.taiSanId);
                    if (taiSan == null)
                    {
                        return ResponseConst.Error<bool>(404, "Tài sản không tồn tại.");
                    }

                    if (taiSan.TrangThai == TrangThaiTaiSan.DaThanhLy)
                    {
                        return ResponseConst.Error<bool>(400, "Tài sản này đã được thanh lý trước đó.");
                    }

                    // 1. Lấy dữ liệu sổ sách hiện tại của tài sản
                    decimal nguyenGia = taiSan.NguyenGia ?? 0;
                    decimal khauHaoLuyKe = taiSan.KhauHaoLuyKe ?? 0;
                    decimal giaTriConLai = taiSan.GiaTriConLai ?? 0;
                    decimal giaTriThanhLy = request.giaTriThanhLy ?? 0;

                    // 2. Tính Lãi/Lỗ: Bằng số tiền bán được trừ đi giá trị còn lại trên sổ sách
                    decimal laiLo = giaTriThanhLy - giaTriConLai;

                    var thanhLy = new ThanhLyTaiSan
                    {
                        TaiSanId = request.taiSanId,
                        NgayThanhLy = request.ngayThanhLy ?? DateTime.UtcNow,
                        NguyenGia = nguyenGia,
                        KhauHaoLuyKe = khauHaoLuyKe,
                        GiaTriConLai = giaTriConLai,
                        GiaTriThanhLy = giaTriThanhLy,
                        LaiLo = laiLo,
                        LyDo = request.lyDo,
                        GhiChu = request.ghiChu,
                        TrangThai = request.trangThai ?? TrangThaiThanhLy.ChoDuyet,
                        NgayTao = DateTime.UtcNow
                    };

                    _dbContext.thanhLyTaiSans.Add(thanhLy);
                    // Lưu trước để EF Core tự động gen ra thanhLy.Id (Dùng để tạo Mã chứng từ ở dưới)
                    await _dbContext.SaveChangesAsync();

                    // 3. Nếu phiếu thanh lý vừa tạo đã được set trạng thái HOÀN THÀNH
                    if (thanhLy.TrangThai == TrangThaiThanhLy.DaHoanThanh)
                    {
                        // 3.1 Cập nhật trạng thái tài sản thành Đã thanh lý
                        taiSan.TrangThai = TrangThaiTaiSan.DaThanhLy;
                        _dbContext.taiSans.Update(taiSan);

                        // 3.2 Tự động sinh Chứng từ kế toán (Master)
                        var chungTu = new ChungTu
                        {
                            MaChungTu = $"TL-{DateTime.Now:yyyyMMdd}-{thanhLy.Id}",
                            NgayLap = thanhLy.NgayThanhLy,
                            LoaiChungTu = (LoaiChungTu)3, // Cứ ép kiểu số 3 sang Enum
                            MoTa = $"Thanh lý tài sản {taiSan.MaTaiSan} - {taiSan.TenTaiSan}",
                            TongTien = nguyenGia,
                            TrangThai = "nhap",
                            NgayTao = DateTime.UtcNow
                        };
                        _dbContext.chungTus.Add(chungTu);
                        await _dbContext.SaveChangesAsync(); // Lưu để lấy ChungTuId

                        // 3.3 Tạo các dòng định khoản (Details)
                        var chiTietList = new List<ChiTietChungTu>();

                        string tkNguyenGia = !string.IsNullOrEmpty(taiSan.MaTaiKhoan) ? taiSan.MaTaiKhoan : "211";
                        string tkHaoMon = tkNguyenGia.StartsWith("213") ? "2143" : "2141";

                        // BÚT TOÁN 1: Xóa sổ tài sản
                        // 1.1 Ghi giảm phần đã khấu hao (Nợ 214 / Có 211)
                        if (khauHaoLuyKe > 0)
                        {
                            chiTietList.Add(new ChiTietChungTu
                            {
                                ChungTuId = chungTu.Id,
                                TaiSanId = taiSan.Id,
                                TaiKhoanNo = tkHaoMon,
                                TaiKhoanCo = tkNguyenGia,
                                SoTien = khauHaoLuyKe,
                                MoTa = $"Xóa sổ hao mòn lũy kế TSCĐ {taiSan.MaTaiSan}"
                            });
                        }

                        // 1.2 Ghi nhận Giá trị còn lại vào Chi phí khác (Nợ 811 / Có 211)
                        if (giaTriConLai > 0)
                        {
                            chiTietList.Add(new ChiTietChungTu
                            {
                                ChungTuId = chungTu.Id,
                                TaiSanId = taiSan.Id,
                                TaiKhoanNo = "811",
                                TaiKhoanCo = tkNguyenGia,
                                SoTien = giaTriConLai,
                                MoTa = $"Ghi nhận giá trị còn lại TSCĐ {taiSan.MaTaiSan}"
                            });
                        }

                        // BÚT TOÁN 2: Ghi nhận thu nhập thanh lý (Nợ 111 / Có 711)
                        if (giaTriThanhLy > 0)
                        {
                            chiTietList.Add(new ChiTietChungTu
                            {
                                ChungTuId = chungTu.Id,
                                TaiSanId = taiSan.Id,
                                TaiKhoanNo = "111", // Hoặc 112, tùy nghiệp vụ
                                TaiKhoanCo = "711",
                                SoTien = giaTriThanhLy,
                                MoTa = $"Thu tiền thanh lý TSCĐ {taiSan.MaTaiSan}"
                            });
                        }

                        _dbContext.chiTietChungTus.AddRange(chiTietList);
                        await _dbContext.SaveChangesAsync();
                    }

                    // Commit toàn bộ thay đổi xuống Database một cách an toàn
                    await transaction.CommitAsync();

                    return ResponseConst.Success("Tạo phiếu thanh lý và chứng từ thành công.", true);
                }
                catch (Exception ex)
                {
                    // Nếu có bất kỳ lỗi gì xảy ra, Rollback toàn bộ để dữ liệu không bị hỏng
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi khi tạo phiếu thanh lý tài sản.");
                    return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
                }
            }); // Kết thúc ExecuteAsync
        }

        // ==========================================
        // 2. CẬP NHẬT PHIẾU THANH LÝ
        // ==========================================
        public async Task<ResponseDto<bool>> UpdateThanhLyTaiSanAsync(UpdateThanhLyTaiSanRequestDto request)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var thanhLy = await _dbContext.thanhLyTaiSans.FirstOrDefaultAsync(x => x.Id == request.id);
                    if (thanhLy == null) return ResponseConst.Error<bool>(404, "Không tìm thấy phiếu thanh lý.");

                    var taiSan = await _dbContext.taiSans.FirstOrDefaultAsync(x => x.Id == thanhLy.TaiSanId);
                    if (taiSan == null) return ResponseConst.Error<bool>(404, "Tài sản không tồn tại.");

                    // 1. Kiểm tra bước chuyển trạng thái (State Transition)
                    bool wasCompleted = thanhLy.TrangThai == TrangThaiThanhLy.DaHoanThanh;
                    bool isBecomingCompleted = !wasCompleted && request.trangThai == TrangThaiThanhLy.DaHoanThanh;

                    if (wasCompleted)
                    {
                        return ResponseConst.Error<bool>(400, "Phiếu đã hoàn thành và ghi sổ, không thể chỉnh sửa.");
                    }

                    // 2. Cập nhật thông tin cơ bản
                    thanhLy.NgayThanhLy = request.ngayThanhLy ?? thanhLy.NgayThanhLy;
                    thanhLy.GiaTriThanhLy = request.giaTriThanhLy ?? thanhLy.GiaTriThanhLy;
                    thanhLy.LyDo = request.lyDo;
                    thanhLy.GhiChu = request.ghiChu;
                    thanhLy.TrangThai = request.trangThai ?? thanhLy.TrangThai;
                    thanhLy.LaiLo = (thanhLy.GiaTriThanhLy ?? 0) - (thanhLy.GiaTriConLai ?? 0);

                    // 3. LOGIC SINH CHỨNG TỪ KHI CHUYỂN SANG HOÀN THÀNH
                    if (isBecomingCompleted)
                    {
                        // 3.1 Cập nhật tài sản thành Đã thanh lý
                        taiSan.TrangThai = TrangThaiTaiSan.DaThanhLy;
                        _dbContext.taiSans.Update(taiSan);

                        // 3.2 Sinh Master Chứng từ
                        var chungTu = new ChungTu
                        {
                            MaChungTu = $"TL-{DateTime.Now:yyyyMMdd}-{thanhLy.Id}",
                            NgayLap = thanhLy.NgayThanhLy,
                            LoaiChungTu = LoaiChungTu.ThanhLy, // Số 3
                            MoTa = $"Thanh lý tài sản {taiSan.MaTaiSan} - {taiSan.TenTaiSan}",
                            TongTien = thanhLy.NguyenGia ?? 0,
                            TrangThai = "hoan_thanh",
                            NgayTao = DateTime.UtcNow
                        };
                        _dbContext.chungTus.Add(chungTu);
                        await _dbContext.SaveChangesAsync();

                        // 3.3 Định khoản chi tiết
                        var chiTietList = new List<ChiTietChungTu>();

                        string tkNguyenGia = !string.IsNullOrEmpty(taiSan.MaTaiKhoan) ? taiSan.MaTaiKhoan : "211";
                        string tkHaoMon = tkNguyenGia.StartsWith("213") ? "2143" : "2141";

                        // BÚT TOÁN 1: Xóa sổ tài sản
                        if (thanhLy.KhauHaoLuyKe > 0)
                        {
                            chiTietList.Add(new ChiTietChungTu
                            {
                                ChungTuId = chungTu.Id,
                                TaiSanId = taiSan.Id,
                                TaiKhoanNo = tkHaoMon,
                                TaiKhoanCo = tkNguyenGia,
                                SoTien = thanhLy.KhauHaoLuyKe ?? 0,
                                MoTa = $"Xóa sổ hao mòn lũy kế TSCĐ {taiSan.MaTaiSan}"
                            });
                        }

                        if (thanhLy.GiaTriConLai > 0)
                        {
                            chiTietList.Add(new ChiTietChungTu
                            {
                                ChungTuId = chungTu.Id,
                                TaiSanId = taiSan.Id,
                                TaiKhoanNo = "811",
                                TaiKhoanCo = tkNguyenGia,
                                SoTien = thanhLy.GiaTriConLai ?? 0,
                                MoTa = $"Ghi nhận giá trị còn lại TSCĐ {taiSan.MaTaiSan}"
                            });
                        }

                        // BÚT TOÁN 2: Ghi nhận doanh thu thanh lý
                        if (thanhLy.GiaTriThanhLy > 0)
                        {
                            chiTietList.Add(new ChiTietChungTu
                            {
                                ChungTuId = chungTu.Id,
                                TaiSanId = taiSan.Id,
                                TaiKhoanNo = "111",
                                TaiKhoanCo = "711",
                                SoTien = thanhLy.GiaTriThanhLy ?? 0,
                                MoTa = $"Thu tiền thanh lý TSCĐ {taiSan.MaTaiSan}"
                            });
                        }

                        _dbContext.chiTietChungTus.AddRange(chiTietList);
                    }

                    _dbContext.thanhLyTaiSans.Update(thanhLy);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return ResponseConst.Success("Cập nhật phiếu thanh lý thành công.", true);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi update thanh lý.");
                    return ResponseConst.Error<bool>(500, "Lỗi: " + ex.Message);
                }
            });
        }

        // ==========================================
        // 3. XÓA PHIẾU THANH LÝ
        // ==========================================
        public async Task<ResponseDto<bool>> DeleteThanhLyTaiSanAsync(int id)
        {
            try
            {

                var thanhLy = await _dbContext.thanhLyTaiSans.FirstOrDefaultAsync(x => x.Id == id);
                if (thanhLy == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy phiếu thanh lý.");
                }


                // Không cho xóa nếu phiếu đã hoàn thành
                if (thanhLy.TrangThai == TrangThaiThanhLy.DaHoanThanh)
                {
                    return ResponseConst.Error<bool>(400, "Không thể xóa phiếu thanh lý đã hoàn tất.");
                }

                _dbContext.thanhLyTaiSans.Remove(thanhLy);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Xóa phiếu thanh lý thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa phiếu thanh lý. ID: {Id}", id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 4. LẤY DANH SÁCH THANH LÝ
        // ==========================================
        public async Task<ResponseDto<List<ThanhLyTaiSanResponse>>> GetAllThanhLyTaiSanAsync()
        {
            try
            {
                var result = await _dbContext.thanhLyTaiSans
                    .Select(x => new ThanhLyTaiSanResponse
                    {
                        id = x.Id,
                        taiSanId = x.TaiSanId,
                        ngayThanhLy = x.NgayThanhLy,
                        nguyenGia = x.NguyenGia,
                        khauHaoLuyKe = x.KhauHaoLuyKe,
                        giaTriConLai = x.GiaTriConLai,
                        giaTriThanhLy = x.GiaTriThanhLy,
                        laiLo = x.LaiLo,
                        lyDo = x.LyDo,
                        ghiChu = x.GhiChu,
                        trangThai = x.TrangThai,
                        ngayTao = x.NgayTao
                    })
                    .OrderByDescending(x => x.ngayTao)
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách phiếu thanh lý thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phiếu thanh lý.");
                return ResponseConst.Error<List<ThanhLyTaiSanResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 5. LẤY CHI TIẾT THANH LÝ THEO ID
        // ==========================================
        public async Task<ResponseDto<ThanhLyTaiSanResponse>> GetThanhLyTaiSanByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.thanhLyTaiSans
                    .Where(x => x.Id == id)
                    .Select(x => new ThanhLyTaiSanResponse
                    {
                        id = x.Id,
                        taiSanId = x.TaiSanId,
                        ngayThanhLy = x.NgayThanhLy,
                        nguyenGia = x.NguyenGia,
                        khauHaoLuyKe = x.KhauHaoLuyKe,
                        giaTriConLai = x.GiaTriConLai,
                        giaTriThanhLy = x.GiaTriThanhLy,
                        laiLo = x.LaiLo,
                        lyDo = x.LyDo,
                        ghiChu = x.GhiChu,
                        trangThai = x.TrangThai,
                        ngayTao = x.NgayTao
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return ResponseConst.Error<ThanhLyTaiSanResponse>(404, "Không tìm thấy phiếu thanh lý.");
                }

                return ResponseConst.Success("Lấy chi tiết phiếu thanh lý thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết phiếu thanh lý. ID: {Id}", id);
                return ResponseConst.Error<ThanhLyTaiSanResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<List<ThanhLyTaiSanResponse>>> GetByTaiSanIdAsync(int taiSanId)
        {
            var result = await _dbContext.thanhLyTaiSans
                .Where(x => x.TaiSanId == taiSanId)
                .OrderByDescending(x => x.NgayThanhLy)
                .Select(x => new ThanhLyTaiSanResponse
                {
                    id = x.Id,
                    taiSanId = x.TaiSanId,
                    ngayThanhLy = x.NgayThanhLy,
                    nguyenGia = x.NguyenGia,
                    khauHaoLuyKe = x.KhauHaoLuyKe,
                    giaTriConLai = x.GiaTriConLai,
                    giaTriThanhLy = x.GiaTriThanhLy,
                    laiLo = x.LaiLo,
                    lyDo = x.LyDo,
                    ghiChu = x.GhiChu,
                    trangThai = x.TrangThai,
                    ngayTao = x.NgayTao
                })
                .ToListAsync();
            return ResponseConst.Success("Thành công", result);
        }
    }
}