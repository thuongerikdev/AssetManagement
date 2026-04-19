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
    public interface ILichSuKhauHaoService
    {
        Task<ResponseDto<bool>> CreateLichSuKhauHaoAsync(CreateLichSuKhauHaoRequestDto request);
        Task<ResponseDto<bool>> UpdateLichSuKhauHaoAsync(UpdateLichSuKhauHaoRequestDto request);
        Task<ResponseDto<bool>> DeleteLichSuKhauHaoAsync(int id);
        Task<ResponseDto<List<LichSuKhauHaoResponse>>> GetAllLichSuKhauHaoAsync();
        Task<ResponseDto<LichSuKhauHaoResponse>> GetLichSuKhauHaoByIdAsync(int id);
        Task<ResponseDto<List<LichSuKhauHaoResponse>>> GetByTaiSanIdAsync(int taiSanId);

        Task<ResponseDto<bool>> CreateKhauHaoHangLoatAsync(KhauHaoHangLoatRequestDto request);
    }

    public class LichSuKhauHaoService : AssetServiceBase, ILichSuKhauHaoService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<LichSuKhauHaoService> _logger;

        public LichSuKhauHaoService(ILogger<LichSuKhauHaoService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // ==========================================
        // 1. TẠO LỊCH SỬ KHẤU HAO MỚI
        // ==========================================
        public async Task<ResponseDto<bool>> CreateLichSuKhauHaoAsync(CreateLichSuKhauHaoRequestDto request)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    // 1. Kiểm tra tài sản
                    var taiSan = await _dbContext.taiSans.FirstOrDefaultAsync(x => x.Id == request.taiSanId);
                    if (taiSan == null)
                    {
                        return ResponseConst.Error<bool>(404, "Tài sản không tồn tại.");
                    }

                    // 2. Kiểm tra xem kỳ này đã được khấu hao chưa (Chống chạy trùng 1 tháng)
                    var isExist = await _dbContext.lichSuKhauHaos.AnyAsync(x => x.TaiSanId == request.taiSanId && x.KyKhauHao == request.kyKhauHao);
                    if (isExist)
                    {
                        return ResponseConst.Error<bool>(400, $"Tài sản này đã được tính khấu hao trong kỳ {request.kyKhauHao}.");
                    }

                    // 3. Tính toán lại Lũy kế và Giá trị còn lại
                    decimal soTienKhauHao = request.soTien;
                    decimal luyKeMoi = (taiSan.KhauHaoLuyKe ?? 0) + soTienKhauHao;
                    decimal giaTriConLaiMoi = (taiSan.NguyenGia ?? 0) - luyKeMoi;

                    // Xử lý nốt tháng cuối cùng: Đảm bảo giá trị còn lại không bị âm
                    if (giaTriConLaiMoi < 0)
                    {
                        soTienKhauHao = taiSan.GiaTriConLai ?? 0;
                        luyKeMoi = taiSan.NguyenGia ?? 0;
                        giaTriConLaiMoi = 0;
                    }

                    // Nếu tài sản đã hết giá trị thì không cần chạy khấu hao nữa
                    if (soTienKhauHao <= 0)
                    {
                        return ResponseConst.Error<bool>(400, "Tài sản này đã khấu hao hết giá trị.");
                    }

                    // 4. SINH CHỨNG TỪ KẾ TOÁN TRƯỚC (Để lấy Id)
                    var chungTu = new ChungTu
                    {
                        MaChungTu = $"KH-{request.kyKhauHao}-{taiSan.Id}-{DateTime.Now:MMddHHmm}",
                        NgayLap = DateTime.UtcNow,
                        LoaiChungTu = (LoaiChungTu)4, // Giả sử 4 là Khấu hao (Bạn có thể check lại Enum của bạn)
                        MoTa = $"Khấu hao TSCĐ {taiSan.MaTaiSan} - {taiSan.TenTaiSan} kỳ {request.kyKhauHao}",
                        TongTien = soTienKhauHao,
                        TrangThai = "hoan_thanh", // Khấu hao thường tự động hoàn thành luôn
                        NgayTao = DateTime.UtcNow
                    };

                    _dbContext.chungTus.Add(chungTu);
                    await _dbContext.SaveChangesAsync(); // Lưu để lấy chungTu.Id

                    // 5. TẠO CHI TIẾT CHỨNG TỪ (HẠCH TOÁN NỢ / CÓ)
                    var chiTietList = new List<ChiTietChungTu>();

                    // 5.1 Xác định TK Chi phí (Nợ)
                    string taiKhoanNo = "642"; // Mặc định là chi phí quản lý
                    if (!string.IsNullOrEmpty(taiSan.MaTaiKhoan))
                    {
                        if (taiSan.MaTaiKhoan == "2112") taiKhoanNo = "627"; // Máy móc -> SX Chung
                        else if (taiSan.MaTaiKhoan == "2113") taiKhoanNo = "641"; // Vận tải -> Bán hàng
                    }

                    // 5.2 Xác định TK Hao mòn (Có)
                    string taiKhoanCo = "2141"; // Mặc định TSCĐ hữu hình
                    if (!string.IsNullOrEmpty(taiSan.MaTaiKhoan) && taiSan.MaTaiKhoan.StartsWith("213"))
                    {
                        taiKhoanCo = "2143"; // Nếu là TSCĐ vô hình (Mã 213x)
                    }

                    chiTietList.Add(new ChiTietChungTu { ChungTuId = chungTu.Id, TaiSanId = taiSan.Id, TaiKhoanNo = taiKhoanNo, SoTien = soTienKhauHao, MoTa = $"Chi phí khấu hao TSCĐ kỳ {request.kyKhauHao}" });
                    chiTietList.Add(new ChiTietChungTu { ChungTuId = chungTu.Id, TaiSanId = taiSan.Id, TaiKhoanCo = taiKhoanCo, SoTien = soTienKhauHao, MoTa = $"Hao mòn TSCĐ kỳ {request.kyKhauHao}" });

                    _dbContext.chiTietChungTus.AddRange(chiTietList);

                    // 6. LƯU BẢN GHI LỊCH SỬ KHẤU HAO
                    var lichSu = new LichSuKhauHao
                    {
                        TaiSanId = request.taiSanId,
                        ChungTuId = chungTu.Id, // Gắn ID chứng từ vừa tạo vào đây
                        KyKhauHao = request.kyKhauHao,
                        SoTien = soTienKhauHao,
                        LuyKeSauKhauHao = luyKeMoi,
                        ConLaiSauKhauHao = giaTriConLaiMoi,
                        NgayTao = DateTime.UtcNow
                    };

                    _dbContext.lichSuKhauHaos.Add(lichSu);

                    // 7. CẬP NHẬT LẠI SỐ LIỆU TRÊN TÀI SẢN GỐC
                    taiSan.KhauHaoLuyKe = luyKeMoi;
                    taiSan.GiaTriConLai = giaTriConLaiMoi;
                    _dbContext.taiSans.Update(taiSan);

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return ResponseConst.Success("Tính khấu hao và sinh chứng từ thành công.", true);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi khi tạo lịch sử khấu hao và chứng từ.");
                    return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
                }
            }); // Kết thúc ExecuteAsync
        }

        // ==========================================
        // 2. CẬP NHẬT LỊCH SỬ KHẤU HAO
        // ==========================================
        public async Task<ResponseDto<bool>> UpdateLichSuKhauHaoAsync(UpdateLichSuKhauHaoRequestDto request)
        {
            try
            {
                var lichSu = await _dbContext.lichSuKhauHaos.FirstOrDefaultAsync(x => x.Id == request.id);
                if (lichSu == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy lịch sử khấu hao.");
                }

                var taiSan = await _dbContext.taiSans.FirstOrDefaultAsync(x => x.Id == request.taiSanId);
                if (taiSan == null)
                {
                    return ResponseConst.Error<bool>(404, "Tài sản không tồn tại.");
                }

                // Tính toán phần chênh lệch (Số tiền mới - Số tiền cũ)
                decimal tienCu = lichSu.SoTien ?? 0;
                decimal tienMoi = request.soTien;
                decimal chenhLech = tienMoi - tienCu;

                // Khôi phục và cộng dồn lại vào tài sản
                decimal luyKeMoi = (taiSan.KhauHaoLuyKe ?? 0) + chenhLech;
                decimal giaTriConLaiMoi = (taiSan.NguyenGia ?? 0) - luyKeMoi;

                // Cập nhật bản ghi lịch sử
                lichSu.KyKhauHao = request.kyKhauHao;
                lichSu.SoTien = tienMoi;
                lichSu.LuyKeSauKhauHao = (lichSu.LuyKeSauKhauHao ?? 0) + chenhLech;
                lichSu.ConLaiSauKhauHao = (lichSu.ConLaiSauKhauHao ?? 0) - chenhLech;
                lichSu.ChungTuId = request.chungTuId;

                _dbContext.lichSuKhauHaos.Update(lichSu);

                // Cập nhật lại tài sản
                taiSan.KhauHaoLuyKe = luyKeMoi;
                taiSan.GiaTriConLai = giaTriConLaiMoi;
                _dbContext.taiSans.Update(taiSan);

                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Cập nhật lịch sử khấu hao thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật lịch sử khấu hao. ID: {Id}", request.id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 3. XÓA LỊCH SỬ KHẤU HAO
        // ==========================================
        public async Task<ResponseDto<bool>> DeleteLichSuKhauHaoAsync(int id)
        {
            try
            {
                var lichSu = await _dbContext.lichSuKhauHaos.FirstOrDefaultAsync(x => x.Id == id);
                if (lichSu == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy lịch sử khấu hao.");
                }

                var taiSan = await _dbContext.taiSans.FirstOrDefaultAsync(x => x.Id == lichSu.TaiSanId);

                // Trả lại tiền khấu hao cho tài sản (Rollback)
                if (taiSan != null)
                {
                    taiSan.KhauHaoLuyKe = (taiSan.KhauHaoLuyKe ?? 0) - (lichSu.SoTien ?? 0);
                    taiSan.GiaTriConLai = (taiSan.NguyenGia ?? 0) - (taiSan.KhauHaoLuyKe ?? 0);
                    _dbContext.taiSans.Update(taiSan);
                }

                _dbContext.lichSuKhauHaos.Remove(lichSu);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Xóa lịch sử khấu hao thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa lịch sử khấu hao. ID: {Id}", id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 4. LẤY DANH SÁCH LỊCH SỬ KHẤU HAO
        // ==========================================
        public async Task<ResponseDto<List<LichSuKhauHaoResponse>>> GetAllLichSuKhauHaoAsync()
        {
            try
            {
                var result = await _dbContext.lichSuKhauHaos
                    .Select(x => new LichSuKhauHaoResponse
                    {
                        id = x.Id,
                        taiSanId = x.TaiSanId,
                        chungTuId = x.ChungTuId,
                        kyKhauHao = x.KyKhauHao,
                        soTien = x.SoTien,
                        luyKeSauKhauHao = x.LuyKeSauKhauHao,
                        conLaiSauKhauHao = x.ConLaiSauKhauHao,
                        ngayTao = x.NgayTao
                    })
                    .OrderByDescending(x => x.kyKhauHao)
                    .ThenByDescending(x => x.ngayTao)
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách lịch sử khấu hao thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách lịch sử khấu hao.");
                return ResponseConst.Error<List<LichSuKhauHaoResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 5. LẤY CHI TIẾT LỊCH SỬ KHẤU HAO
        // ==========================================
        public async Task<ResponseDto<LichSuKhauHaoResponse>> GetLichSuKhauHaoByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.lichSuKhauHaos
                    .Where(x => x.Id == id)
                    .Select(x => new LichSuKhauHaoResponse
                    {
                        id = x.Id,
                        taiSanId = x.TaiSanId,
                        chungTuId = x.ChungTuId,
                        kyKhauHao = x.KyKhauHao,
                        soTien = x.SoTien,
                        luyKeSauKhauHao = x.LuyKeSauKhauHao,
                        conLaiSauKhauHao = x.ConLaiSauKhauHao,
                        ngayTao = x.NgayTao
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return ResponseConst.Error<LichSuKhauHaoResponse>(404, "Không tìm thấy lịch sử khấu hao.");
                }

                return ResponseConst.Success("Lấy chi tiết lịch sử khấu hao thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết lịch sử khấu hao. ID: {Id}", id);
                return ResponseConst.Error<LichSuKhauHaoResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }


        public async Task<ResponseDto<List<LichSuKhauHaoResponse>>> GetByTaiSanIdAsync(int taiSanId)
        {
            try
            {
                var result = await _dbContext.lichSuKhauHaos
                    .Where(x => x.TaiSanId == taiSanId)
                    .OrderByDescending(x => x.KyKhauHao) // Sắp xếp kỳ mới nhất lên đầu
                    .Select(x => new LichSuKhauHaoResponse
                    {
                        id = x.Id,
                        taiSanId = x.TaiSanId,
                        chungTuId = x.ChungTuId,
                        kyKhauHao = x.KyKhauHao,
                        soTien = x.SoTien,
                        luyKeSauKhauHao = x.LuyKeSauKhauHao,
                        conLaiSauKhauHao = x.ConLaiSauKhauHao,
                        ngayTao = x.NgayTao
                    })
                    .ToListAsync();

                return ResponseConst.Success("Lấy lịch sử khấu hao thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy lịch sử khấu hao theo tài sản.");
                return ResponseConst.Error<List<LichSuKhauHaoResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> CreateKhauHaoHangLoatAsync(KhauHaoHangLoatRequestDto request)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    if (request.DanhSachTaiSan == null || !request.DanhSachTaiSan.Any())
                        return ResponseConst.Error<bool>(400, "Danh sách tài sản trống.");

                    // 1. TÍNH TỔNG TIỀN & TẠO 1 CHỨNG TỪ TỔNG (MASTER) DUY NHẤT
                    decimal tongTienKhauHao = request.DanhSachTaiSan.Sum(x => x.SoTien);

                    var chungTuTong = new ChungTu
                    {
                        MaChungTu = $"KH-{request.KyKhauHao}-{DateTime.Now:ddHHmmss}",
                        NgayLap = DateTime.UtcNow,

                        // SỬA LỖI TẠI ĐÂY: Dùng trực tiếp Enum thay vì số nguyên
                        LoaiChungTu = LoaiChungTu.KhauHao,

                        MoTa = $"Trích khấu hao TSCĐ kỳ {request.KyKhauHao} cho {request.DanhSachTaiSan.Count} tài sản",
                        TongTien = tongTienKhauHao,
                        TrangThai = "hoan_thanh",
                        NgayTao = DateTime.UtcNow
                    };

                    _dbContext.chungTus.Add(chungTuTong);
                    await _dbContext.SaveChangesAsync(); // Lưu để lấy ID Chứng từ Cha

                    // 2. DUYỆT TỪNG TÀI SẢN ĐỂ SINH DÒNG CHI TIẾT & CẬP NHẬT
                    var chiTietList = new List<ChiTietChungTu>();
                    var lichSuList = new List<LichSuKhauHao>();

                    foreach (var item in request.DanhSachTaiSan)
                    {
                        var taiSan = await _dbContext.taiSans.FindAsync(item.TaiSanId);
                        if (taiSan == null) continue;

                        // Tính toán Lũy kế mới
                        decimal luyKeMoi = (taiSan.KhauHaoLuyKe ?? 0) + item.SoTien;
                        decimal giaTriConLaiMoi = (taiSan.NguyenGia ?? 0) - luyKeMoi;

                        if (giaTriConLaiMoi < 0)
                        {
                            item.SoTien = taiSan.GiaTriConLai ?? 0;
                            luyKeMoi = taiSan.NguyenGia ?? 0;
                            giaTriConLaiMoi = 0;
                        }

                        // Xác định TK Kế toán
                        string taiKhoanNo = "642";
                        if (taiSan.MaTaiKhoan == "2112") taiKhoanNo = "627";
                        else if (taiSan.MaTaiKhoan == "2113") taiKhoanNo = "641";
                        string taiKhoanCo = taiSan.MaTaiKhoan?.StartsWith("213") == true ? "2143" : "2141";

                        // Sinh dòng Detail: Gắn vào ID Chứng Từ Cha
                        chiTietList.Add(new ChiTietChungTu { ChungTuId = chungTuTong.Id, TaiSanId = taiSan.Id, TaiKhoanNo = taiKhoanNo, SoTien = item.SoTien, MoTa = $"Chi phí KH kỳ {request.KyKhauHao}" });
                        chiTietList.Add(new ChiTietChungTu { ChungTuId = chungTuTong.Id, TaiSanId = taiSan.Id, TaiKhoanCo = taiKhoanCo, SoTien = item.SoTien, MoTa = $"Hao mòn TSCĐ kỳ {request.KyKhauHao}" });

                        // Sinh dòng Lịch sử: Gắn vào ID Chứng Từ Cha
                        lichSuList.Add(new LichSuKhauHao { TaiSanId = taiSan.Id, ChungTuId = chungTuTong.Id, KyKhauHao = request.KyKhauHao, SoTien = item.SoTien, LuyKeSauKhauHao = luyKeMoi, ConLaiSauKhauHao = giaTriConLaiMoi, NgayTao = DateTime.UtcNow });

                        // Cập nhật Tài sản
                        taiSan.KhauHaoLuyKe = luyKeMoi;
                        taiSan.GiaTriConLai = giaTriConLaiMoi;
                        _dbContext.taiSans.Update(taiSan);
                    }

                    // Ghi toàn bộ vào DB 1 lần (Bulk Insert)
                    _dbContext.chiTietChungTus.AddRange(chiTietList);
                    _dbContext.lichSuKhauHaos.AddRange(lichSuList);

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return ResponseConst.Success($"Đã sinh Chứng từ tổng {chungTuTong.MaChungTu} thành công.", true);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi khi tính khấu hao hàng loạt.");
                    return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
                }
            });
        }
    }
}