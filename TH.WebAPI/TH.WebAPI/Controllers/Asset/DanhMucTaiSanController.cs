using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;
using TH.Asset.Dtos;

namespace TH.WebAPI.Controllers.Asset
{
    [ApiController]
    [Route("api/[controller]")]
    public class DanhMucTaiSanController : Controller
    {
        private readonly IDanhMucTaiSanService _danhMucTaiSanService;

        public DanhMucTaiSanController(IDanhMucTaiSanService danhMucTaiSanService)
        {
            _danhMucTaiSanService = danhMucTaiSanService;
        }

        // POST: api/DanhMucTaiSan/create
        [HttpPost("create")]
        [Authorize(Policy = "DanhMucTaiSanCreate")]
        public async Task<IActionResult> CreateDanhMucTaiSan([FromBody] CreateDanhMucTaiSanRequestDto request)
        {
            var result = await _danhMucTaiSanService.CreateDanhMucTaiSanAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/DanhMucTaiSan/update
        [HttpPut("update")]
        [Authorize(Policy = "DanhMucTaiSanUpdate")]
        public async Task<IActionResult> UpdateDanhMucTaiSan([FromBody] UpdateDanhMucTaiSanRequestDto request)
        {
            var result = await _danhMucTaiSanService.UpdateDanhMucTaiSanAsync(request);
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

        // DELETE: api/DanhMucTaiSan/delete/5
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "DanhMucTaiSanDelete")]
        public async Task<IActionResult> DeleteDanhMucTaiSan(int id)
        {
            var result = await _danhMucTaiSanService.DeleteDanhMucTaiSanAsync(id);
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

        // GET: api/DanhMucTaiSan/get-all
        [HttpGet("get-all")]
        [Authorize(Policy = "DanhMucTaiSanGetAll")]
        public async Task<IActionResult> GetAllDanhMucTaiSan()
        {
            var result = await _danhMucTaiSanService.GetAllDanhMucTaiSanAsync();
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/DanhMucTaiSan/get/5
        [HttpGet("get/{id}")]
        [Authorize(Policy = "DanhMucTaiSanGetById")]
        public async Task<IActionResult> GetDanhMucTaiSanById(int id)
        {
            var result = await _danhMucTaiSanService.GetDanhMucTaiSanByIdAsync(id);
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
