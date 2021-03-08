using WangZhen.Techniques.Auth.Api.Entities;
using WangZhen.Techniques.Auth.Api.Helpers;
using WangZhen.Techniques.Auth.Api.Services;


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WangZhen.Techniques.Auth.Api.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly JwtSetting _jwtSetting;

        public JwtMiddleware(RequestDelegate next, IOptions<JwtSetting> options)
        {
            _next = next;
            _jwtSetting = options.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split("").Last();

            if (token != null)
            {
                attachUserToContext(context,userService,token);
            }
            await _next(context);
        }

        public void attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHanlder = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSetting.Secret);
                tokenHanlder.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // 令牌过期时间0延迟
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken securityToken);

                var jwtToken = (JwtSecurityToken)securityToken;
                var userId = int.Parse(jwtToken.Claims.First(x=>x.Type=="id").Value);

                context.Items["User"] = new User
                {
                    Id=userId
                };

            }
            catch (Exception ex)
            {

            }
        }

    }
}
