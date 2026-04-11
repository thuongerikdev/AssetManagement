using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Common;
using TH.Asset.Domain.Entities;
using TH.Asset.Dtos;
using TH.Asset.Infrastructure.Database;
using TH.Constant;

namespace TH.Asset.ApplicationService.Service
{
    public interface IChungTuService
    {
        Task<ResponseDto<bool>> CreateChungTuAsync(CreateChungTuRequestDto request);
        Task<ResponseDto<bool>> UpdateChungTuAsync(UpdateChungTuRequestDto request);
        Task<ResponseDto<bool>> DeleteChungTuAsync(int id);
        Task<ResponseDto<List<ChungTuResponse>>> GetAllChungTuAsync();
        Task<ResponseDto<ChungTuResponse>> GetChungTuByIdAsync(int id);

        Task<ResponseDto<List<ChungTuResponse>>> GetChungTuByTaiSanIdAsync(int taiSanId);
    }

    public class ChungTuService : AssetServiceBase, IChungTuService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<ChungTuService> _logger;

        public ChungTuService(ILogger<ChungTuService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // ==========================================
        // 1. TẠO CHỨNG TỪ (KÈM CHI TIẾT)
        // ==========================================
        public async Task<ResponseDto<bool>> CreateChungTuAsync(CreateChungTuRequestDto request)
        {
            // Bắt đầu một Transaction thủ công để đảm bảo Master-Detail được lưu trọn vẹn
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // 1. Kiểm tra trùng mã chứng từ
                var isExist = await _dbContext.chungTus.AnyAsync(x => x.MaChungTu == request.maChungTu);
                if (isExist)
                {
                    return ResponseConst.Error<bool>(400, "Mã chứng từ đã tồn tại trong hệ thống.");
                }

                // 2. Tính toán tổng tiền từ các dòng chi tiết (nếu có)
                decimal calculatedTongTien = 0;
                if (request.chiTietChungTus != null && request.chiTietChungTus.Any())
                {
                    calculatedTongTien = request.chiTietChungTus.Sum(x => x.soTien ?? 0);
                }

                // 3. Khởi tạo Chứng từ (Master)
                var chungTu = new ChungTu
                {
                    MaChungTu = request.maChungTu,
                    NgayLap = request.ngayLap ?? DateTime.UtcNow,
                    LoaiChungTu = request.loaiChungTu,
                    MoTa = request.moTa,
                    TongTien = calculatedTongTien > 0 ? calculatedTongTien : request.tongTien,
                    TrangThai = request.trangThai ?? "nhap", // Mặc định là 'nhap' (Nháp)
                    NguoiLapId = request.nguoiLapId,
                    NgayTao = DateTime.UtcNow
                };

                _dbContext.chungTus.Add(chungTu);
                await _dbContext.SaveChangesAsync(); // Lưu để lấy ChungTuId cho các dòng Detail

                // 4. Khởi tạo Chi tiết chứng từ (Detail)
                if (request.chiTietChungTus != null && request.chiTietChungTus.Any())
                {
                    var chiTietList = request.chiTietChungTus.Select(ct => new ChiTietChungTu
                    {
                        ChungTuId = chungTu.Id,
                        TaiKhoanNo = string.IsNullOrEmpty(ct.taiKhoanNo) ? null : ct.taiKhoanNo,
                        TaiKhoanCo = string.IsNullOrEmpty(ct.taiKhoanCo) ? null : ct.taiKhoanCo,
                        SoTien = ct.soTien ?? 0,
                        MoTa = ct.moTa,
                        TaiSanId = ct.taiSanId
                    }).ToList();

                    _dbContext.chiTietChungTus.AddRange(chiTietList);
                    await _dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return ResponseConst.Success("Tạo chứng từ thành công.", true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Lỗi khi tạo chứng từ.");
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 2. CẬP NHẬT CHỨNG TỪ
        // ==========================================
        public async Task<ResponseDto<bool>> UpdateChungTuAsync(UpdateChungTuRequestDto request)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Lấy chứng từ kèm danh sách chi tiết hiện tại
                var chungTu = await _dbContext.chungTus
                    .Include(x => x.ChiTietChungTus)
                    .FirstOrDefaultAsync(x => x.Id == request.id);

                if (chungTu == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy chứng từ.");
                }

                // Ràng buộc kế toán: Không cho sửa chứng từ đã ghi sổ
                if (chungTu.TrangThai == "da_ghi_so")
                {
                    return ResponseConst.Error<bool>(400, "Không thể sửa chứng từ đã được ghi sổ kế toán.");
                }

                if (chungTu.MaChungTu != request.maChungTu)
                {
                    var isExist = await _dbContext.chungTus.AnyAsync(x => x.MaChungTu == request.maChungTu);
                    if (isExist) return ResponseConst.Error<bool>(400, "Mã chứng từ mới đã tồn tại.");
                }

                // Cập nhật Master
                chungTu.MaChungTu = request.maChungTu;
                chungTu.NgayLap = request.ngayLap;
                chungTu.LoaiChungTu = request.loaiChungTu;
                chungTu.MoTa = request.moTa;
                chungTu.TrangThai = request.trangThai;
                // Người lập thường không đổi, nhưng nếu FE truyền thì cập nhật
                chungTu.NguoiLapId = request.nguoiLapId ?? chungTu.NguoiLapId;

                // Xử lý Detail: Xóa các chi tiết cũ và thêm mới (Cách đơn giản và an toàn nhất cho Master-Detail)
                if (request.chiTietChungTus != null)
                {
                    _dbContext.chiTietChungTus.RemoveRange(chungTu.ChiTietChungTus!);

                    var newDetails = request.chiTietChungTus.Select(ct => new ChiTietChungTu
                    {
                        ChungTuId = chungTu.Id,
                        TaiKhoanNo = string.IsNullOrEmpty(ct.taiKhoanNo) ? null : ct.taiKhoanNo,
                        TaiKhoanCo = string.IsNullOrEmpty(ct.taiKhoanCo) ? null : ct.taiKhoanCo,
                        SoTien = ct.soTien ?? 0,
                        MoTa = ct.moTa,
                        TaiSanId = ct.taiSanId
                    }).ToList();

                    _dbContext.chiTietChungTus.AddRange(newDetails);

                    // Cập nhật lại tổng tiền
                    chungTu.TongTien = newDetails.Sum(x => x.SoTien);
                }
                else
                {
                    chungTu.TongTien = request.tongTien;
                }

                _dbContext.chungTus.Update(chungTu);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return ResponseConst.Success("Cập nhật chứng từ thành công.", true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Lỗi khi cập nhật chứng từ. ID: {Id}", request.id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 3. XÓA CHỨNG TỪ
        // ==========================================
        public async Task<ResponseDto<bool>> DeleteChungTuAsync(int id)
        {
            try
            {
                var chungTu = await _dbContext.chungTus.FirstOrDefaultAsync(x => x.Id == id);
                if (chungTu == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy chứng từ.");
                }

                // Ràng buộc kế toán: Không cho xóa chứng từ đã ghi sổ
                if (chungTu.TrangThai == "da_ghi_so")
                {
                    return ResponseConst.Error<bool>(400, "Không thể xóa chứng từ đã được ghi sổ kế toán. Vui lòng lập chứng từ đảo nếu có sai sót.");
                }

                // Nhờ EF Core đã config Cascade Delete ở DbContext, khi xóa ChungTu sẽ tự xóa các ChiTietChungTu liên quan
                _dbContext.chungTus.Remove(chungTu);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Xóa chứng từ thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa chứng từ. ID: {Id}", id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 4. LẤY DANH SÁCH CHỨNG TỪ
        // ==========================================
        public async Task<ResponseDto<List<ChungTuResponse>>> GetAllChungTuAsync()
        {
            try
            {
                // Thông thường lấy danh sách sẽ không include chi tiết để API chạy nhanh hơn, chỉ include khi lấy ById
                var result = await _dbContext.chungTus
                    .Include(x => x.ChiTietChungTus)
                    .Select(x => new ChungTuResponse
                    {
                        id = x.Id,
                        maChungTu = x.MaChungTu,
                        ngayLap = x.NgayLap,
                        loaiChungTu = x.LoaiChungTu,
                        moTa = x.MoTa,
                        tongTien = x.TongTien,
                        trangThai = x.TrangThai,
                        nguoiLapId = x.NguoiLapId,
                        ngayTao = x.NgayTao,
                        chiTietChungTus = x.ChiTietChungTus!.Select(ct => new ChiTietChungTuResponse
                        {
                            id = ct.Id,
                            chungTuId = ct.ChungTuId,
                            taiKhoanNo = ct.TaiKhoanNo,
                            taiKhoanCo = ct.TaiKhoanCo,
                            soTien = ct.SoTien,
                            moTa = ct.MoTa,
                            taiSanId = ct.TaiSanId
                        }).ToList()
                    })
                    

                    .OrderByDescending(x => x.ngayLap)
                    .ThenByDescending(x => x.ngayTao)
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách chứng từ thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách chứng từ.");
                return ResponseConst.Error<List<ChungTuResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // ==========================================
        // 5. LẤY CHI TIẾT CHỨNG TỪ (KÈM DÒNG HẠCH TOÁN)
        // ==========================================
        public async Task<ResponseDto<ChungTuResponse>> GetChungTuByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.chungTus
                    .Include(x => x.ChiTietChungTus)
                    .Where(x => x.Id == id)
                    .Select(x => new ChungTuResponse
                    {
                        id = x.Id,
                        maChungTu = x.MaChungTu,
                        ngayLap = x.NgayLap,
                        loaiChungTu = x.LoaiChungTu,
                        moTa = x.MoTa,
                        tongTien = x.TongTien,
                        trangThai = x.TrangThai,
                        nguoiLapId = x.NguoiLapId,
                        ngayTao = x.NgayTao,
                        chiTietChungTus = x.ChiTietChungTus!.Select(ct => new ChiTietChungTuResponse
                        {
                            id = ct.Id,
                            chungTuId = ct.ChungTuId,
                            taiKhoanNo = ct.TaiKhoanNo,
                            taiKhoanCo = ct.TaiKhoanCo,
                            soTien = ct.SoTien,
                            moTa = ct.MoTa,
                            taiSanId = ct.TaiSanId
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return ResponseConst.Error<ChungTuResponse>(404, "Không tìm thấy chứng từ.");
                }

                return ResponseConst.Success("Lấy chi tiết chứng từ thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết chứng từ. ID: {Id}", id);
                return ResponseConst.Error<ChungTuResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
        public async Task<ResponseDto<List<ChungTuResponse>>> GetChungTuByTaiSanIdAsync(int taiSanId)
        {
            try
            {
                // Lọc các chứng từ có chứa TaiSanId trong các dòng chi tiết hạch toán
                var result = await _dbContext.chungTus
                    .Include(x => x.ChiTietChungTus)
                    .Where(x => x.ChiTietChungTus!.Any(ct => ct.TaiSanId == taiSanId))
                    .Select(x => new ChungTuResponse
                    {
                        id = x.Id,
                        maChungTu = x.MaChungTu,
                        ngayLap = x.NgayLap,
                        loaiChungTu = x.LoaiChungTu,
                        moTa = x.MoTa,
                        tongTien = x.TongTien,
                        trangThai = x.TrangThai,
                        nguoiLapId = x.NguoiLapId,
                        ngayTao = x.NgayTao,
                        chiTietChungTus = x.ChiTietChungTus!.Select(ct => new ChiTietChungTuResponse
                        {
                            id = ct.Id,
                            chungTuId = ct.ChungTuId,
                            taiKhoanNo = ct.TaiKhoanNo,
                            taiKhoanCo = ct.TaiKhoanCo,
                            soTien = ct.SoTien,
                            moTa = ct.MoTa,
                            taiSanId = ct.TaiSanId
                        }).ToList()
                    })
                    .OrderByDescending(x => x.ngayLap)
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách chứng từ theo tài sản thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách chứng từ theo tài sản.");
                return ResponseConst.Error<List<ChungTuResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

    }
}