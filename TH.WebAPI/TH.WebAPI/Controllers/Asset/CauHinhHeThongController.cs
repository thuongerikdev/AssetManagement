using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;
using TH.Asset.Dtos;

namespace TH.WebAPI.Controllers.Asset
{
    [ApiController]
    [Route("api/[controller]")]
    public class CauHinhHeThongController : Controller
    {
        private readonly ICauHinhHeThongService _cauHinhService;

        public CauHinhHeThongController(ICauHinhHeThongService cauHinhService)
        {
            _cauHinhService = cauHinhService;
        }

        // GET: api/CauHinhHeThong/get
        // Lấy cấu hình hệ thống hiện tại. Nếu chưa có, Service sẽ tự tạo 1 bản ghi mặc định.
        [HttpGet("get")]
        public async Task<IActionResult> GetCauHinh()
        {
            var result = await _cauHinhService.GetCauHinhAsync();
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/CauHinhHeThong/update
        // Cập nhật đè lên bản ghi cấu hình hiện tại (không cần truyền ID)
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCauHinh([FromBody] CauHinhHeThongRequestDto request)
        {
            var result = await _cauHinhService.UpdateCauHinhAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}