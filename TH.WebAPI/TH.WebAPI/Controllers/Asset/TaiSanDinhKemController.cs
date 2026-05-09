using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;

namespace TH.WebAPI.Controllers.Asset
{
    public class UploadTaiSanDinhKemRequest
    {
        public IFormFile file { get; set; } = default!;
        public string? moTa { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class TaiSanDinhKemController : ControllerBase
    {
        private readonly ITaiSanDinhKemService _service;

        public TaiSanDinhKemController(ITaiSanDinhKemService service)
        {
            _service = service;
        }

        // GET: api/TaiSanDinhKem/by-asset/{taiSanId}
        [HttpGet("by-asset/{taiSanId}")]
        [Authorize(Policy = "TaiSanDinhKemGetByAsset")]
        public async Task<IActionResult> GetByAsset(int taiSanId)
        {
            var result = await _service.GetByAssetIdAsync(taiSanId);
            return result.ErrorCode == 200 ? Ok(result) : BadRequest(result);
        }

        // POST: api/TaiSanDinhKem/upload/{taiSanId}
        [HttpPost("upload/{taiSanId}")]
        [Authorize(Policy = "TaiSanDinhKemUpload")]
        [RequestSizeLimit(20 * 1024 * 1024)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(int taiSanId, [FromForm] UploadTaiSanDinhKemRequest request)
        {
            var result = await _service.UploadAsync(taiSanId, request.file, request.moTa);
            if (result.ErrorCode == 404) return NotFound(result);
            return result.ErrorCode == 200 ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/TaiSanDinhKem/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "TaiSanDinhKemDelete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result.ErrorCode == 404) return NotFound(result);
            return result.ErrorCode == 200 ? Ok(result) : BadRequest(result);
        }
    }
}
