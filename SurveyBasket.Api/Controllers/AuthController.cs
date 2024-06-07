﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using SurveyBasket.Api.Authentication;
//using SurveyBasket.Api.Services;

//namespace SurveyBasket.Api.Controllers
//{
//    [Route("[controller]")]
//    [ApiController]
//    public class AuthController : Controller
//    {
//        private readonly IAuthService _authService;
//        private readonly JwtOptions _options;
//        private readonly JwtOptions _optionsSnapshot;
//        private readonly JwtOptions _optionsMonitor;

//        public AuthController(IAuthService authService,
//            IOptions<JwtOptions> options,
//            IOptionsSnapshot<JwtOptions> optionsSnapshot,
//            IOptionsMonitor<JwtOptions> optionsMonitor)
//        {
//            _authService = authService;
//            _options = options;
//            _optionsSnapshot = optionsSnapshot;
//            _optionsMonitor = optionsSnapshot;
//        }

//        [HttpPost("")]
//        public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
//        {
//            var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

//            return authResult is null ? BadRequest("Invalid Email/Password") : Ok(authResult);
//        }

//        [HttpGet("Test")]
//        public IActionResult Test()
//        {
//            return Ok(_jwtOptions.Audience);
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

            return authResult is null ? BadRequest("Invalid Email/Password") : Ok(authResult);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return authResult is null ? BadRequest("Invalid token") : Ok(authResult);
        }

        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return isRevoked ? Ok() : BadRequest("Operation failed");
        }
    }
}