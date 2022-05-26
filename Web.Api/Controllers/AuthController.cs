using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Application.Interface;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        /// <summary>
        /// Generate Api Token for access
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost("Token/{client_secret}")]
        public async Task<IActionResult> AuthToken([FromRoute] string client_secret)
        {
            var obj = await _authService.GenerateAuth(client_secret);
            if (obj.ResponseCode=="00")
            {
                return Ok(obj);
            }
            else
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
