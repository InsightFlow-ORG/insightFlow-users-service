using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using userService.src.dtos;
using userService.src.services;

namespace userService.src.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (User.Identity?.IsAuthenticated == true)
                return BadRequest(new { message = "Sesión activa." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _userService.GetUserByEmail(loginDto.Email);
            if (user == null)
                return Unauthorized("Email o contraseña inválidos.");

            if (user.IsActive == 0)
                return Unauthorized("Usuario inactivo.");

            if (!_userService.VerifyPassword(user, loginDto.Password))
                return Unauthorized("Email o contraseña inválidos.");

            var token = _tokenService.CreateToken(user);
            return Ok(new
            {
                userName = user.UserName,
                email = user.Email,
                token
            });
        }
    }
}