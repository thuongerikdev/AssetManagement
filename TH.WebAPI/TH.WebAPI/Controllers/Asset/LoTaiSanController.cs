//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using TH.Asset.ApplicationService.Service;
//using TH.Asset.Dtos;

//namespace TH.WebAPI.Controllers.Asset
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class LoTaiSanController : Controller
//    {
//        private readonly ILoTaiSanService _loTaiSanService;

//        public LoTaiSanController(ILoTaiSanService loTaiSanService)
//        {
//            _loTaiSanService = loTaiSanService;
//        }

//        // POST: api/LoTaiSan/create
//        [HttpPost("create")]
//        public async Task<IActionResult> CreateLoTaiSan([FromBody] CreateLoTaiSanRequestDto request)
//        {
//            var result = await _loTaiSanService.CreateLoTaiSanAsync(request);
//            if (result.ErrorCode == 200)
//            {
//                return Ok(result);
//            }
//            return BadRequest(result);
//        }

//        // PUT: api/LoTaiSan/update
//        [HttpPut("update")]
//        public async Task<IActionResult> UpdateLoTaiSan([FromBody] UpdateLoTaiSanRequestDto request)
//        {
//            var result = await _loTaiSanService.UpdateLoTaiSanAsync(request);
//            if (result.ErrorCode == 200)
//            {
//                return Ok(result);
//            }
//            // Trả về NotFound nếu mã lỗi là 404
//            if (result.ErrorCode == 404)
//            {
//                return NotFound(result);
//            }
//            return BadRequest(result);
//        }

//        // DELETE: api/LoTaiSan/delete/5
//        [HttpDelete("delete/{id}")]
//        public async Task<IActionResult> DeleteLoTaiSan(int id)
//        {
//            var result = await _loTaiSanService.DeleteLoTaiSanAsync(id);
//            if (result.ErrorCode == 200)
//            {
//                return Ok(result);
//            }
//            // Trả về NotFound nếu mã lỗi là 404
//            if (result.ErrorCode == 404)
//            {
//                return NotFound(result);
//            }
//            return BadRequest(result);
//        }

//        // GET: api/LoTaiSan/get-all
//        [HttpGet("get-all")]
//        public async Task<IActionResult> GetAllLoTaiSan()
//        {
//            var result = await _loTaiSanService.GetAllLoTaiSanAsync();
//            if (result.ErrorCode == 200)
//            {
//                return Ok(result);
//            }
//            return BadRequest(result);
//        }

//        // GET: api/LoTaiSan/get/5
//        [HttpGet("get/{id}")]
//        public async Task<IActionResult> GetLoTaiSanById(int id)
//        {
//            var result = await _loTaiSanService.GetLoTaiSanByIdAsync(id);
//            if (result.ErrorCode == 200)
//            {
//                return Ok(result);
//            }
//            // Trả về NotFound nếu mã lỗi là 404
//            if (result.ErrorCode == 404)
//            {
//                return NotFound(result);
//            }
//            return BadRequest(result);
//        }
//    }
//}