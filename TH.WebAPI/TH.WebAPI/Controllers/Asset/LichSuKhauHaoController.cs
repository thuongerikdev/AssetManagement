using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;
using TH.Asset.Dtos;

namespace TH.WebAPI.Controllers.Asset
{
    [ApiController]
    [Route("api/[controller]")]
    public class LichSuKhauHaoController : Controller
    {
        private readonly ILichSuKhauHaoService _lichSuKhauHaoService;

        public LichSuKhauHaoController(ILichSuKhauHaoService lichSuKhauHaoService)
        {
            _lichSuKhauHaoService = lichSuKhauHaoService;
        }

        // POST: api/LichSuKhauHao/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateLichSuKhauHao([FromBody] CreateLichSuKhauHaoRequestDto request)
        {
            var result = await _lichSuKhauHaoService.CreateLichSuKhauHaoAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/LichSuKhauHao/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateLichSuKhauHao([FromBody] UpdateLichSuKhauHaoRequestDto request)
        {
            var result = await _lichSuKhauHaoService.UpdateLichSuKhauHaoAsync(request);
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

        // DELETE: api/LichSuKhauHao/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteLichSuKhauHao(int id)
        {
            var result = await _lichSuKhauHaoService.DeleteLichSuKhauHaoAsync(id);
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

        // GET: api/LichSuKhauHao/get-all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllLichSuKhauHao()
        {
            var result = await _lichSuKhauHaoService.GetAllLichSuKhauHaoAsync();
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/LichSuKhauHao/get/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetLichSuKhauHaoById(int id)
        {
            var result = await _lichSuKhauHaoService.GetLichSuKhauHaoByIdAsync(id);
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

        [HttpGet("get-by-asset/{taiSanId}")]
        public async Task<IActionResult> GetLichSuKhauHaoByTaiSanId(int taiSanId)
        {
            var result = await _lichSuKhauHaoService.GetByTaiSanIdAsync(taiSanId);
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