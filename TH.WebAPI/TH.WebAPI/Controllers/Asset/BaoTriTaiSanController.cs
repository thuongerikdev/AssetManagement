using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;
using TH.Asset.Dtos;

namespace TH.WebAPI.Controllers.Asset
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaoTriTaiSanController : Controller
    {
        private readonly IBaoTriTaiSanService _baoTriService;

        public BaoTriTaiSanController(IBaoTriTaiSanService baoTriService)
        {
            _baoTriService = baoTriService;
        }

        // POST: api/BaoTriTaiSan/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateBaoTri([FromBody] CreateBaoTriTaiSanRequestDto request)
        {
            var result = await _baoTriService.CreateBaoTriTaiSanAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/BaoTriTaiSan/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateBaoTri([FromBody] UpdateBaoTriTaiSanRequestDto request)
        {
            var result = await _baoTriService.UpdateBaoTriTaiSanAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            // Trả về NotFound nếu mã lỗi là 404 (Không tìm thấy bản ghi)
            if (result.ErrorCode == 404)
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        // DELETE: api/BaoTriTaiSan/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBaoTri(int id)
        {
            var result = await _baoTriService.DeleteBaoTriTaiSanAsync(id);
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

        // GET: api/BaoTriTaiSan/get-all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBaoTri()
        {
            var result = await _baoTriService.GetAllBaoTriTaiSanAsync();
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/BaoTriTaiSan/get/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetBaoTriById(int id)
        {
            var result = await _baoTriService.GetBaoTriTaiSanByIdAsync(id);
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
    }
}