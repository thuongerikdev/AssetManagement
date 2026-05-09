using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "ThanhLyTaiSanCreate")]
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
        [Authorize(Policy = "ThanhLyTaiSanUpdate")]
        public async Task<IActionResult> UpdateThanhLy([FromBody] UpdateThanhLyTaiSanRequestDto request)
        {
            var result = await _thanhLyService.UpdateThanhLyTaiSanAsync(request);
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

        // DELETE: api/ThanhLyTaiSan/delete/5
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "ThanhLyTaiSanDelete")]
        public async Task<IActionResult> DeleteThanhLy(int id)
        {
            var result = await _thanhLyService.DeleteThanhLyTaiSanAsync(id);
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

        // GET: api/ThanhLyTaiSan/get-all
        [HttpGet("get-all")]
        [Authorize(Policy = "ThanhLyTaiSanGetAll")]
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
        [Authorize(Policy = "ThanhLyTaiSanGetById")]
        public async Task<IActionResult> GetThanhLyById(int id)
        {
            var result = await _thanhLyService.GetThanhLyTaiSanByIdAsync(id);
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

        // GET: api/ThanhLyTaiSan/get-by-asset/{taiSanId}
        [HttpGet("get-by-asset/{taiSanId}")]
        [Authorize(Policy = "ThanhLyTaiSanGetByAsset")]
        public async Task<IActionResult> GetThanhLyByTaiSanId(int taiSanId)
        {
            var result = await _thanhLyService.GetByTaiSanIdAsync(taiSanId);
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
