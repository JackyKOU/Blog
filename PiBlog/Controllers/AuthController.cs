using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AspNet.Security.OAuth.GitHub;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PiBlog.Dto;
using PiBlog.Dto.Blog;
using PiBlog.Interface;
using PiBlog.Configuations;
using PiBlog.Util;

namespace PiBlog.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AuthController:ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private const string LoginProviderKey = "LoginProvider";

        private readonly IConfiguration _configuration;
        public AuthController(IHttpContextAccessor contextAccessor,IConfiguration configuration)
        {
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        /// <summary>
        /// Get Github login url
        /// </summary>
        /// <returns></returns>
        [HttpGet("~/signin")]
        // [HttpGet]
        // [Route("signin")]
        public async Task<IActionResult>SigninGithubAccount(string provider,string redirectUrl)
        {
            // Note: the "provider" parameter corresponds to the external
            // authentication provider choosen by the user agent.
            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest();
            }

            if(!await this.HttpContext.IsProviderSupportedAsync(provider))
            {
                return BadRequest();
            }

            var request = _contextAccessor.HttpContext.Request;
            var url =
                $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}-callback?provider={provider}&redirectUrl={redirectUrl}";
            var properties = new AuthenticationProperties { RedirectUri = url };
            properties.Items[LoginProviderKey] = provider;
            return Challenge(properties, provider);
        }

        /// <summary>
        /// 授权成功后自动回调的地址
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="redirectUrl">授权成功后的跳转地址</param>
        /// <returns></returns>
        // [HttpGet]
        // [Route("signin-callback")]
        [HttpGet("~/signin-callback")]
        public async Task<IActionResult> SigninGithubCallBack(string provider = null, string redirectUrl = "")
        {
            var authenticateResult = await _contextAccessor.HttpContext.AuthenticateAsync(provider);
            if (!authenticateResult.Succeeded) return Redirect(redirectUrl);
            var openIdClaim = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier);
            if (openIdClaim == null || string.IsNullOrWhiteSpace(openIdClaim.Value))
                return Redirect(redirectUrl);

            string token = this.CreateToken(authenticateResult.Principal);

            return Redirect($"{redirectUrl}?token={token}");
        }
        
        private string CreateToken(ClaimsPrincipal claimsPrincipal)
        {
            var handler = new JwtSecurityTokenHandler();
            //The algorithm: 'HS256' requires the SecurityKey.KeySize to be greater than '128' bits
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppSettings.JWT.SecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                AppSettings.JWT.Issuer,
                AppSettings.JWT.Audience,
                claimsPrincipal.Claims,
                expires: DateTime.Now.AddMinutes(AppSettings.JWT.Expires),
                signingCredentials: credentials
            );

            return handler.WriteToken(token);
        }
    }
}