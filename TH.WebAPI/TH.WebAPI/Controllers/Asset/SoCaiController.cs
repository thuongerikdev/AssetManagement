using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;

namespace TH.WebAPI.Controllers.Asset
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoCaiController : Controller
    {
        private readonly ISoCaiService _soCaiService;

        public SoCaiController(ISoCaiService soCaiService)
        {
            _soCaiService = soCaiService;
        }

        // GET: api/SoCai/tom-tat?fromDate=2025-01-01&toDate=2025-12-31
        [HttpGet("tom-tat")]
        public async Task<IActionResult> GetTomTat(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var result = await _soCaiService.GetTomTatAsync(fromDate, toDate);
            return result.ErrorCode == 200 ? Ok(result) : BadRequest(result);
        }

        // GET: api/SoCai/chi-tiet/211?fromDate=2025-01-01&toDate=2025-12-31
        [HttpGet("chi-tiet/{maTaiKhoan}")]
        public async Task<IActionResult> GetChiTiet(
            string maTaiKhoan,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var result = await _soCaiService.GetChiTietAsync(maTaiKhoan, fromDate, toDate);
            if (result.ErrorCode == 404) return NotFound(result);
            return result.ErrorCode == 200 ? Ok(result) : BadRequest(result);
        }
    }
}
