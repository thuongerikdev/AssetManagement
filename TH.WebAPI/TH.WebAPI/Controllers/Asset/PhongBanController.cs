using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TH.Asset.ApplicationService.Service;
using TH.Asset.Dtos;
using TH.Auth.ApplicationService.Service.User;

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

        [HttpGet("my-info")]
        [Authorize]
        public async Task<IActionResult> GetMyDepartmentInfo([FromServices] IAuthUserService authUserService, CancellationToken ct)
        {
            // 1. Lấy userId từ Token hiện tại
            var userIdStr = User.FindFirstValue("userId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

            // 2. Lấy thông tin User
            var userRes = await authUserService.GetUserByIDAsync(userId, ct);
            if (userRes.ErrorCode != 200 || userRes.Data == null)
            {
                return BadRequest(new { ErrorCode = 400, ErrorMessage = "Không tìm thấy dữ liệu người dùng" });
            }

            var user = userRes.Data;

            // ==========================================
            // 3. KIỂM TRA TRƯỞNG PHÒNG TỪ RoleSlimDto
            // ==========================================
            bool isManager = false;

            if (user.roles != null)
            {
                foreach (var r in user.roles)
                {
                    // Lấy roleName từ RoleSlimDto (đổi thành r.RoleName nếu class của bạn viết hoa)
                    string roleName = r.roleName ?? "";

                    if (roleName.Contains("truong_phong", StringComparison.OrdinalIgnoreCase) ||
                        roleName.Contains("truong_phong_ban", StringComparison.OrdinalIgnoreCase) ||
                        roleName.Contains("giam_doc", StringComparison.OrdinalIgnoreCase) ||
                        roleName.Contains("admin", StringComparison.OrdinalIgnoreCase))
                    {
                        isManager = true;
                        break;
                    }
                }
            }

            // ==========================================
            // 4. LẤY TÊN PHÒNG BAN
            // ==========================================
            string tenPhongBan = "Chưa thuộc phòng ban nào";
            int? phongBanId = null;

            // Bây giờ DTO đã có departmentID rồi nên dòng này sẽ hết báo đỏ
            var deptIdString = user.departmentID;

            if (!string.IsNullOrEmpty(deptIdString) && int.TryParse(deptIdString, out int pId))
            {
                phongBanId = pId;
                var phongBanRes = await _phongBanService.GetPhongBanByIdAsync(pId);
                if (phongBanRes.ErrorCode == 200 && phongBanRes.Data != null)
                {
                    tenPhongBan = phongBanRes.Data.tenPhongBan ?? phongBanRes.Data.maPhongBan;
                }
            }

            // 5. Trả về đúng format Frontend đang chờ
            return Ok(new
            {
                errorCode = 200,
                data = new
                {
                    userId = userId,
                    departmentId = phongBanId,
                    departmentName = tenPhongBan,
                    isManager = isManager
                }
            });
        }
    }
}
