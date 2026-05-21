using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        Task<ResponseDto<bool>> RejectAssetAsync(int id);
        Task<ResponseDto<List<TaiSan>>> GetTaiSanByUserIdAsync(int userId);
        Task<ResponseDto<GenerateMaTaiSanResponse>> GenerateMaTaiSanAsync(int danhMucId);
        Task<ResponseDto<List<TaiSanResponse>>> GetTaiSanByPhongBanIdAsync(int phongBanId);
    }

    public class TaiSanService : AssetServiceBase, ITaiSanService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<TaiSanService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaiSanService(ILogger<TaiSanService> logger, AssetDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        private int? GetCurrentUserId()
        {
            var userClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId");
            if (userClaim != null && int.TryParse(userClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
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
        // 0. SINH MÃ TÀI SẢN TỰ ĐỘNG THEO CẤU HÌNH
        // ==========================================
        public async Task<ResponseDto<GenerateMaTaiSanResponse>> GenerateMaTaiSanAsync(int danhMucId)
        {
            try
            {
                var danhMuc = await _dbContext.danhMucTaiSans.FindAsync(danhMucId);
                if (danhMuc == null)
                    return ResponseConst.Error<GenerateMaTaiSanResponse>(404, "Danh mục tài sản không tồn tại.");

                var cauHinh = await _dbContext.cauHinhHeThongs.FirstOrDefaultAsync();
                var format = cauHinh?.DinhDangMaTaiSan ?? "{DANH_MUC}-{SO_THU_TU}";
                var doDai = cauHinh?.DoDaiMaTaiSan ?? 4;

                var prefix = (danhMuc.TienTo ?? danhMuc.MaDanhMuc).ToUpper();

                // Tìm số thứ tự cao nhất hiện có cho danh mục này
                var existingCodes = await _dbContext.taiSans
                    .Where(x => x.MaTaiSan != null && x.MaTaiSan.StartsWith(prefix + "-"))
                    .Select(x => x.MaTaiSan!)
                    .ToListAsync();

                int nextNum = 1;
                foreach (var code in existingCodes)
                {
                    var suffix = code[(prefix.Length + 1)..];
                    if (int.TryParse(suffix, out int num) && num >= nextNum)
                        nextNum = num + 1;
                }

                var paddedNum = nextNum.ToString().PadLeft(doDai, '0');
                var maTaiSan = format
                    .Replace("{DANH_MUC}", prefix)
                    .Replace("{SO_THU_TU}", paddedNum);

                return ResponseConst.Success("Sinh mã tài sản thành công.", new GenerateMaTaiSanResponse
                {
                    maTaiSan = maTaiSan,
                    dinhDangApDung = format,
                    soThuTuTiepTheo = nextNum
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi sinh mã tài sản. DanhMucId: {Id}", danhMucId);
                return ResponseConst.Error<GenerateMaTaiSanResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 1. TẠO MỚI TÀI SẢN
        // ==========================================
        public async Task<ResponseDto<bool>> CreateTaiSanAsync(CreateTaiSanRequestDto request)
        {
            // Bọc bằng Execution Strategy để tránh lỗi Transaction với Npgsql Retry
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
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

                    // 3. Lấy cấu hình để áp dụng phương pháp khấu hao mặc định
                    var cauHinh = await _dbContext.cauHinhHeThongs.FirstOrDefaultAsync();

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
                        GiaTriConLai = request.nguyenGia ?? 0,
                        KhauHaoLuyKe = 0,
                        KhauHaoHangThang = 0,
                        PhuongPhapKhauHao = request.phuongPhapKhauHao ?? cauHinh?.PhuongPhapKhauHaoMacDinh,
                        ThoiGianKhauHao = request.thoiGianKhauHao,
                        MaTaiKhoan = request.maTaiKhoan,
                        PhongBanId = request.phongBanId,
                        NguoiDungId = request.nguoiDungId,
                        NgayCapPhat = request.nguoiDungId.HasValue
                                    ? (request.ngayCapPhat ?? DateTime.UtcNow)
                                    : null,
                        PhuongThucThanhToan = request.phuongThucThanhToan,
                        NgayTao = DateTime.UtcNow
                    };

                    _dbContext.taiSans.Add(taiSan);
                    await _dbContext.SaveChangesAsync(); // Lưu để lấy ID tài sản mới

                    // 4. TỰ ĐỘNG SINH PHIẾU CẤP PHÁT NẾU ĐỦ ĐIỀU KIỆN
                    // Điều kiện: Trạng thái là Chờ Xác Nhận (1) + Có Phòng Ban + Có Người Dùng
                    if (request.trangThai == TrangThaiTaiSan.ChoXacNhan
                        && request.phongBanId.HasValue
                        && request.nguoiDungId.HasValue)
                    {
                        var dieuChuyen = new DieuChuyenTaiSan
                        {
                            TaiSanId = taiSan.Id,
                            LoaiDieuChuyen = LoaiDieuChuyen.CapPhat,
                            NgayThucHien = taiSan.NgayCapPhat ?? DateTime.UtcNow,
                            DenPhongBanId = request.phongBanId,
                            DenNguoiDungId = request.nguoiDungId,
                            TrangThai = "cho_xu_ly", // Giữ trạng thái chờ để nhân viên vào xác nhận
                            GhiChu = "Tự động tạo phiếu cấp phát khi thêm mới tài sản",
                            NgayTao = DateTime.UtcNow
                        };

                        _dbContext.dieuChuyenTaiSans.Add(dieuChuyen);
                        await _dbContext.SaveChangesAsync(); // Lưu phiếu điều chuyển
                    }

                    await transaction.CommitAsync();
                    return ResponseConst.Success("Thêm tài sản thành công.", true);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi khi tạo tài sản.");
                    return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
                }
            });
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
                taiSan.PhuongThucThanhToan = request.phuongThucThanhToan;

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
        // 3. XÓA TÀI SẢN (XÓA TOÀN BỘ DỮ LIỆU LIÊN QUAN)
        // ==========================================
        public async Task<ResponseDto<bool>> DeleteTaiSanAsync(int id)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var taiSan = await _dbContext.taiSans.FirstOrDefaultAsync(x => x.Id == id);
                    if (taiSan == null)
                        return ResponseConst.Error<bool>(404, "Không tìm thấy tài sản.");

                    // 1. Lấy danh sách ChungTuId liên quan đến tài sản này (qua ChiTietChungTu)
                    var chungTuIds = await _dbContext.chiTietChungTus
                        .Where(x => x.TaiSanId == id)
                        .Select(x => x.ChungTuId)
                        .Distinct()
                        .ToListAsync();

                    // 2. Xóa LichSuKhauHao của tài sản này trước (tránh conflict FK kép với TaiSan và ChungTu)
                    var lichSuList = await _dbContext.lichSuKhauHaos.Where(x => x.TaiSanId == id).ToListAsync();
                    if (lichSuList.Any())
                    {
                        _dbContext.lichSuKhauHaos.RemoveRange(lichSuList);
                        await _dbContext.SaveChangesAsync();
                    }

                    // 3. Xóa các ChungTu liên quan (cascade tự xóa ChiTietChungTu)
                    if (chungTuIds.Any())
                    {
                        var chungTuList = await _dbContext.chungTus.Where(x => chungTuIds.Contains(x.Id)).ToListAsync();
                        _dbContext.chungTus.RemoveRange(chungTuList);
                        await _dbContext.SaveChangesAsync();
                    }

                    // 4. Xóa file đính kèm
                    var dinhKemList = await _dbContext.taiSanDinhKems.Where(x => x.TaiSanId == id).ToListAsync();
                    if (dinhKemList.Any())
                    {
                        _dbContext.taiSanDinhKems.RemoveRange(dinhKemList);
                        await _dbContext.SaveChangesAsync();
                    }

                    // 5. Xóa tài sản — cascade tự xóa: DieuChuyenTaiSan, BaoTriTaiSan, ThanhLyTaiSan, LichSuKhauHao còn lại
                    _dbContext.taiSans.Remove(taiSan);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return ResponseConst.Success("Xóa tài sản và toàn bộ dữ liệu liên quan thành công.", true);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi khi xóa tài sản. ID: {Id}", id);
                    return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
                }
            });
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
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var taiSan = await _dbContext.taiSans.FindAsync(id);
                    if (taiSan == null) return ResponseConst.Error<bool>(404, "Không tìm thấy tài sản.");

                    // Kiểm tra trạng thái
                    if (taiSan.TrangThai != TrangThaiTaiSan.ChoXacNhan)
                        return ResponseConst.Error<bool>(400, "Tài sản không ở trạng thái chờ xác nhận.");

                    // Kiểm tra quyền sở hữu: Chỉ người được gán tài sản mới có thể xác nhận
                    var currentUserId = GetCurrentUserId();
                    if (currentUserId == null)
                        return ResponseConst.Error<bool>(401, "Bạn chưa đăng nhập.");

                    if (taiSan.NguoiDungId != currentUserId)
                        return ResponseConst.Error<bool>(403, "Bạn chỉ có thể xác nhận tài sản của chính mình.");

                    // Chuyển tài sản sang Đang sử dụng
                    taiSan.TrangThai = TrangThaiTaiSan.DangSuDung;
                    taiSan.NgayCapPhat = DateTime.UtcNow;
                    _dbContext.taiSans.Update(taiSan);

                    // Tự động tìm Phiếu cấp phát/điều chuyển đang "Chờ xử lý" của tài sản này
                    var phieuCho = await _dbContext.dieuChuyenTaiSans
                        .Where(x => x.TaiSanId == id && x.TrangThai == "cho_xu_ly")
                        .FirstOrDefaultAsync();

                    if (phieuCho != null)
                    {
                        phieuCho.TrangThai = "da_hoan_thanh";
                        _dbContext.dieuChuyenTaiSans.Update(phieuCho);
                    }

                    // Tạo chứng từ ghi tăng
                    string taiKhoanCo = taiSan.PhuongThucThanhToan == PhuongThucThanhToan.ChuyenKhoan ? "112" : "111";

                    var chungTu = new ChungTu
                    {
                        MaChungTu = $"CT-GT-{DateTime.UtcNow:yyMMddHHmmss}-{taiSan.Id}",
                        NgayLap = DateTime.UtcNow,
                        LoaiChungTu = LoaiChungTu.GhiTang,
                        MoTa = $"Ghi tăng tài sản: {taiSan.TenTaiSan} ({taiSan.MaTaiSan})",
                        TongTien = taiSan.NguyenGia ?? 0,
                        TrangThai = "da_ghi_so",
                        NguoiLapId = taiSan.NguoiDungId,
                        NgayTao = DateTime.UtcNow
                    };

                    _dbContext.chungTus.Add(chungTu);
                    await _dbContext.SaveChangesAsync();

                    var chiTietChungTu = new ChiTietChungTu
                    {
                        ChungTuId = chungTu.Id,
                        TaiSanId = taiSan.Id,
                        TaiKhoanNo = taiSan.MaTaiKhoan,
                        TaiKhoanCo = taiKhoanCo,
                        SoTien = taiSan.NguyenGia ?? 0,
                        MoTa = $"Thanh toán mua tài sản {taiSan.MaTaiSan}"
                    };

                    _dbContext.chiTietChungTus.Add(chiTietChungTu);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return ResponseConst.Success("Xác nhận nhận tài sản và sinh chứng từ thành công!", true);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi khi xác nhận tài sản và sinh chứng từ.");
                    return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
                }
            });
        }

        public async Task<ResponseDto<bool>> RejectAssetAsync(int id)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var taiSan = await _dbContext.taiSans.FindAsync(id);
                    if (taiSan == null) return ResponseConst.Error<bool>(404, "Không tìm thấy tài sản.");

                    // Kiểm tra trạng thái: chỉ từ chối được khi đang chờ xác nhận
                    if (taiSan.TrangThai != TrangThaiTaiSan.ChoXacNhan)
                        return ResponseConst.Error<bool>(400, "Tài sản không ở trạng thái chờ xác nhận, không thể từ chối.");

                    // Kiểm tra quyền sở hữu: Chỉ người được gán tài sản mới có thể từ chối
                    var currentUserId = GetCurrentUserId();
                    if (currentUserId == null)
                        return ResponseConst.Error<bool>(401, "Bạn chưa đăng nhập.");

                    if (taiSan.NguoiDungId != currentUserId)
                        return ResponseConst.Error<bool>(403, "Bạn chỉ có thể từ chối tài sản của chính mình.");

                    // 1. Quay trạng thái tài sản về "Chưa cấp phát"
                    taiSan.TrangThai = TrangThaiTaiSan.ChuaCapPhat;
                    taiSan.NguoiDungId = null;
                    taiSan.NgayCapPhat = null;
                    _dbContext.taiSans.Update(taiSan);

                    // 2. Tìm phiếu cấp phát/điều chuyển đang "Chờ xử lý" và cập nhật thành "Từ chối"
                    var phieuCho = await _dbContext.dieuChuyenTaiSans
                        .Where(x => x.TaiSanId == id && x.TrangThai == "cho_xu_ly")
                        .FirstOrDefaultAsync();

                    if (phieuCho != null)
                    {
                        phieuCho.TrangThai = "tu_choi";
                        _dbContext.dieuChuyenTaiSans.Update(phieuCho);
                    }

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return ResponseConst.Success("Từ chối tài sản thành công!", true);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi khi từ chối tài sản.");
                    return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
                }
            });
        }

        public async Task<ResponseDto<List<TaiSanResponse>>> GetTaiSanByPhongBanIdAsync(int phongBanId)
        {
            try
            {
                var result = await _dbContext.taiSans
                    .Where(x => x.PhongBanId == phongBanId
                             && x.TrangThai != TrangThaiTaiSan.DaThanhLy)
                    .Select(x => new TaiSanResponse
                    {
                        id = x.Id,
                        maTaiSan = x.MaTaiSan,
                        tenTaiSan = x.TenTaiSan,
                        trangThai = x.TrangThai,
                        ngayCapPhat = x.NgayCapPhat,
                        nguoiDungId = x.NguoiDungId,
                        phongBanId = x.PhongBanId,
                        soSeri = x.SoSeri,
                        nhaSanXuat = x.NhaSanXuat
                    })
                    .OrderBy(x => x.nguoiDungId == null ? 1 : 0)
                    .ToListAsync();

                return ResponseConst.Success("Lấy tài sản phòng ban thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi lấy tài sản theo phòng ban.");
                return ResponseConst.Error<List<TaiSanResponse>>(500, "Lỗi hệ thống: " + ex.Message);
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