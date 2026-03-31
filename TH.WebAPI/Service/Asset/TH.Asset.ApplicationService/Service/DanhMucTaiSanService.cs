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
    public interface IDanhMucTaiSanService
    {
        Task<ResponseDto<bool>> CreateDanhMucTaiSanAsync(CreateDanhMucTaiSanRequestDto request);
        Task<ResponseDto<bool>> UpdateDanhMucTaiSanAsync(UpdateDanhMucTaiSanRequestDto request);
        Task<ResponseDto<bool>> DeleteDanhMucTaiSanAsync(int id);
        Task<ResponseDto<List<DanhMucTaiSanResponse>>> GetAllDanhMucTaiSanAsync();
        Task<ResponseDto<DanhMucTaiSanResponse>> GetDanhMucTaiSanByIdAsync(int id);
    }

    public class DanhMucTaiSanService : AssetServiceBase, IDanhMucTaiSanService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<DanhMucTaiSanService> _logger;

        public DanhMucTaiSanService(ILogger<DanhMucTaiSanService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<ResponseDto<bool>> CreateDanhMucTaiSanAsync(CreateDanhMucTaiSanRequestDto request)
        {
            try
            {
                // 1. Kiểm tra mã danh mục đã tồn tại chưa
                var isExist = await _dbContext.danhMucTaiSans.AnyAsync(x => x.MaDanhMuc == request.maDanhMuc);
                if (isExist)
                {
                    return ResponseConst.Error<bool>(400, "Mã danh mục đã tồn tại trong hệ thống.");
                }

                // 2. Kiểm tra mã tài khoản kế toán nếu có truyền vào
                if (!string.IsNullOrEmpty(request.maTaiKhoan))
                {
                    var tkExist = await _dbContext.taiKhoanKeToans.AnyAsync(x => x.MaTaiKhoan == request.maTaiKhoan);
                    if (!tkExist)
                    {
                        return ResponseConst.Error<bool>(400, "Mã tài khoản kế toán không tồn tại.");
                    }
                }

                var danhMuc = new DanhMucTaiSan
                {
                    MaDanhMuc = request.maDanhMuc,
                    TenDanhMuc = request.tenDanhMuc,
                    TienTo = request.tienTo,
                    ThoiGianKhauHao = request.thoiGianKhauHao,
                    MaTaiKhoan = string.IsNullOrEmpty(request.maTaiKhoan) ? null : request.maTaiKhoan
                };

                _dbContext.danhMucTaiSans.Add(danhMuc);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Thêm danh mục tài sản thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo danh mục tài sản.");
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> UpdateDanhMucTaiSanAsync(UpdateDanhMucTaiSanRequestDto request)
        {
            try
            {
                var danhMuc = await _dbContext.danhMucTaiSans.FirstOrDefaultAsync(x => x.Id == request.id);
                if (danhMuc == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy danh mục tài sản.");
                }

                // 1. Kiểm tra mã danh mục mới có bị trùng không
                if (danhMuc.MaDanhMuc != request.maDanhMuc)
                {
                    var isExist = await _dbContext.danhMucTaiSans.AnyAsync(x => x.MaDanhMuc == request.maDanhMuc);
                    if (isExist)
                    {
                        return ResponseConst.Error<bool>(400, "Mã danh mục mới đã tồn tại ở bản ghi khác.");
                    }
                }

                // 2. Kiểm tra mã tài khoản kế toán nếu có thay đổi
                if (!string.IsNullOrEmpty(request.maTaiKhoan) && danhMuc.MaTaiKhoan != request.maTaiKhoan)
                {
                    var tkExist = await _dbContext.taiKhoanKeToans.AnyAsync(x => x.MaTaiKhoan == request.maTaiKhoan);
                    if (!tkExist)
                    {
                        return ResponseConst.Error<bool>(400, "Mã tài khoản kế toán mới không tồn tại.");
                    }
                }

                // Cập nhật dữ liệu
                danhMuc.MaDanhMuc = request.maDanhMuc;
                danhMuc.TenDanhMuc = request.tenDanhMuc;
                danhMuc.TienTo = request.tienTo;
                danhMuc.ThoiGianKhauHao = request.thoiGianKhauHao;
                danhMuc.MaTaiKhoan = string.IsNullOrEmpty(request.maTaiKhoan) ? null : request.maTaiKhoan;

                _dbContext.danhMucTaiSans.Update(danhMuc);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Cập nhật danh mục tài sản thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật danh mục tài sản. ID: {Id}", request.id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> DeleteDanhMucTaiSanAsync(int id)
        {
            try
            {
                var danhMuc = await _dbContext.danhMucTaiSans.FirstOrDefaultAsync(x => x.Id == id);
                if (danhMuc == null)
                {
                    return ResponseConst.Error<bool>(404, "Không tìm thấy danh mục tài sản.");
                }

                // Kiểm tra xem danh mục này có đang được sử dụng bởi Tài sản nào không
                var isUsed = await _dbContext.taiSans.AnyAsync(x => x.DanhMucId == id);
                if (isUsed)
                {
                    return ResponseConst.Error<bool>(400, "Không thể xóa danh mục đang có tài sản gắn kèm.");
                }

                _dbContext.danhMucTaiSans.Remove(danhMuc);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Xóa danh mục tài sản thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa danh mục tài sản. ID: {Id}", id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<List<DanhMucTaiSanResponse>>> GetAllDanhMucTaiSanAsync()
        {
            try
            {
                var result = await _dbContext.danhMucTaiSans
                    .Select(x => new DanhMucTaiSanResponse
                    {
                        id = x.Id,
                        maDanhMuc = x.MaDanhMuc,
                        tenDanhMuc = x.TenDanhMuc,
                        tienTo = x.TienTo,
                        thoiGianKhauHao = x.ThoiGianKhauHao,
                        maTaiKhoan = x.MaTaiKhoan
                    })
                    .OrderBy(x => x.maDanhMuc)
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách danh mục tài sản thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách danh mục tài sản.");
                return ResponseConst.Error<List<DanhMucTaiSanResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<DanhMucTaiSanResponse>> GetDanhMucTaiSanByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.danhMucTaiSans
                    .Where(x => x.Id == id)
                    .Select(x => new DanhMucTaiSanResponse
                    {
                        id = x.Id,
                        maDanhMuc = x.MaDanhMuc,
                        tenDanhMuc = x.TenDanhMuc,
                        tienTo = x.TienTo,
                        thoiGianKhauHao = x.ThoiGianKhauHao,
                        maTaiKhoan = x.MaTaiKhoan
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return ResponseConst.Error<DanhMucTaiSanResponse>(404, "Không tìm thấy danh mục tài sản.");
                }

                return ResponseConst.Success("Lấy chi tiết danh mục tài sản thành công.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết danh mục tài sản. ID: {Id}", id);
                return ResponseConst.Error<DanhMucTaiSanResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}