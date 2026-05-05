using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;
using TH.Asset.Dtos;

namespace TH.WebAPI.Controllers.Asset
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChungTuController : Controller
    {
        private readonly IChungTuService _chungTuService;

        public ChungTuController(IChungTuService chungTuService)
        {
            _chungTuService = chungTuService;
        }

        // POST: api/ChungTu/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateChungTu([FromBody] CreateChungTuRequestDto request)
        {
            var result = await _chungTuService.CreateChungTuAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/ChungTu/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateChungTu([FromBody] UpdateChungTuRequestDto request)
        {
            var result = await _chungTuService.UpdateChungTuAsync(request);
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

        // DELETE: api/ChungTu/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteChungTu(int id)
        {
            var result = await _chungTuService.DeleteChungTuAsync(id);
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

        // GET: api/ChungTu/get-all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllChungTu()
        {
            var result = await _chungTuService.GetAllChungTuAsync();
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/ChungTu/get/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetChungTuById(int id)
        {
            var result = await _chungTuService.GetChungTuByIdAsync(id);
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
        //[HttpPost("post-voucher/{chungTuId}")]
        //public async Task<IActionResult> PostVoucher(int chungTuId)
        //{
        //    var result = await _chungTuService.P(chungTuId);
        //    if (result.ErrorCode == 200)
        //    {
        //        return Ok(result);
        //    }
        //    if (result.ErrorCode == 404)
        //    {
        //        return NotFound(result);
        //    }
        //    return BadRequest(result);
        //}

        [HttpGet("get-by-asset/{taiSanId}")]
        public async Task<IActionResult> GetChungTuByTaiSanId(int taiSanId)
        {
            var result = await _chungTuService.GetChungTuByTaiSanIdAsync(taiSanId);
            if (result.ErrorCode == 200) return Ok(result);
            if (result.ErrorCode == 404) return NotFound(result);
            return BadRequest(result);
        }

        // GET: api/ChungTu/generate-code?loaiChungTu=ghi_tang
        [HttpGet("generate-code")]
        public async Task<IActionResult> GenerateMaChungTu([FromQuery] string? loaiChungTu)
        {
            var result = await _chungTuService.GenerateMaChungTuAsync(loaiChungTu);
            return result.ErrorCode == 200 ? Ok(result) : BadRequest(result);
        }
    }
}