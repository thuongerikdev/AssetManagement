using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TH.Auth.ApplicationService.Service.MFA;

namespace TH.WebAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSessionController : Controller
    {
        private readonly IAuthUserSessionService _userSessionService;
        public UserSessionController(IAuthUserSessionService userSessionService)
        {
            _userSessionService = userSessionService;
        }
        [HttpGet("getall")]
        [Authorize(Policy = "UserSessionGetAll")]
        public async Task<IActionResult> GetAllUserSessions(CancellationToken ct)
        {
            var result = await _userSessionService.GetAllSessionsAsync(ct);
            if (result.ErrorCode != 200)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("getByUserId/{userId}")]
        [Authorize(Policy = "UserSessionGetByUserId")]
        public async Task<IActionResult> GetUserSessionsByUserId(int userId, CancellationToken ct)
        {
            var result = await _userSessionService.GetActiveSessionsByUserIdAsync(userId, ct);
            if (result.ErrorCode != 200)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("getBySessionId/{sessionId}")]
        [Authorize(Policy = "UserSessionGetBySessionId")]
        public async Task<IActionResult> GetUserSessionBySessionId(int sessionId, CancellationToken ct)
        {
            var result = await _userSessionService.FindByIdAsync(sessionId, ct);
            if (result.ErrorCode != 200)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
