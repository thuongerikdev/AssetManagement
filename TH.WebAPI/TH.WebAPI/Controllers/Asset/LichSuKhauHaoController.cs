using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "LichSuKhauHaoCreate")]
        public async Task<IActionResult> CreateLichSuKhauHao([FromBody] CreateLichSuKhauHaoRequestDto request)
        {
            var result = await _lichSuKhauHaoService.CreateLichSuKhauHaoAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // POST: api/LichSuKhauHao/create-bulk
        [HttpPost("create-bulk")]
        [Authorize(Policy = "LichSuKhauHaoCreateBulk")]
        public async Task<IActionResult> CreateLichSuKhauHaoBulk([FromBody] KhauHaoHangLoatRequestDto request)
        {
            var result = await _lichSuKhauHaoService.CreateKhauHaoHangLoatAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/LichSuKhauHao/update
        [HttpPut("update")]
        [Authorize(Policy = "LichSuKhauHaoUpdate")]
        public async Task<IActionResult> UpdateLichSuKhauHao([FromBody] UpdateLichSuKhauHaoRequestDto request)
        {
            var result = await _lichSuKhauHaoService.UpdateLichSuKhauHaoAsync(request);
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

        // DELETE: api/LichSuKhauHao/delete/5
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "LichSuKhauHaoDelete")]
        public async Task<IActionResult> DeleteLichSuKhauHao(int id)
        {
            var result = await _lichSuKhauHaoService.DeleteLichSuKhauHaoAsync(id);
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

        // GET: api/LichSuKhauHao/get-all
        [HttpGet("get-all")]
        [Authorize(Policy = "LichSuKhauHaoGetAll")]
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
        [Authorize(Policy = "LichSuKhauHaoGetById")]
        public async Task<IActionResult> GetLichSuKhauHaoById(int id)
        {
            var result = await _lichSuKhauHaoService.GetLichSuKhauHaoByIdAsync(id);
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

        // GET: api/LichSuKhauHao/get-by-asset/{taiSanId}
        [HttpGet("get-by-asset/{taiSanId}")]
        [Authorize(Policy = "LichSuKhauHaoGetByAsset")]
        public async Task<IActionResult> GetLichSuKhauHaoByTaiSanId(int taiSanId)
        {
            var result = await _lichSuKhauHaoService.GetByTaiSanIdAsync(taiSanId);
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
