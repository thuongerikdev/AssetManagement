using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Common;
using TH.Asset.Domain.Entities;
using TH.Asset.Dtos;
using TH.Asset.Infrastructure.Database;
using TH.Constant;

namespace TH.Asset.ApplicationService.Service
{
    public interface ITaiSanDinhKemService
    {
        Task<ResponseDto<List<TaiSanDinhKemResponse>>> GetByAssetIdAsync(int taiSanId);
        Task<ResponseDto<TaiSanDinhKemResponse>> UploadAsync(int taiSanId, IFormFile file, string? moTa);
        Task<ResponseDto<bool>> DeleteAsync(int id);
    }

    public class TaiSanDinhKemService : AssetServiceBase, ITaiSanDinhKemService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<TaiSanDinhKemService> _logger;
        private readonly string _uploadRoot;

        public TaiSanDinhKemService(ILogger<TaiSanDinhKemService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _uploadRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "attachments");
            Directory.CreateDirectory(_uploadRoot);
        }

        public async Task<ResponseDto<List<TaiSanDinhKemResponse>>> GetByAssetIdAsync(int taiSanId)
        {
            try
            {
                var list = await _dbContext.taiSanDinhKems
                    .Where(x => x.TaiSanId == taiSanId)
                    .OrderByDescending(x => x.NgayTai)
                    .Select(x => new TaiSanDinhKemResponse
                    {
                        id = x.Id,
                        taiSanId = x.TaiSanId,
                        tenFile = x.TenFile,
                        loaiFile = x.LoaiFile,
                        duongDan = x.DuongDan,
                        kichThuoc = x.KichThuoc,
                        ngayTai = x.NgayTai,
                        moTa = x.MoTa
                    })
                    .ToListAsync();

                return ResponseConst.Success("Lấy danh sách đính kèm thành công.", list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy file đính kèm. TaiSanId: {Id}", taiSanId);
                return ResponseConst.Error<List<TaiSanDinhKemResponse>>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<TaiSanDinhKemResponse>> UploadAsync(int taiSanId, IFormFile file, string? moTa)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return ResponseConst.Error<TaiSanDinhKemResponse>(400, "File không hợp lệ.");

                if (file.Length > 20 * 1024 * 1024)
                    return ResponseConst.Error<TaiSanDinhKemResponse>(400, "File không được vượt quá 20MB.");

                var assetExists = await _dbContext.taiSans.AnyAsync(x => x.Id == taiSanId);
                if (!assetExists)
                    return ResponseConst.Error<TaiSanDinhKemResponse>(404, "Tài sản không tồn tại.");

                var ext = Path.GetExtension(file.FileName);
                var safeFileName = $"{taiSanId}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{ext}";
                var assetFolder = Path.Combine(_uploadRoot, taiSanId.ToString());
                Directory.CreateDirectory(assetFolder);
                var fullPath = Path.Combine(assetFolder, safeFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                    await file.CopyToAsync(stream);

                var relativePath = $"/attachments/{taiSanId}/{safeFileName}";

                var entity = new TaiSanDinhKem
                {
                    TaiSanId = taiSanId,
                    TenFile = file.FileName,
                    LoaiFile = file.ContentType,
                    DuongDan = relativePath,
                    KichThuoc = file.Length,
                    NgayTai = DateTime.UtcNow,
                    MoTa = moTa
                };

                _dbContext.taiSanDinhKems.Add(entity);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Upload file thành công.", new TaiSanDinhKemResponse
                {
                    id = entity.Id,
                    taiSanId = entity.TaiSanId,
                    tenFile = entity.TenFile,
                    loaiFile = entity.LoaiFile,
                    duongDan = entity.DuongDan,
                    kichThuoc = entity.KichThuoc,
                    ngayTai = entity.NgayTai,
                    moTa = entity.MoTa
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi upload file đính kèm. TaiSanId: {Id}", taiSanId);
                return ResponseConst.Error<TaiSanDinhKemResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbContext.taiSanDinhKems.FindAsync(id);
                if (entity == null)
                    return ResponseConst.Error<bool>(404, "Không tìm thấy file đính kèm.");

                if (!string.IsNullOrEmpty(entity.DuongDan))
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", entity.DuongDan.TrimStart('/'));
                    if (File.Exists(fullPath)) File.Delete(fullPath);
                }

                _dbContext.taiSanDinhKems.Remove(entity);
                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Xóa file đính kèm thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa file đính kèm. Id: {Id}", id);
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}
