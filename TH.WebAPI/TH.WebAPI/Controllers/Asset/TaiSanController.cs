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
        public async Task<IActionResult> UpdateTaiSan([FromBody] UpdateTaiSanRequestDto request)
        {
            var result = await _taiSanService.UpdateTaiSanAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            // Trả về NotFound nếu mã lỗi là 404
            if (result.ErrorCode == 404)
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        // DELETE: api/TaiSan/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTaiSan(int id)
        {
            var result = await _taiSanService.DeleteTaiSanAsync(id);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            // Trả về NotFound nếu mã lỗi là 404
            if (result.ErrorCode == 404)
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        // GET: api/TaiSan/get-all
        [HttpGet("get-all")]
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
        public async Task<IActionResult> GetTaiSanById(int id)
        {
            var result = await _taiSanService.GetTaiSanByIdAsync(id);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            // Trả về NotFound nếu mã lỗi là 404
            if (result.ErrorCode == 404)
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        [HttpPost("confirm/{id}")]
        public async Task<IActionResult> ConfirmAsset(int id)
        {
            var result = await _taiSanService.ConfirmAssetAsync(id);
            return Ok(result);
        }

        [HttpGet("my-assets/{userId}")]
        public async Task<IActionResult> GetMyAssets(int userId)
        {
            // Thực tế sau này userId sẽ lấy từ Token (User.Claims), 
            // tạm thời ta truyền qua URL để test.
            var result = await _taiSanService.GetTaiSanByUserIdAsync(userId);
            return Ok(result);
        }
    }
}