using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "BaoTriTaiSanCreate")]
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
        [Authorize(Policy = "BaoTriTaiSanUpdate")]
        public async Task<IActionResult> UpdateBaoTri([FromBody] UpdateBaoTriTaiSanRequestDto request)
        {
            var result = await _baoTriService.UpdateBaoTriTaiSanAsync(request);
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

        // DELETE: api/BaoTriTaiSan/delete/5
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "BaoTriTaiSanDelete")]
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
        [Authorize(Policy = "BaoTriTaiSanGetAll")]
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
        [Authorize(Policy = "BaoTriTaiSanGetById")]
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

        // GET: api/BaoTriTaiSan/get-by-asset/{taiSanId}
        [HttpGet("get-by-asset/{taiSanId}")]
        [Authorize(Policy = "BaoTriTaiSanGetByAsset")]
        public async Task<IActionResult> GetBaoTriByAssetId(int taiSanId)
        {
            var result = await _baoTriService.GetByTaiSanIdAsync(taiSanId);
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
