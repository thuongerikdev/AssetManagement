using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;
using TH.Asset.Dtos;

namespace TH.WebAPI.Controllers.Asset
{
    [ApiController]
    [Route("api/[controller]")]
    public class DieuChuyenTaiSanController : Controller
    {
        private readonly IDieuChuyenTaiSanService _dieuChuyenService;

        public DieuChuyenTaiSanController(IDieuChuyenTaiSanService dieuChuyenService)
        {
            _dieuChuyenService = dieuChuyenService;
        }

        // POST: api/DieuChuyenTaiSan/create
        [HttpPost("create")]
        [Authorize(Policy = "DieuChuyenTaiSanCreate")]
        public async Task<IActionResult> CreateDieuChuyen([FromBody] CreateDieuChuyenTaiSanRequestDto request)
        {
            var result = await _dieuChuyenService.CreateDieuChuyenTaiSanAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/DieuChuyenTaiSan/update
        [HttpPut("update")]
        [Authorize(Policy = "DieuChuyenTaiSanUpdate")]
        public async Task<IActionResult> UpdateDieuChuyen([FromBody] UpdateDieuChuyenTaiSanRequestDto request)
        {
            var result = await _dieuChuyenService.UpdateDieuChuyenTaiSanAsync(request);
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

        // DELETE: api/DieuChuyenTaiSan/delete/5
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "DieuChuyenTaiSanDelete")]
        public async Task<IActionResult> DeleteDieuChuyen(int id)
        {
            var result = await _dieuChuyenService.DeleteDieuChuyenTaiSanAsync(id);
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

        // GET: api/DieuChuyenTaiSan/get-all
        [HttpGet("get-all")]
        [Authorize(Policy = "DieuChuyenTaiSanGetAll")]
        public async Task<IActionResult> GetAllDieuChuyen()
        {
            var result = await _dieuChuyenService.GetAllDieuChuyenTaiSanAsync();
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/DieuChuyenTaiSan/get/5
        [HttpGet("get/{id}")]
        [Authorize(Policy = "DieuChuyenTaiSanGetById")]
        public async Task<IActionResult> GetDieuChuyenById(int id)
        {
            var result = await _dieuChuyenService.GetDieuChuyenTaiSanByIdAsync(id);
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

        // GET: api/DieuChuyenTaiSan/get-by-asset/{taiSanId}
        [HttpGet("get-by-asset/{taiSanId}")]
        [Authorize(Policy = "DieuChuyenTaiSanGetByAsset")]
        public async Task<IActionResult> GetDieuChuyenByTaiSanId(int taiSanId)
        {
            var result = await _dieuChuyenService.GetByTaiSanIdAsync(taiSanId);
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
