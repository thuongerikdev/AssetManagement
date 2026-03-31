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
    public interface IDieuChuyenTaiSanService
    {
        Task<ResponseDto<bool>> CreateDieuChuyenTaiSanAsync(CreateDieuChuyenTaiSanRequestDto request);
        Task<ResponseDto<bool>> UpdateDieuChuyenTaiSanAsync(UpdateDieuChuyenTaiSanRequestDto request);
        Task<ResponseDto<bool>> DeleteDieuChuyenTaiSanAsync(int id);
        Task<ResponseDto<List<DieuChuyenTaiSanResponse>>> GetAllDieuChuyenTaiSanAsync();
        Task<ResponseDto<DieuChuyenTaiSanResponse>> GetDieuChuyenTaiSanByIdAsync(int id);
    }

    public class DieuChuyenTaiSanService : AssetServiceBase, IDieuChuyenTaiSanService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<DieuChuyenTaiSanService> _logger;

        public DieuChuyenTaiSanService(ILogger<DieuChuyenTaiSanService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // ==========================================
        // 1. TẠO MỚI PHIẾU ĐIỀU CHUYỂN
        // ==========================================
        public async Task<ResponseDto<bool>> CreateDieuChuyenTaiSanAsync(CreateDieuChuyenTaiSanRequestDto request)
        {
            try
            {
                // 1. Kiểm tra tài sản có tồn tại không
                var taiSan = await _dbContext.taiSans.FirstOrDefaultAsync(x => x.Id == request.taiSanId);
                if (taiSan == null)
                {
                    return ResponseConst.Error<bool>(404, "Tài sản không tồn tại.");
                }

                // 2. Validate Phòng ban
                if (request.tuPhongBanId.HasValue && !await _dbContext.phongBans.AnyAsync(x => x.Id == request.tuPhongBanId))
                    return ResponseConst.Error<bool>(400, "Phòng ban xuất phát (Từ phòng ban) không tồn tại.");

                if (request.denPhongBanId.HasValue && !await _dbContext.phongBans.AnyAsync(x => x.Id == request.denPhongBanId))
                    return ResponseConst.Error<bool>(400, "Phòng ban đích (Đến phòng ban) không tồn tại.");

                // 3. Tạo phiếu điều chuyển (Mặc định: Chờ xử lý)
                var dieuChuyen = new DieuChuyenTaiSan
                {
                    TaiSanId = request.taiSanId,
                    LoaiDieuChuyen = request.loaiDieuChuyen,
                    NgayThucHien = request.ngayThucHien,
                    TuPhongBanId = request.tuPhongBanId,
                    DenPhongBanId = request.denPhongBanId,
                    TuNguoiDungId = request.tuNguoiDungId,
                    DenNguoiDungId = request.denNguoiDungId,
                    GhiChu = request.ghiChu,
                    // SỬA: Phiếu mới tạo sẽ ở trạng thái chờ xử lý (chờ người dùng xác nhận)
                    TrangThai = "cho_xu_ly",
                    NgayTao = DateTime.UtcNow
                };

                _dbContext.dieuChuyenTaiSans.Add(dieuChuyen);

                // 4. Cập nhật vị trí tạm thời và trạng thái Tài sản thành "Chờ Xác Nhận"
                taiSan.PhongBanId = request.denPhongBanId;
                taiSan.NguoiDungId = request.denNguoiDungId;
                taiSan.TrangThai = TrangThaiTaiSan.ChoXacNhan;

                // Nếu là cấp phát lần đầu, gán ngày cấp phát
                if (request.loaiDieuChuyen == LoaiDieuChuyen.CapPhat && !taiSan.NgayCapPhat.HasValue)
                {
                    taiSan.NgayCapPhat = request.ngayThucHien ?? DateTime.UtcNow;
                }

                _dbContext.taiSans.Update(taiSan);

                // Lưu tất cả vào Database
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Tạo phiếu điều chuyển tài sản thành công. Đang chờ người dùng xác nhận.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo phiếu điều chuyển tài sản.");
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 2. CẬP NHẬT PHIẾU ĐIỀU CHUYỂN
        // ==========================================
        public async Task<ResponseDto<bool>> UpdateDieuChuyenTaiSanAsync(UpdateDieuChuyenTaiSanRequestDto request)
        {
            try
            {
                var dieuChuyen = await _dbContext.dieuChuyenTaiSans.FirstOrDefaultAsync(x => x.Id == request.id);
                if (dieuChuyen == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy phiếu điều chuyển.");
                }

                // Kiểm tra xem phiếu có vừa được chuyển sang "Hoàn thành" không
                bool isJustCompleted = dieuChuyen.TrangThai != "da_hoan_thanh" && request.trangThai == "da_hoan_thanh";

                // Cập nhật các trường
                dieuChuyen.LoaiDieuChuyen = request.loaiDieuChuyen;
                dieuChuyen.NgayThucHien = request.ngayThucHien;
                dieuChuyen.TuPhongBanId = request.tuPhongBanId;
                dieuChuyen.DenPhongBanId = request.denPhongBanId;
                dieuChuyen.TuNguoiDungId = request.tuNguoiDungId;
                dieuChuyen.DenNguoiDungId = request.denNguoiDungId;
                dieuChuyen.GhiChu = request.ghiChu;
                dieuChuyen.TrangThai = request.trangThai ?? dieuChuyen.TrangThai;

                _dbContext.dieuChuyenTaiSans.Update(dieuChuyen);

                // NẾU NGƯỜI DÙNG XÁC NHẬN (Phiếu -> Đã hoàn thành)
                if (isJustCompleted && dieuChuyen.TaiSanId.HasValue)
                {
                    var taiSan = await _dbContext.taiSans.FirstOrDefaultAsync(x => x.Id == dieuChuyen.TaiSanId);
                    if (taiSan != null)
                    {
                        // Chuyển tài sản thành Đang sử dụng
                        taiSan.TrangThai = TrangThaiTaiSan.DangSuDung;
                        // Chốt lại vị trí lần cuối cùng cho chắc chắn
                        taiSan.PhongBanId = request.denPhongBanId;
                        taiSan.NguoiDungId = request.denNguoiDungId;

                        _dbContext.taiSans.Update(taiSan);
                    }
                }

                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Cập nhật phiếu điều chuyển thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật phiếu điều chuyển. ID: {Id}", request.id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 3. XÓA PHIẾU ĐIỀU CHUYỂN
        // ==========================================
        public async Task<ResponseDto<bool>> DeleteDieuChuyenTaiSanAsync(int id)
        {
            try
            {
                var dieuChuyen = await _dbContext.dieuChuyenTaiSans.FirstOrDefaultAsync(x => x.Id == id);
                if (dieuChuyen == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy phiếu điều chuyển.");
                }

                _dbContext.dieuChuyenTaiSans.Remove(dieuChuyen);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Xóa phiếu điều chuyển thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa phiếu điều chuyển. ID: {Id}", id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 4. LẤY DANH SÁCH ĐIỀU CHUYỂN
        // ==========================================
        public async Task<ResponseDto<List<DieuChuyenTaiSanResponse>>> GetAllDieuChuyenTaiSanAsync()
        {
            try
            {
                var result = await _dbContext.dieuChuyenTaiSans
                    .Select(x => new DieuChuyenTaiSanResponse
                    {
                        id = x.Id,
                        taiSanId = x.TaiSanId,
                        loaiDieuChuyen = x.LoaiDieuChuyen,
                        ngayThucHien = x.NgayThucHien,
                        tuPhongBanId = x.TuPhongBanId,
                        denPhongBanId = x.DenPhongBanId,
                        tuNguoiDungId = x.TuNguoiDungId,
                        denNguoiDungId = x.DenNguoiDungId,
                        trangThai = x.TrangThai,
                        ghiChu = x.GhiChu,
                        ngayTao = x.NgayTao
                    })
                    .OrderByDescending(x => x.ngayThucHien)
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách phiếu điều chuyển thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách điều chuyển.");
                return ResponseConst.Error<List<DieuChuyenTaiSanResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 5. LẤY CHI TIẾT ĐIỀU CHUYỂN THEO ID
        // ==========================================
        public async Task<ResponseDto<DieuChuyenTaiSanResponse>> GetDieuChuyenTaiSanByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.dieuChuyenTaiSans
                    .Where(x => x.Id == id)
                    .Select(x => new DieuChuyenTaiSanResponse
                    {
                        id = x.Id,
                        taiSanId = x.TaiSanId,
                        loaiDieuChuyen = x.LoaiDieuChuyen,
                        ngayThucHien = x.NgayThucHien,
                        tuPhongBanId = x.TuPhongBanId,
                        denPhongBanId = x.DenPhongBanId,
                        tuNguoiDungId = x.TuNguoiDungId,
                        denNguoiDungId = x.DenNguoiDungId,
                        trangThai = x.TrangThai,
                        ghiChu = x.GhiChu,
                        ngayTao = x.NgayTao
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return ResponseConst.Error<DieuChuyenTaiSanResponse>(404, "Không tìm thấy phiếu điều chuyển.");
                }

                return ResponseConst.Success("Lấy chi tiết phiếu điều chuyển thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết phiếu điều chuyển. ID: {Id}", id);
                return ResponseConst.Error<DieuChuyenTaiSanResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}