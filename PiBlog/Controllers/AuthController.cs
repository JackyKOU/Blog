using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController:ControllerBase
    {
        private readonly IHttpClientFactory _httpClient;
        public AuthController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get Github login url
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("login")]
        public async Task<IActionResult>GetGithubLoginUrl()
        {
            var resp = new Response<string>();
            resp.Data = string.Concat(new string[]
            {
                GitHubConfig.API_Authorize,
                "?client_id=",GitHubConfig.Client_ID,
                "&scope=",GitHubConfig.API_Scope,
                "&state=",Guid.NewGuid().ToString("N"),
                "&redirect_uri=",GitHubConfig.Redirect_Uri
            });
            
            return await Task.FromResult(Ok(resp));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("access_token")]
        public async Task<IActionResult> GetAccessToken(string code)
        {
            var resp = new Response<string>();
            if(string.IsNullOrEmpty(code))
            {
                resp.Msg = "code is null or empty";
                return Ok(resp);
            }

            var content = new StringContent($"code={code}&client_id={GitHubConfig.Client_ID}&redirect_uri={GitHubConfig.Redirect_Uri}&client_secret={GitHubConfig.Client_Secret}");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var client = _httpClient.CreateClient();
            var httpResponse = await client.PostAsync(GitHubConfig.API_AccessToken,content);
            var result =  await httpResponse.Content.ReadAsStringAsync();
            if(result.StartsWith("access_token"))
                resp.Data = result.Split("=")[1].Split("&").First();
            else
                resp.Msg = "code is not correct";

            return Ok(resp);
        }

        [HttpGet]
        [Route("token")]
        public async Task<IActionResult> GenerateToken(string access_token)
        {
            var resp = new Response<string>();

            if(string.IsNullOrEmpty(access_token))
            {
                resp.Msg = "access_token is empty";
                return Ok(resp);
            }

            var url = $"{GitHubConfig.API_User}?access_token={access_token}";
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Add("user-agent","Mozilla/5.0");
            var httpResponse = await client.GetAsync(url);
            if(httpResponse.StatusCode!=HttpStatusCode.OK)
            {
                resp.Msg = "access_token is not correct";
                return Ok(resp);
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var user = content.DeserializeFromJson<UserResponse>();
            if(user == null)
            {
                resp.Msg = "Not get user data";
                return Ok(resp);
            }

            if(user.id!=GitHubConfig.Id)
            {
                resp.Msg = "Account not be authorized";
                return Ok(resp);
            }

            var claims = new[]{
                new Claim(ClaimTypes.Name, user.name),
                new Claim(ClaimTypes.Email, user.email),
                //it is the expir date , set to AppSettings.JWT.Expires
                new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(AppSettings.JWT.Expires)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JWT.SecurityKey));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer:AppSettings.JWT.Domain,
                audience:AppSettings.JWT.Domain,
                claims:claims,
                expires: DateTime.Now.AddMinutes(AppSettings.JWT.Expires),
                signingCredentials:creds
            );

            var result = new JwtSecurityTokenHandler().WriteToken(token);
            resp.Data = result;
            return Ok(resp);

        }
    }
}