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
    public interface ILoTaiSanService
    {
        Task<ResponseDto<bool>> CreateLoTaiSanAsync(CreateLoTaiSanRequestDto request);
        Task<ResponseDto<bool>> UpdateLoTaiSanAsync(UpdateLoTaiSanRequestDto request);
        Task<ResponseDto<bool>> DeleteLoTaiSanAsync(int id);
        Task<ResponseDto<List<LoTaiSanResponse>>> GetAllLoTaiSanAsync();
        Task<ResponseDto<LoTaiSanResponse>> GetLoTaiSanByIdAsync(int id);
    }

    public class LoTaiSanService : AssetServiceBase, ILoTaiSanService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<LoTaiSanService> _logger;

        public LoTaiSanService(ILogger<LoTaiSanService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<ResponseDto<bool>> CreateLoTaiSanAsync(CreateLoTaiSanRequestDto request)
        {
            try
            {
                // Kiểm tra xem mã lô đã tồn tại chưa
                var isExist = await _dbContext.loTaiSans.AnyAsync(x => x.MaLo == request.maLo);
                if (isExist)
                {
                    return ResponseConst.Error<bool>(400, "Mã lô tài sản đã tồn tại trong hệ thống.");
                }

                var loTaiSan = new LoTaiSan
                {
                    MaLo = request.maLo,
                    SoLuong = request.soLuong,
                    TongGiaTri = request.tongGiaTri,
                    NgayTao = DateTime.UtcNow // Tự động gán ngày tạo là thời gian hiện tại (UTC)
                };

                _dbContext.loTaiSans.Add(loTaiSan);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Thêm lô tài sản thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo lô tài sản.");
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> UpdateLoTaiSanAsync(UpdateLoTaiSanRequestDto request)
        {
            try
            {
                var loTaiSan = await _dbContext.loTaiSans.FirstOrDefaultAsync(x => x.Id == request.id);
                if (loTaiSan == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy lô tài sản.");
                }

                // Nếu đổi mã lô, kiểm tra xem mã mới có trùng với lô khác không
                if (loTaiSan.MaLo != request.maLo)
                {
                    var isExist = await _dbContext.loTaiSans.AnyAsync(x => x.MaLo == request.maLo);
                    if (isExist)
                    {
                        return ResponseConst.Error<bool>(400, "Mã lô mới đã tồn tại ở bản ghi khác.");
                    }
                }

                // Cập nhật các trường
                loTaiSan.MaLo = request.maLo;
                loTaiSan.SoLuong = request.soLuong;
                loTaiSan.TongGiaTri = request.tongGiaTri;
                // Không cập nhật NgayTao

                _dbContext.loTaiSans.Update(loTaiSan);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Cập nhật lô tài sản thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật lô tài sản. ID: {Id}", request.id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> DeleteLoTaiSanAsync(int id)
        {
            try
            {
                var loTaiSan = await _dbContext.loTaiSans.FirstOrDefaultAsync(x => x.Id == id);
                if (loTaiSan == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy lô tài sản.");
                }

                // Ràng buộc an toàn: Không xóa nếu có tài sản chi tiết đang gắn với lô này
                var isUsed = await _dbContext.taiSans.AnyAsync(x => x.LoId == id);
                if (isUsed)
                {
                    return ResponseConst.Error<bool>(400, "Không thể xóa lô đang có tài sản gắn kèm.");
                }

                _dbContext.loTaiSans.Remove(loTaiSan);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Xóa lô tài sản thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa lô tài sản. ID: {Id}", id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<List<LoTaiSanResponse>>> GetAllLoTaiSanAsync()
        {
            try
            {
                var result = await _dbContext.loTaiSans
                    .Select(x => new LoTaiSanResponse
                    {
                        id = x.Id,
                        maLo = x.MaLo,
                        soLuong = x.SoLuong,
                        tongGiaTri = x.TongGiaTri,
                        ngayTao = x.NgayTao
                    })
                    .OrderByDescending(x => x.ngayTao) // Sắp xếp giảm dần theo ngày tạo (mới nhất lên đầu)
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách lô tài sản thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách lô tài sản.");
                return ResponseConst.Error<List<LoTaiSanResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<LoTaiSanResponse>> GetLoTaiSanByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.loTaiSans
                    .Where(x => x.Id == id)
                    .Select(x => new LoTaiSanResponse
                    {
                        id = x.Id,
                        maLo = x.MaLo,
                        soLuong = x.SoLuong,
                        tongGiaTri = x.TongGiaTri,
                        ngayTao = x.NgayTao
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return ResponseConst.Error<LoTaiSanResponse>(404, "Không tìm thấy lô tài sản.");
                }

                return ResponseConst.Success("Lấy chi tiết lô tài sản thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết lô tài sản. ID: {Id}", id);
                return ResponseConst.Error<LoTaiSanResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}