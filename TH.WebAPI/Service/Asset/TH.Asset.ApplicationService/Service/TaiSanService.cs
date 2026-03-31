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
    public interface ITaiSanService
    {
        Task<ResponseDto<bool>> CreateTaiSanAsync(CreateTaiSanRequestDto request);
        Task<ResponseDto<bool>> UpdateTaiSanAsync(UpdateTaiSanRequestDto request);
        Task<ResponseDto<bool>> DeleteTaiSanAsync(int id);
        Task<ResponseDto<List<TaiSanResponse>>> GetAllTaiSanAsync();
        Task<ResponseDto<TaiSanResponse>> GetTaiSanByIdAsync(int id);
        Task<ResponseDto<bool>> ConfirmAssetAsync(int id);
        Task<ResponseDto<List<TaiSan>>> GetTaiSanByUserIdAsync(int userId);
    }

    public class TaiSanService : AssetServiceBase, ITaiSanService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<TaiSanService> _logger;

        public TaiSanService(ILogger<TaiSanService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // ==========================================
        // HÀM HỖ TRỢ: Kiểm tra Khóa Ngoại (Đã bỏ Lô)
        // ==========================================
        private async Task<string?> ValidateForeignKeysAsync(int? danhMucId, int? phongBanId, string? maTaiKhoan)
        {
            if (danhMucId.HasValue && !await _dbContext.danhMucTaiSans.AnyAsync(x => x.Id == danhMucId))
                return "Danh mục tài sản không tồn tại.";

            if (phongBanId.HasValue && !await _dbContext.phongBans.AnyAsync(x => x.Id == phongBanId))
                return "Phòng ban không tồn tại.";

            if (!string.IsNullOrEmpty(maTaiKhoan) && !await _dbContext.taiKhoanKeToans.AnyAsync(x => x.MaTaiKhoan == maTaiKhoan))
                return "Mã tài khoản kế toán không tồn tại.";

            return null; // Không có lỗi
        }

        // ==========================================
        // 1. TẠO MỚI TÀI SẢN
        // ==========================================
        public async Task<ResponseDto<bool>> CreateTaiSanAsync(CreateTaiSanRequestDto request)
        {
            try
            {
                // 1. Kiểm tra trùng mã tài sản
                var isExist = await _dbContext.taiSans.AnyAsync(x => x.MaTaiSan == request.maTaiSan);
                if (isExist)
                {
                    return ResponseConst.Error<bool>(400, "Mã tài sản đã tồn tại trong hệ thống.");
                }

                // 2. Validate Foreign Keys
                var validationError = await ValidateForeignKeysAsync(request.danhMucId, request.phongBanId, request.maTaiKhoan);
                if (validationError != null) return ResponseConst.Error<bool>(400, validationError);

                var taiSan = new TaiSan
                {
                    MaTaiSan = request.maTaiSan,
                    TenTaiSan = request.tenTaiSan,
                    DanhMucId = request.danhMucId,
                    TrangThai = request.trangThai,
                    SoSeri = request.soSeri,
                    NhaSanXuat = request.nhaSanXuat,
                    MoTa = request.moTa,
                    ThongSoKyThuat = request.thongSoKyThuat,
                    NgayMua = request.ngayMua,
                    NguyenGia = request.nguyenGia,

                    // Logic mặc định khi tạo mới
                    GiaTriConLai = request.nguyenGia ?? 0,
                    KhauHaoLuyKe = 0,
                    KhauHaoHangThang = 0,

                    PhuongPhapKhauHao = request.phuongPhapKhauHao,
                    ThoiGianKhauHao = request.thoiGianKhauHao,
                    MaTaiKhoan = request.maTaiKhoan,
                    PhongBanId = request.phongBanId,

                    // Lưu NguoiDungId (không ràng buộc khóa ngoại)
                    NguoiDungId = request.nguoiDungId,
                    // Tự động set ngày cấp phát nếu có cấp cho người dùng
                    NgayCapPhat = request.nguoiDungId.HasValue ? (request.ngayCapPhat ?? DateTime.UtcNow) : null,

                    NgayTao = DateTime.UtcNow
                };

                _dbContext.taiSans.Add(taiSan);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Thêm tài sản thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo tài sản.");
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 2. CẬP NHẬT TÀI SẢN
        // ==========================================
        public async Task<ResponseDto<bool>> UpdateTaiSanAsync(UpdateTaiSanRequestDto request)
        {
            try
            {
                var taiSan = await _dbContext.taiSans.FirstOrDefaultAsync(x => x.Id == request.id);
                if (taiSan == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy tài sản.");
                }

                // 1. Kiểm tra mã mới có trùng không
                if (taiSan.MaTaiSan != request.maTaiSan)
                {
                    var isExist = await _dbContext.taiSans.AnyAsync(x => x.MaTaiSan == request.maTaiSan);
                    if (isExist) return ResponseConst.Error<bool>(400, "Mã tài sản mới đã tồn tại ở bản ghi khác.");
                }

                // 2. Validate Foreign Keys
                var validationError = await ValidateForeignKeysAsync(request.danhMucId, request.phongBanId, request.maTaiKhoan);
                if (validationError != null) return ResponseConst.Error<bool>(400, validationError);

                // Cập nhật các trường
                taiSan.MaTaiSan = request.maTaiSan;
                taiSan.TenTaiSan = request.tenTaiSan;
                taiSan.DanhMucId = request.danhMucId;
                taiSan.TrangThai = request.trangThai;
                taiSan.SoSeri = request.soSeri;
                taiSan.NhaSanXuat = request.nhaSanXuat;
                taiSan.MoTa = request.moTa;
                taiSan.ThongSoKyThuat = request.thongSoKyThuat;
                taiSan.NgayMua = request.ngayMua;
                taiSan.NguyenGia = request.nguyenGia;

                // Các trường cập nhật thêm từ UpdateRequest
                taiSan.GiaTriConLai = request.giaTriConLai ?? taiSan.GiaTriConLai;
                taiSan.KhauHaoLuyKe = request.khauHaoLuyKe ?? taiSan.KhauHaoLuyKe;
                taiSan.KhauHaoHangThang = request.khauHaoHangThang ?? taiSan.KhauHaoHangThang;

                taiSan.PhuongPhapKhauHao = request.phuongPhapKhauHao;
                taiSan.ThoiGianKhauHao = request.thoiGianKhauHao;
                taiSan.MaTaiKhoan = request.maTaiKhoan;
                taiSan.PhongBanId = request.phongBanId;

                // Nếu đổi người dùng thì ghi nhận lại ngày cấp phát
                if (taiSan.NguoiDungId != request.nguoiDungId)
                {
                    taiSan.NguoiDungId = request.nguoiDungId;
                    taiSan.NgayCapPhat = request.nguoiDungId.HasValue ? (request.ngayCapPhat ?? DateTime.UtcNow) : null;
                }
                else
                {
                    taiSan.NgayCapPhat = request.ngayCapPhat;
                }

                taiSan.NgayCapNhat = DateTime.UtcNow;

                _dbContext.taiSans.Update(taiSan);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Cập nhật tài sản thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật tài sản. ID: {Id}", request.id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 3. XÓA TÀI SẢN
        // ==========================================
        public async Task<ResponseDto<bool>> DeleteTaiSanAsync(int id)
        {
            try
            {
                var taiSan = await _dbContext.taiSans.FirstOrDefaultAsync(x => x.Id == id);
                if (taiSan == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy tài sản.");
                }

                // Kiểm tra xem tài sản đã có phát sinh lịch sử chưa
                var isUsed = await _dbContext.lichSuKhauHaos.AnyAsync(x => x.TaiSanId == id) ||
                             await _dbContext.dieuChuyenTaiSans.AnyAsync(x => x.TaiSanId == id) ||
                             await _dbContext.baoTriTaiSans.AnyAsync(x => x.TaiSanId == id) ||
                             await _dbContext.chiTietChungTus.AnyAsync(x => x.TaiSanId == id);

                if (isUsed)
                {
                    return ResponseConst.Error<bool>(400, "Tài sản này đã phát sinh giao dịch/lịch sử. Không thể xóa hoàn toàn, vui lòng thực hiện Thanh lý.");
                }

                _dbContext.taiSans.Remove(taiSan);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Xóa tài sản thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa tài sản. ID: {Id}", id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 4. LẤY DANH SÁCH TÀI SẢN
        // ==========================================
        public async Task<ResponseDto<List<TaiSanResponse>>> GetAllTaiSanAsync()
        {
            try
            {
                var result = await _dbContext.taiSans
                    .Select(x => new TaiSanResponse
                    {
                        id = x.Id,
                        maTaiSan = x.MaTaiSan,
                        tenTaiSan = x.TenTaiSan,
                        danhMucId = x.DanhMucId,
                        // Đã bỏ lô
                        trangThai = x.TrangThai,
                        soSeri = x.SoSeri,
                        nhaSanXuat = x.NhaSanXuat,
                        moTa = x.MoTa,
                        thongSoKyThuat = x.ThongSoKyThuat,
                        ngayMua = x.NgayMua,
                        nguyenGia = x.NguyenGia,
                        giaTriConLai = x.GiaTriConLai,
                        khauHaoLuyKe = x.KhauHaoLuyKe,
                        khauHaoHangThang = x.KhauHaoHangThang,
                        phuongPhapKhauHao = x.PhuongPhapKhauHao,
                        thoiGianKhauHao = x.ThoiGianKhauHao,
                        maTaiKhoan = x.MaTaiKhoan,
                        phongBanId = x.PhongBanId,
                        nguoiDungId = x.NguoiDungId,
                        ngayCapPhat = x.NgayCapPhat,
                        ngayTao = x.NgayTao,
                        ngayCapNhat = x.NgayCapNhat
                    })
                    .OrderByDescending(x => x.ngayTao)
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách tài sản thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách tài sản.");
                return ResponseConst.Error<List<TaiSanResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 5. LẤY CHI TIẾT TÀI SẢN THEO ID
        // ==========================================
        public async Task<ResponseDto<TaiSanResponse>> GetTaiSanByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.taiSans
                    .Where(x => x.Id == id)
                    .Select(x => new TaiSanResponse
                    {
                        id = x.Id,
                        maTaiSan = x.MaTaiSan,
                        tenTaiSan = x.TenTaiSan,
                        danhMucId = x.DanhMucId,
                        // Đã bỏ lô
                        trangThai = x.TrangThai,
                        soSeri = x.SoSeri,
                        nhaSanXuat = x.NhaSanXuat,
                        moTa = x.MoTa,
                        thongSoKyThuat = x.ThongSoKyThuat,
                        ngayMua = x.NgayMua,
                        nguyenGia = x.NguyenGia,
                        giaTriConLai = x.GiaTriConLai,
                        khauHaoLuyKe = x.KhauHaoLuyKe,
                        khauHaoHangThang = x.KhauHaoHangThang,
                        phuongPhapKhauHao = x.PhuongPhapKhauHao,
                        thoiGianKhauHao = x.ThoiGianKhauHao,
                        maTaiKhoan = x.MaTaiKhoan,
                        phongBanId = x.PhongBanId,
                        nguoiDungId = x.NguoiDungId,
                        ngayCapPhat = x.NgayCapPhat,
                        ngayTao = x.NgayTao,
                        ngayCapNhat = x.NgayCapNhat
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return ResponseConst.Error<TaiSanResponse>(404, "Không tìm thấy tài sản.");
                }

                return ResponseConst.Success("Lấy chi tiết tài sản thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết tài sản. ID: {Id}", id);
                return ResponseConst.Error<TaiSanResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> ConfirmAssetAsync(int id)
        {
            try
            {
                var taiSan = await _dbContext.taiSans.FindAsync(id);
                if (taiSan == null) return ResponseConst.Error<bool>(404, "Không tìm thấy tài sản.");

                if (taiSan.TrangThai != TrangThaiTaiSan.ChoXacNhan)
                    return ResponseConst.Error<bool>(400, "Tài sản không ở trạng thái chờ xác nhận.");

                // 1. Chuyển tài sản sang Đang sử dụng
                taiSan.TrangThai = TrangThaiTaiSan.DangSuDung;
                taiSan.NgayCapPhat = DateTime.UtcNow; // Chốt ngày nhận
                _dbContext.taiSans.Update(taiSan);

                // 2. Tự động tìm Phiếu cấp phát/điều chuyển đang "Chờ xử lý" của tài sản này
                var phieuCho = await _dbContext.dieuChuyenTaiSans
                    .Where(x => x.TaiSanId == id && x.TrangThai == "cho_xu_ly")
                    .FirstOrDefaultAsync();

                // Chốt luôn phiếu thành Đã hoàn thành
                if (phieuCho != null)
                {
                    phieuCho.TrangThai = "da_hoan_thanh";
                    _dbContext.dieuChuyenTaiSans.Update(phieuCho);
                }

                await _dbContext.SaveChangesAsync();
                return ResponseConst.Success("Xác nhận nhận tài sản và chốt phiếu thành công!", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xác nhận tài sản.");
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<List<TaiSan>>> GetTaiSanByUserIdAsync(int userId)
        {
            try
            {
                // Lấy các tài sản đang được gán cho User này (Chờ xác nhận hoặc Đang sử dụng)
                var result = await _dbContext.taiSans
                    .Where(x => x.NguoiDungId == userId &&
                               (x.TrangThai == TrangThaiTaiSan.ChoXacNhan || x.TrangThai == TrangThaiTaiSan.DangSuDung))
                    .OrderBy(x => x.TrangThai) // Đưa "Chờ xác nhận" lên đầu
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách tài sản cá nhân thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi lấy tài sản user.");
                return ResponseConst.Error<List<TaiSan>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}