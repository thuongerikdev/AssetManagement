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
        public async Task<IActionResult> UpdateDieuChuyen([FromBody] UpdateDieuChuyenTaiSanRequestDto request)
        {
            var result = await _dieuChuyenService.UpdateDieuChuyenTaiSanAsync(request);
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

        // DELETE: api/DieuChuyenTaiSan/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDieuChuyen(int id)
        {
            var result = await _dieuChuyenService.DeleteDieuChuyenTaiSanAsync(id);
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

        // GET: api/DieuChuyenTaiSan/get-all
        [HttpGet("get-all")]
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
        public async Task<IActionResult> GetDieuChuyenById(int id)
        {
            var result = await _dieuChuyenService.GetDieuChuyenTaiSanByIdAsync(id);
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