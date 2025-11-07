using ElectronicBidding.Application.Abstractions.Users;
using ElectronicBidding.Application.Implementation.Users;
using ElectronicBidding.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicBidding.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Route("register-user")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            var result = await _userService.Register(request);
            return result.IsSuccess ? Created(nameof(RegisterUser), result) : BadRequest(result);
        }

        [Route("login-user")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var result = await _userService.Login(request);
            return result.IsSuccess ? Ok( result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [Route("update-role")]
        [HttpPost]
        public async Task<IActionResult> UpdateRole(string userId, Role prefferdrole)
        {
            var result = await _userService.UpdateRole(userId, prefferdrole);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
