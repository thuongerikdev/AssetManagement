using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;
using TH.Asset.Dtos;

namespace TH.WebAPI.Controllers.Asset
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaiSanController : Controller
    {
        private readonly ITaiSanService _taiSanService;

        public TaiSanController(ITaiSanService taiSanService)
        {
            _taiSanService = taiSanService;
        }

        // POST: api/TaiSan/create
        [HttpPost("create")]
        [Authorize(Policy = "TaiSanCreate")]
        public async Task<IActionResult> CreateTaiSan([FromBody] CreateTaiSanRequestDto request)
        {
            var result = await _taiSanService.CreateTaiSanAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/TaiSan/update
        [HttpPut("update")]
        [Authorize(Policy = "TaiSanUpdate")]
        public async Task<IActionResult> UpdateTaiSan([FromBody] UpdateTaiSanRequestDto request)
        {
            var result = await _taiSanService.UpdateTaiSanAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            if (result.ErrorCode == 404)
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        // DELETE: api/TaiSan/delete/5
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "TaiSanDelete")]
        public async Task<IActionResult> DeleteTaiSan(int id)
        {
            var result = await _taiSanService.DeleteTaiSanAsync(id);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            if (result.ErrorCode == 404)
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        // GET: api/TaiSan/get-all
        [HttpGet("get-all")]
        [Authorize(Policy = "TaiSanGetAll")]
        public async Task<IActionResult> GetAllTaiSan()
        {
            var result = await _taiSanService.GetAllTaiSanAsync();
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/TaiSan/get/5
        [HttpGet("get/{id}")]
        [Authorize(Policy = "TaiSanGetById")]
        public async Task<IActionResult> GetTaiSanById(int id)
        {
            var result = await _taiSanService.GetTaiSanByIdAsync(id);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            if (result.ErrorCode == 404)
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        // POST: api/TaiSan/confirm/{id}
        [HttpPost("confirm/{id}")]
        [Authorize(Policy = "TaiSanConfirm")]
        public async Task<IActionResult> ConfirmAsset(int id)
        {
            var result = await _taiSanService.ConfirmAssetAsync(id);
            return Ok(result);
        }

        // GET: api/TaiSan/my-assets/{userId}
        [HttpGet("my-assets/{userId}")]
        [Authorize(Policy = "TaiSanGetMine")]
        public async Task<IActionResult> GetMyAssets(int userId)
        {
            var result = await _taiSanService.GetTaiSanByUserIdAsync(userId);
            return Ok(result);
        }

        // GET: api/TaiSan/generate-code?danhMucId=1
        [HttpGet("generate-code")]
        [Authorize(Policy = "TaiSanGenerateCode")]
        public async Task<IActionResult> GenerateMaTaiSan([FromQuery] int danhMucId)
        {
            var result = await _taiSanService.GenerateMaTaiSanAsync(danhMucId);
            if (result.ErrorCode == 404) return NotFound(result);
            return result.ErrorCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpGet("department/{phongBanId}")]
        // [Authorize] (Tuỳ bạn cấu hình policy)
        public async Task<IActionResult> GetByDepartmentId(int phongBanId)
        {
            var result = await _taiSanService.GetTaiSanByPhongBanIdAsync(phongBanId);
            if (result.ErrorCode == 200) return Ok(result);
            return BadRequest(result);
        }
    }
}
