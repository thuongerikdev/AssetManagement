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

        [HttpGet("by-asset/{taiSanId}")]
        public async Task<IActionResult> GetByAsset(int taiSanId)
        {
            var result = await _service.GetByAssetIdAsync(taiSanId);
            return result.ErrorCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpPost("upload/{taiSanId}")]
        [RequestSizeLimit(20 * 1024 * 1024)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(int taiSanId, [FromForm] UploadTaiSanDinhKemRequest request)
        {
            var result = await _service.UploadAsync(taiSanId, request.file, request.moTa);
            if (result.ErrorCode == 404) return NotFound(result);
            return result.ErrorCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result.ErrorCode == 404) return NotFound(result);
            return result.ErrorCode == 200 ? Ok(result) : BadRequest(result);
        }
    }
}
