using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
    public interface ICauHinhHeThongService
    {
        Task<ResponseDto<CauHinhHeThongResponse>> GetCauHinhAsync();
        Task<ResponseDto<bool>> UpdateCauHinhAsync(CauHinhHeThongRequestDto request);
    }

    public class CauHinhHeThongService : AssetServiceBase, ICauHinhHeThongService
    {
        private readonly AssetDbContext _dbContext;
        private readonly ILogger<CauHinhHeThongService> _logger;

        public CauHinhHeThongService(ILogger<CauHinhHeThongService> logger, AssetDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<ResponseDto<CauHinhHeThongResponse>> GetCauHinhAsync()
        {
            try
            {
                // Luôn lấy bản ghi đầu tiên trong hệ thống
                var cauHinh = await _dbContext.cauHinhHeThongs.FirstOrDefaultAsync();

                // Nếu database hoàn toàn trống (chưa chạy seed data), tự động tạo 1 bản ghi mặc định
                if (cauHinh == null)
                {
                    cauHinh = new CauHinhHeThong
                    {
                        TenCongTy = "Công ty Mặc Định",
                        TienToChungTu = "CT",
                        SoBatDauChungTu = 1,
                        TuDongKhauHao = true,
                        DinhDangMaTaiSan = "{DANH_MUC}-{SO_THU_TU}",
                        DoDaiMaTaiSan = 4,
                        PhuongPhapKhauHaoMacDinh = PhuongPhapKhauHao.DuongThang
                    };

                    _dbContext.cauHinhHeThongs.Add(cauHinh);
                    await _dbContext.SaveChangesAsync();
                }

                var response = new CauHinhHeThongResponse
                {
                    id = cauHinh.Id,
                    tenCongTy = cauHinh.TenCongTy,
                    maSoThue = cauHinh.MaSoThue,
                    diaChi = cauHinh.DiaChi,
                    tienToChungTu = cauHinh.TienToChungTu,
                    soBatDauChungTu = cauHinh.SoBatDauChungTu,
                    phuongPhapKhauHaoMacDinh = cauHinh.PhuongPhapKhauHaoMacDinh,
                    tuDongKhauHao = cauHinh.TuDongKhauHao,
                    dinhDangMaTaiSan = cauHinh.DinhDangMaTaiSan,
                    doDaiMaTaiSan = cauHinh.DoDaiMaTaiSan
                };

                return ResponseConst.Success("Lấy cấu hình hệ thống thành công.", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy cấu hình hệ thống.");
                return ResponseConst.Error<CauHinhHeThongResponse>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> UpdateCauHinhAsync(CauHinhHeThongRequestDto request)
        {
            try
            {
                // Lấy bản ghi cấu hình đầu tiên hiện có
                var cauHinh = await _dbContext.cauHinhHeThongs.FirstOrDefaultAsync();

                if (cauHinh == null)
                {
                    // Đề phòng trường hợp chưa có, khởi tạo mới
                    cauHinh = new CauHinhHeThong();
                    _dbContext.cauHinhHeThongs.Add(cauHinh);
                }
                else
                {
                    _dbContext.cauHinhHeThongs.Update(cauHinh);
                }

                // Cập nhật các trường
                cauHinh.TenCongTy = request.tenCongTy;
                cauHinh.MaSoThue = request.maSoThue;
                cauHinh.DiaChi = request.diaChi;
                cauHinh.TienToChungTu = request.tienToChungTu;
                cauHinh.SoBatDauChungTu = request.soBatDauChungTu;
                cauHinh.PhuongPhapKhauHaoMacDinh = request.phuongPhapKhauHaoMacDinh;
                cauHinh.TuDongKhauHao = request.tuDongKhauHao;
                cauHinh.DinhDangMaTaiSan = request.dinhDangMaTaiSan;
                cauHinh.DoDaiMaTaiSan = request.doDaiMaTaiSan;

                await _dbContext.SaveChangesAsync();

                return ResponseConst.Success("Cập nhật cấu hình hệ thống thành công.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật cấu hình hệ thống.");
                return ResponseConst.Error<bool>(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}