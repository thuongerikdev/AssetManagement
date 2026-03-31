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
    public interface IBaoTriTaiSanService
    {
        Task<ResponseDto<bool>> CreateBaoTriTaiSanAsync(CreateBaoTriTaiSanRequestDto request);
        Task<ResponseDto<bool>> UpdateBaoTriTaiSanAsync(UpdateBaoTriTaiSanRequestDto request);
        Task<ResponseDto<bool>> DeleteBaoTriTaiSanAsync(int id);
        Task<ResponseDto<List<BaoTriTaiSanResponse>>> GetAllBaoTriTaiSanAsync();
        Task<ResponseDto<BaoTriTaiSanResponse>> GetBaoTriTaiSanByIdAsync(int id);
    }

    public class BaoTriTaiSanService : AssetServiceBase, IBaoTriTaiSanService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<BaoTriTaiSanService> _logger;

        public BaoTriTaiSanService(ILogger<BaoTriTaiSanService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // ==========================================
        // 1. TẠO MỚI PHIẾU BẢO TRÌ
        // ==========================================
        public async Task<ResponseDto<bool>> CreateBaoTriTaiSanAsync(CreateBaoTriTaiSanRequestDto request)
        {
            // 1. Tạo Execution Strategy
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            // 2. Bọc toàn bộ logic cũ vào trong strategy.ExecuteAsync
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var taiSan = await _dbContext.taiSans.FirstOrDefaultAsync(x => x.Id == request.taiSanId);
                    if (taiSan == null)
                    {
                        return ResponseConst.Error<bool>(404, "Tài sản không tồn tại.");
                    }

                    var baoTri = new BaoTriTaiSan
                    {
                        TrangThai = request.trangThai ?? TrangThaiBaoTri.ChoXuLy,
                        TaiSanId = request.taiSanId,
                        NgayThucHien = request.ngayThucHien,
                        LoaiBaoTri = request.loaiBaoTri,
                        MoTa = request.moTa,
                        CoChiPhi = request.coChiPhi,
                        ChiPhi = request.coChiPhi == true ? request.chiPhi : 0,
                        LoaiChiPhi = request.loaiChiPhi,
                        NhaCungCap = request.nhaCungCap,
                        GhiChu = request.ghiChu,
                        NgayTao = DateTime.UtcNow
                    };

                    _dbContext.baoTriTaiSans.Add(baoTri);
                    await _dbContext.SaveChangesAsync();

                    //// LOGIC TỰ ĐỘNG SINH CHỨNG TỪ
                    //if (baoTri.TrangThai == TrangThaiBaoTri.DaHoanThanh && baoTri.CoChiPhi == true && baoTri.ChiPhi > 0) // <--- Sửa số 2 thành TrangThaiBaoTri.DaHoanThanh

                    //    {
                    //    var chungTu = new ChungTu
                    //    {
                    //        MaChungTu = $"BT-{DateTime.Now:yyyyMMdd}-{baoTri.Id}",
                    //        NgayLap = baoTri.NgayThucHien,
                    //        LoaiChungTu = (LoaiChungTu)2,
                    //        MoTa = $"Chi phí bảo trì/sửa chữa TSCĐ {taiSan.MaTaiSan}",
                    //        TongTien = baoTri.ChiPhi ?? 0,
                    //        TrangThai = "nhap",
                    //        NgayTao = DateTime.UtcNow
                    //    };

                    //    _dbContext.chungTus.Add(chungTu);
                    //    await _dbContext.SaveChangesAsync();

                    //    var chiTietList = new List<ChiTietChungTu>();

                    //    if (baoTri.LoaiChiPhi == "nang_cap")
                    //    {
                    //        chiTietList.Add(new ChiTietChungTu { ChungTuId = chungTu.Id, TaiSanId = taiSan.Id, TaiKhoanNo = "211", SoTien = baoTri.ChiPhi ?? 0, MoTa = "Tăng nguyên giá do nâng cấp TSCĐ" });
                    //        chiTietList.Add(new ChiTietChungTu { ChungTuId = chungTu.Id, TaiSanId = taiSan.Id, TaiKhoanCo = "111", SoTien = baoTri.ChiPhi ?? 0, MoTa = "Thanh toán chi phí nâng cấp TSCĐ" });

                    //        taiSan.NguyenGia = (taiSan.NguyenGia ?? 0) + baoTri.ChiPhi;
                    //        taiSan.GiaTriConLai = (taiSan.GiaTriConLai ?? 0) + baoTri.ChiPhi;
                    //        _dbContext.taiSans.Update(taiSan);
                    //    }
                    //    else
                    //    {
                    //        chiTietList.Add(new ChiTietChungTu { ChungTuId = chungTu.Id, TaiSanId = taiSan.Id, TaiKhoanNo = "627", SoTien = baoTri.ChiPhi ?? 0, MoTa = "Chi phí sửa chữa, bảo dưỡng TSCĐ" });
                    //        chiTietList.Add(new ChiTietChungTu { ChungTuId = chungTu.Id, TaiSanId = taiSan.Id, TaiKhoanCo = "111", SoTien = baoTri.ChiPhi ?? 0, MoTa = "Thanh toán chi phí bảo dưỡng" });
                    //    }

                    //    _dbContext.chiTietChungTus.AddRange(chiTietList);
                    //}

                    //await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return ResponseConst.Success("Tạo phiếu bảo trì tài sản thành công.", true);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi khi tạo phiếu bảo trì tài sản.");
                    return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
                }
            }); // Kết thúc ExecuteAsync
        }

        // ==========================================
        // 2. CẬP NHẬT PHIẾU BẢO TRÌ
        // ==========================================
        public async Task<ResponseDto<bool>> UpdateBaoTriTaiSanAsync(UpdateBaoTriTaiSanRequestDto request)
        {
            // 1. Tạo Execution Strategy
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            // 2. Bọc toàn bộ logic vào trong strategy.ExecuteAsync
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var baoTri = await _dbContext.baoTriTaiSans.FirstOrDefaultAsync(x => x.Id == request.id);
                    if (baoTri == null)
                    {
                        return ResponseConst.Error<bool>(404, "Phiếu bảo trì không tồn tại.");
                    }

                    // --- BƯỚC QUAN TRỌNG: LƯU LẠI TRẠNG THÁI CŨ ---
                    bool wasAlreadyCompleted = baoTri.TrangThai == TrangThaiBaoTri.DaHoanThanh;
                    bool isNowCompleted = request.trangThai == TrangThaiBaoTri.DaHoanThanh;

                    // Cập nhật thông tin phiếu bảo trì
                    baoTri.NgayThucHien = request.ngayThucHien;
                    baoTri.LoaiBaoTri = request.loaiBaoTri;
                    baoTri.MoTa = request.moTa;
                    baoTri.CoChiPhi = request.coChiPhi;
                    baoTri.ChiPhi = request.coChiPhi == true ? request.chiPhi : 0;
                    baoTri.LoaiChiPhi = request.loaiChiPhi;
                    baoTri.NhaCungCap = request.nhaCungCap;
                    baoTri.TrangThai = request.trangThai ?? baoTri.TrangThai;
                    baoTri.GhiChu = request.ghiChu;

                    _dbContext.baoTriTaiSans.Update(baoTri);
                    await _dbContext.SaveChangesAsync();

                    //// --- LOGIC TỰ ĐỘNG SINH CHỨNG TỪ ---
                    //// Chỉ sinh khi CHUYỂN TỪ trạng thái khác SANG Hoàn thành VÀ có chi phí
                    //if (!wasAlreadyCompleted && isNowCompleted && baoTri.CoChiPhi == true && baoTri.ChiPhi > 0)
                    //{
                    //    var taiSan = await _dbContext.taiSans.FirstOrDefaultAsync(x => x.Id == baoTri.TaiSanId);
                    //    if (taiSan != null)
                    //    {
                    //        var chungTu = new ChungTu
                    //        {
                    //            MaChungTu = $"BT-{DateTime.Now:yyyyMMdd}-{baoTri.Id}",
                    //            NgayLap = baoTri.NgayThucHien ?? DateTime.UtcNow,
                    //            LoaiChungTu = (LoaiChungTu)2, // 2 = Bảo trì
                    //            MoTa = $"Chi phí bảo trì/sửa chữa TSCĐ {taiSan.MaTaiSan}",
                    //            TongTien = baoTri.ChiPhi ?? 0,
                    //            TrangThai = "nhap",
                    //            NgayTao = DateTime.UtcNow
                    //        };

                    //        _dbContext.chungTus.Add(chungTu);
                    //        await _dbContext.SaveChangesAsync();

                    //        var chiTietList = new List<ChiTietChungTu>();

                    //        // Xác định Tài khoản Có
                    //        string taiKhoanCo = !string.IsNullOrEmpty(baoTri.NhaCungCap) ? "331" : "111";
                    //        string moTaTaiKhoanCo = taiKhoanCo == "331" ? "Công nợ bảo trì/nâng cấp TSCĐ" : "Thanh toán tiền mặt bảo trì/nâng cấp TSCĐ";

                    //        if (baoTri.LoaiChiPhi == "nang_cap")
                    //        {
                    //            string taiKhoanNo = !string.IsNullOrEmpty(taiSan.MaTaiKhoan) ? taiSan.MaTaiKhoan : "211";

                    //            chiTietList.Add(new ChiTietChungTu { ChungTuId = chungTu.Id, TaiSanId = taiSan.Id, TaiKhoanNo = taiKhoanNo, SoTien = baoTri.ChiPhi ?? 0, MoTa = "Tăng nguyên giá do nâng cấp TSCĐ" });
                    //            chiTietList.Add(new ChiTietChungTu { ChungTuId = chungTu.Id, TaiSanId = taiSan.Id, TaiKhoanCo = taiKhoanCo, SoTien = baoTri.ChiPhi ?? 0, MoTa = moTaTaiKhoanCo });

                    //            taiSan.NguyenGia = (taiSan.NguyenGia ?? 0) + baoTri.ChiPhi;
                    //            taiSan.GiaTriConLai = (taiSan.GiaTriConLai ?? 0) + baoTri.ChiPhi;
                    //            _dbContext.taiSans.Update(taiSan);
                    //        }
                    //        else
                    //        {
                    //            string taiKhoanChiPhi = "642";
                    //            if (taiSan.MaTaiKhoan == "2112") taiKhoanChiPhi = "627";
                    //            else if (taiSan.MaTaiKhoan == "2113") taiKhoanChiPhi = "641";

                    //            chiTietList.Add(new ChiTietChungTu { ChungTuId = chungTu.Id, TaiSanId = taiSan.Id, TaiKhoanNo = taiKhoanChiPhi, SoTien = baoTri.ChiPhi ?? 0, MoTa = $"Chi phí sửa chữa, bảo dưỡng TSCĐ ({taiKhoanChiPhi})" });
                    //            chiTietList.Add(new ChiTietChungTu { ChungTuId = chungTu.Id, TaiSanId = taiSan.Id, TaiKhoanCo = taiKhoanCo, SoTien = baoTri.ChiPhi ?? 0, MoTa = moTaTaiKhoanCo });
                    //        }

                    //        _dbContext.chiTietChungTus.AddRange(chiTietList);
                    //        await _dbContext.SaveChangesAsync();
                    //    }
                    //}

                    await transaction.CommitAsync();
                    return ResponseConst.Success("Cập nhật phiếu bảo trì thành công.", true);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi khi cập nhật phiếu bảo trì tài sản.");
                    return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
                }
            }); // Kết thúc ExecuteAsync
        }

        // ==========================================
        // 3. XÓA PHIẾU BẢO TRÌ
        // ==========================================
        public async Task<ResponseDto<bool>> DeleteBaoTriTaiSanAsync(int id)
        {
            try
            {
                var baoTri = await _dbContext.baoTriTaiSans.FirstOrDefaultAsync(x => x.Id == id);
                if (baoTri == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy phiếu bảo trì.");
                }

                // Ràng buộc nghiệp vụ: Không cho phép xóa phiếu bảo trì đã hoàn thành
                // (Vì nếu là nâng cấp, nó đã cộng tiền vào Tài sản rồi, xóa đi sẽ gây sai lệch dữ liệu kế toán)
                if (baoTri.TrangThai == TrangThaiBaoTri.DaHoanThanh)
                {
                    return ResponseConst.Error<bool>(400, "Không thể xóa phiếu bảo trì đã hoàn thành. Nếu có sai sót, vui lòng tạo phiếu điều chỉnh.");
                }

                _dbContext.baoTriTaiSans.Remove(baoTri);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Xóa phiếu bảo trì thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa phiếu bảo trì. ID: {Id}", id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 4. LẤY DANH SÁCH BẢO TRÌ
        // ==========================================
        public async Task<ResponseDto<List<BaoTriTaiSanResponse>>> GetAllBaoTriTaiSanAsync()
        {
            try
            {
                var result = await _dbContext.baoTriTaiSans
                    .Select(x => new BaoTriTaiSanResponse
                    {
                        id = x.Id,
                        taiSanId = x.TaiSanId,
                        ngayThucHien = x.NgayThucHien,
                        loaiBaoTri = x.LoaiBaoTri,
                        moTa = x.MoTa,
                        coChiPhi = x.CoChiPhi,
                        chiPhi = x.ChiPhi,
                        loaiChiPhi = x.LoaiChiPhi,
                        nhaCungCap = x.NhaCungCap,
                        trangThai = x.TrangThai,
                        ghiChu = x.GhiChu,
                        ngayTao = x.NgayTao
                    })
                    .OrderByDescending(x => x.ngayTao)
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách phiếu bảo trì thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phiếu bảo trì.");
                return ResponseConst.Error<List<BaoTriTaiSanResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 5. LẤY CHI TIẾT BẢO TRÌ THEO ID
        // ==========================================
        public async Task<ResponseDto<BaoTriTaiSanResponse>> GetBaoTriTaiSanByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.baoTriTaiSans
                    .Where(x => x.Id == id)
                    .Select(x => new BaoTriTaiSanResponse
                    {
                        id = x.Id,
                        taiSanId = x.TaiSanId,
                        ngayThucHien = x.NgayThucHien,
                        loaiBaoTri = x.LoaiBaoTri,
                        moTa = x.MoTa,
                        coChiPhi = x.CoChiPhi,
                        chiPhi = x.ChiPhi,
                        loaiChiPhi = x.LoaiChiPhi,
                        nhaCungCap = x.NhaCungCap,
                        trangThai = x.TrangThai,
                        ghiChu = x.GhiChu,
                        ngayTao = x.NgayTao
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return ResponseConst.Error<BaoTriTaiSanResponse>(404, "Không tìm thấy phiếu bảo trì.");
                }

                return ResponseConst.Success("Lấy chi tiết phiếu bảo trì thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết phiếu bảo trì. ID: {Id}", id);
                return ResponseConst.Error<BaoTriTaiSanResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}