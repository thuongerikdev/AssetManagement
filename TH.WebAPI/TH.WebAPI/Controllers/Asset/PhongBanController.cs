using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;
using TH.Asset.Dtos;

namespace TH.WebAPI.Controllers.Asset
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhongBanController : Controller
    {
        private readonly IPhongBanService _phongBanService;

        public PhongBanController(IPhongBanService phongBanService)
        {
            _phongBanService = phongBanService;
        }

        // POST: api/PhongBan/create
        [HttpPost("create")]
        [Authorize(Policy = "PhongBanCreate")]
        public async Task<IActionResult> CreatePhongBan([FromBody] CreatePhongBanRequestDto request)
        {
            var result = await _phongBanService.CreatePhongBanAsync(request);
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/PhongBan/update
        [HttpPut("update")]
        [Authorize(Policy = "PhongBanUpdate")]
        public async Task<IActionResult> UpdatePhongBan([FromBody] UpdatePhongBanRequestDto request)
        {
            var result = await _phongBanService.UpdatePhongBanAsync(request);
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

        // DELETE: api/PhongBan/delete/5
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "PhongBanDelete")]
        public async Task<IActionResult> DeletePhongBan(int id)
        {
            var result = await _phongBanService.DeletePhongBanAsync(id);
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

        // GET: api/PhongBan/get-all
        [HttpGet("get-all")]
        [Authorize(Policy = "PhongBanGetAll")]
        public async Task<IActionResult> GetAllPhongBan()
        {
            var result = await _phongBanService.GetAllPhongBanAsync();
            if (result.ErrorCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/PhongBan/get/5
        [HttpGet("get/{id}")]
        [Authorize(Policy = "PhongBanGetById")]
        public async Task<IActionResult> GetPhongBanById(int id)
        {
            var result = await _phongBanService.GetPhongBanByIdAsync(id);
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
