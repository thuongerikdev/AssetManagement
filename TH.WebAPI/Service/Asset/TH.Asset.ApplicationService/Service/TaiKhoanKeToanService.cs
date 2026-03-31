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
    public interface ITaiKhoanKeToanService
    {
        Task<ResponseDto<bool>> CreateTaiKhoanKeToanAsync(CreateTaiKhoanKeToanRequestDto request);
        Task<ResponseDto<bool>> UpdateTaiKhoanKeToanAsync(UpdateTaiKhoanKeToanRequestDto request);
        Task<ResponseDto<bool>> DeleteTaiKhoanKeToanAsync(int id);
        Task<ResponseDto<List<TaiKhoanKeToanResponse>>> GetAllTaiKhoanKeToanAsync();
        Task<ResponseDto<TaiKhoanKeToanResponse>> GetTaiKhoanKeToanByIdAsync(int id);
    }

    public class TaiKhoanKeToanService : AssetServiceBase, ITaiKhoanKeToanService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<TaiKhoanKeToanService> _logger;

        public TaiKhoanKeToanService(ILogger<TaiKhoanKeToanService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<ResponseDto<bool>> CreateTaiKhoanKeToanAsync(CreateTaiKhoanKeToanRequestDto request)
        {
            try
            {
                // 1. Kiểm tra mã tài khoản đã tồn tại chưa
                var isExist = await _dbContext.taiKhoanKeToans.AnyAsync(x => x.MaTaiKhoan == request.maTaiKhoan);
                if (isExist)
                {
                    return ResponseConst.Error<bool>(400, "Mã tài khoản đã tồn tại trong hệ thống.");
                }

                // 2. Nếu có truyền mã tài khoản cha, kiểm tra xem cha có tồn tại không
                if (!string.IsNullOrEmpty(request.maTaiKhoanCha))
                {
                    var parentExist = await _dbContext.taiKhoanKeToans.AnyAsync(x => x.MaTaiKhoan == request.maTaiKhoanCha);
                    if (!parentExist)
                    {
                        return ResponseConst.Error<bool>(400, "Mã tài khoản cha không tồn tại.");
                    }
                }

                var taiKhoan = new TaiKhoanKeToan
                {
                    MaTaiKhoan = request.maTaiKhoan,
                    TenTaiKhoan = request.tenTaiKhoan,
                    LoaiTaiKhoan = request.loaiTaiKhoan,
                    MaTaiKhoanCha = request.maTaiKhoanCha
                };

                _dbContext.taiKhoanKeToans.Add(taiKhoan);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Thêm tài khoản kế toán thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo tài khoản kế toán.");
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> UpdateTaiKhoanKeToanAsync(UpdateTaiKhoanKeToanRequestDto request)
        {
            try
            {
                var taiKhoan = await _dbContext.taiKhoanKeToans.FirstOrDefaultAsync(x => x.Id == request.id);
                if (taiKhoan == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy tài khoản kế toán.");
                }

                // 1. Nếu đổi Mã tài khoản, kiểm tra xem mã mới đã bị trùng chưa
                if (taiKhoan.MaTaiKhoan != request.maTaiKhoan)
                {
                    var isExist = await _dbContext.taiKhoanKeToans.AnyAsync(x => x.MaTaiKhoan == request.maTaiKhoan);
                    if (isExist)
                    {
                        return ResponseConst.Error<bool>(400, "Mã tài khoản mới đã tồn tại ở bản ghi khác.");
                    }

                    // Cảnh báo: Việc đổi Mã tài khoản có thể ảnh hưởng đến các bảng khác đang tham chiếu bằng string
                    // Tùy nghiệp vụ bạn có thể cấm đổi MaTaiKhoan sau khi tạo, nếu cấm thì bỏ đoạn if này và không gán lại MaTaiKhoan.
                }

                // 2. Kiểm tra tài khoản cha
                if (!string.IsNullOrEmpty(request.maTaiKhoanCha))
                {
                    if (request.maTaiKhoanCha == request.maTaiKhoan)
                    {
                        return ResponseConst.Error<bool>(400, "Tài khoản cha không thể là chính nó.");
                    }

                    var parentExist = await _dbContext.taiKhoanKeToans.AnyAsync(x => x.MaTaiKhoan == request.maTaiKhoanCha);
                    if (!parentExist)
                    {
                        return ResponseConst.Error<bool>(400, "Mã tài khoản cha không tồn tại.");
                    }
                }

                taiKhoan.MaTaiKhoan = request.maTaiKhoan;
                taiKhoan.TenTaiKhoan = request.tenTaiKhoan;
                taiKhoan.LoaiTaiKhoan = request.loaiTaiKhoan;
                taiKhoan.MaTaiKhoanCha = string.IsNullOrEmpty(request.maTaiKhoanCha) ? null : request.maTaiKhoanCha;

                _dbContext.taiKhoanKeToans.Update(taiKhoan);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Cập nhật tài khoản kế toán thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật tài khoản kế toán. ID: {Id}", request.id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> DeleteTaiKhoanKeToanAsync(int id)
        {
            try
            {
                var taiKhoan = await _dbContext.taiKhoanKeToans.FirstOrDefaultAsync(x => x.Id == id);
                if (taiKhoan == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy tài khoản kế toán.");
                }

                // 1. Kiểm tra xem có tài khoản con nào đang phụ thuộc vào mã này không
                var hasChildren = await _dbContext.taiKhoanKeToans.AnyAsync(x => x.MaTaiKhoanCha == taiKhoan.MaTaiKhoan);
                if (hasChildren)
                {
                    return ResponseConst.Error<bool>(400, "Không thể xóa. Tài khoản này đang có tài khoản cấp dưới.");
                }

                // 2. Kiểm tra xem có danh mục hoặc tài sản nào đang dùng mã tài khoản này không
                var isUsedInDanhMuc = await _dbContext.danhMucTaiSans.AnyAsync(x => x.MaTaiKhoan == taiKhoan.MaTaiKhoan);
                var isUsedInTaiSan = await _dbContext.taiSans.AnyAsync(x => x.MaTaiKhoan == taiKhoan.MaTaiKhoan);
                if (isUsedInDanhMuc || isUsedInTaiSan)
                {
                    return ResponseConst.Error<bool>(400, "Không thể xóa. Tài khoản này đang được thiết lập cho Danh mục hoặc Tài sản.");
                }

                // 3. Kiểm tra xem có phát sinh chứng từ (Chi tiết chứng từ) nào dùng mã này chưa
                var isUsedInChungTu = await _dbContext.chiTietChungTus.AnyAsync(x => x.TaiKhoanNo == taiKhoan.MaTaiKhoan || x.TaiKhoanCo == taiKhoan.MaTaiKhoan);
                if (isUsedInChungTu)
                {
                    return ResponseConst.Error<bool>(400, "Không thể xóa. Tài khoản này đã phát sinh giao dịch trong hệ thống chứng từ.");
                }

                _dbContext.taiKhoanKeToans.Remove(taiKhoan);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Xóa tài khoản kế toán thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa tài khoản kế toán. ID: {Id}", id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<List<TaiKhoanKeToanResponse>>> GetAllTaiKhoanKeToanAsync()
        {
            try
            {
                var result = await _dbContext.taiKhoanKeToans
                    .Select(x => new TaiKhoanKeToanResponse
                    {
                        id = x.Id,
                        maTaiKhoan = x.MaTaiKhoan,
                        tenTaiKhoan = x.TenTaiKhoan,
                        loaiTaiKhoan = x.LoaiTaiKhoan,
                        maTaiKhoanCha = x.MaTaiKhoanCha
                    })
                    .OrderBy(x => x.maTaiKhoan)
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách tài khoản kế toán thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách tài khoản kế toán.");
                return ResponseConst.Error<List<TaiKhoanKeToanResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<TaiKhoanKeToanResponse>> GetTaiKhoanKeToanByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.taiKhoanKeToans
                    .Where(x => x.Id == id)
                    .Select(x => new TaiKhoanKeToanResponse
                    {
                        id = x.Id,
                        maTaiKhoan = x.MaTaiKhoan,
                        tenTaiKhoan = x.TenTaiKhoan,
                        loaiTaiKhoan = x.LoaiTaiKhoan,
                        maTaiKhoanCha = x.MaTaiKhoanCha
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return ResponseConst.Error<TaiKhoanKeToanResponse>(404, "Không tìm thấy tài khoản kế toán.");
                }

                return ResponseConst.Success("Lấy chi tiết tài khoản kế toán thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết tài khoản kế toán. ID: {Id}", id);
                return ResponseConst.Error<TaiKhoanKeToanResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}