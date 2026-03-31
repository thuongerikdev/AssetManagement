using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;
using TH.Asset.Dtos;

namespace TH.WebAPI.Controllers.Asset
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaiKhoanKeToanController : Controller
    {
        private readonly ITaiKhoanKeToanService _taiKhoanKeToanService;

        public TaiKhoanKeToanController(ITaiKhoanKeToanService taiKhoanKeToanService)
        {
            _taiKhoanKeToanService = taiKhoanKeToanService;
        }

        // POST: api/TaiKhoanKeToan/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateTaiKhoanKeToan([FromBody] CreateTaiKhoanKeToanRequestDto request)
        {
            var result = await _taiKhoanKeToanService.CreateTaiKhoanKeToanAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/TaiKhoanKeToan/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTaiKhoanKeToan([FromBody] UpdateTaiKhoanKeToanRequestDto request)
        {
            var result = await _taiKhoanKeToanService.UpdateTaiKhoanKeToanAsync(request);
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

        // DELETE: api/TaiKhoanKeToan/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTaiKhoanKeToan(int id)
        {
            var result = await _taiKhoanKeToanService.DeleteTaiKhoanKeToanAsync(id);
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

        // GET: api/TaiKhoanKeToan/get-all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllTaiKhoanKeToan()
        {
            var result = await _taiKhoanKeToanService.GetAllTaiKhoanKeToanAsync();
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/TaiKhoanKeToan/get/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetTaiKhoanKeToanById(int id)
        {
            var result = await _taiKhoanKeToanService.GetTaiKhoanKeToanByIdAsync(id);
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