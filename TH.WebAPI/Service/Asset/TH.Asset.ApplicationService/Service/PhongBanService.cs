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
    public interface IPhongBanService
    {
        Task<ResponseDto<bool>> CreatePhongBanAsync(CreatePhongBanRequestDto request);
        Task<ResponseDto<bool>> UpdatePhongBanAsync(UpdatePhongBanRequestDto request);

        // Đã đổi string -> int cho khớp với thiết kế CSDL mới
        Task<ResponseDto<bool>> DeletePhongBanAsync(int id);
        Task<ResponseDto<List<PhongBanResponse>>> GetAllPhongBanAsync();
        Task<ResponseDto<PhongBanResponse>> GetPhongBanByIdAsync(int id);
    }

    public class PhongBanService : AssetServiceBase, IPhongBanService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<PhongBanService> _logger;

        public PhongBanService(ILogger<PhongBanService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<ResponseDto<bool>> CreatePhongBanAsync(CreatePhongBanRequestDto request)
        {
            try
            {
                // Kiểm tra xem mã phòng ban đã tồn tại chưa
                var isExist = await _dbContext.phongBans.AnyAsync(x => x.MaPhongBan == request.maPhongBan);
                if (isExist)
                {
                    return ResponseConst.Error<bool>(400, "Mã phòng ban đã tồn tại trong hệ thống.");
                }

                var phongBan = new PhongBan
                {
                    // BỎ GÁN ID vì Id hiện là int auto-increment (tự tăng) trong Database
                    MaPhongBan = request.maPhongBan,
                    TenPhongBan = request.tenPhongBan
                };

                _dbContext.phongBans.Add(phongBan);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Thêm phòng ban thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo phòng ban.");
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> UpdatePhongBanAsync(UpdatePhongBanRequestDto request)
        {
            try
            {
                var phongBan = await _dbContext.phongBans.FirstOrDefaultAsync(x => x.Id == request.id);
                if (phongBan == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy phòng ban.");
                }

                // Nếu người dùng có thay đổi Mã phòng ban, cần check trùng mã mới
                if (phongBan.MaPhongBan != request.maPhongBan)
                {
                    var isExist = await _dbContext.phongBans.AnyAsync(x => x.MaPhongBan == request.maPhongBan);
                    if (isExist)
                    {
                        return ResponseConst.Error<bool>(400, "Mã phòng ban mới đã tồn tại ở bản ghi khác.");
                    }
                }

                // Cập nhật các trường
                phongBan.MaPhongBan = request.maPhongBan;
                phongBan.TenPhongBan = request.tenPhongBan;

                _dbContext.phongBans.Update(phongBan);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Cập nhật phòng ban thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật phòng ban. ID: {Id}", request.id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> DeletePhongBanAsync(int id)
        {
            try
            {
                var phongBan = await _dbContext.phongBans.FirstOrDefaultAsync(x => x.Id == id);
                if (phongBan == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy phòng ban.");
                }

                // Logic an toàn: Kiểm tra phòng ban có đang gắn với tài sản nào không trước khi xóa
                var hasAssets = await _dbContext.taiSans.AnyAsync(x => x.PhongBanId == id);
                if (hasAssets)
                {
                    return ResponseConst.Error<bool>(400, "Không thể xóa phòng ban đang có tài sản gắn kèm.");
                }

                _dbContext.phongBans.Remove(phongBan);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Xóa phòng ban thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa phòng ban. ID: {Id}", id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<List<PhongBanResponse>>> GetAllPhongBanAsync()
        {
            try
            {
                var result = await _dbContext.phongBans
                    .Select(x => new PhongBanResponse
                    {
                        id = x.Id,
                        maPhongBan = x.MaPhongBan,
                        tenPhongBan = x.TenPhongBan
                    })
                    .OrderBy(x => x.maPhongBan)
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách phòng ban thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phòng ban.");
                return ResponseConst.Error<List<PhongBanResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<PhongBanResponse>> GetPhongBanByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.phongBans
                    .Where(x => x.Id == id)
                    .Select(x => new PhongBanResponse
                    {
                        id = x.Id,
                        maPhongBan = x.MaPhongBan,
                        tenPhongBan = x.TenPhongBan
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return ResponseConst.Error<PhongBanResponse>(404, "Không tìm thấy phòng ban.");
                }

                return ResponseConst.Success("Lấy chi tiết phòng ban thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết phòng ban. ID: {Id}", id);
                return ResponseConst.Error<PhongBanResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}