using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;
using TH.Asset.Dtos;

namespace TH.WebAPI.Controllers.Asset
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThanhLyTaiSanController : Controller
    {
        private readonly IThanhLyTaiSanService _thanhLyService;

        public ThanhLyTaiSanController(IThanhLyTaiSanService thanhLyService)
        {
            _thanhLyService = thanhLyService;
        }

        // POST: api/ThanhLyTaiSan/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateThanhLy([FromBody] CreateThanhLyTaiSanRequestDto request)
        {
            var result = await _thanhLyService.CreateThanhLyTaiSanAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/ThanhLyTaiSan/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateThanhLy([FromBody] UpdateThanhLyTaiSanRequestDto request)
        {
            var result = await _thanhLyService.UpdateThanhLyTaiSanAsync(request);
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

        // DELETE: api/ThanhLyTaiSan/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteThanhLy(int id)
        {
            var result = await _thanhLyService.DeleteThanhLyTaiSanAsync(id);
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

        // GET: api/ThanhLyTaiSan/get-all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllThanhLy()
        {
            var result = await _thanhLyService.GetAllThanhLyTaiSanAsync();
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/ThanhLyTaiSan/get/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetThanhLyById(int id)
        {
            var result = await _thanhLyService.GetThanhLyTaiSanByIdAsync(id);
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
    }
}