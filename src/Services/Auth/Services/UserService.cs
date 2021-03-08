
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using WangZhen.Techniques.Auth.Api.Entities;
using WangZhen.Techniques.Auth.Api.Helpers;
using WangZhen.Techniques.Auth.Api.Models;

namespace WangZhen.Techniques.Auth.Api.Services
{
    public class UserService : IUserService
    {
        private readonly JwtSetting _jwtSetting;

        public UserService(IOptions<JwtSetting> options)
        {
            _jwtSetting = options.Value;
        }


        private List<User> _users = new List<User>()
        {
            new User{Id=1, PassWord="duibuqi520@@", UserName="wangzhen", Role="Admin" },
            new User{ Id=2,PassWord="duibuqi520@@", UserName="liyou", Role="user"}
        };
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.UserName == model.userName && x.PassWord == model.passWord);
            if (user == null) return null;
            var token = generateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        private string generateJwtToken(User user)
        {
            var tokenHanlder = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSetting.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHanlder.CreateToken(tokenDescriptor);
            return tokenHanlder.WriteToken(token);
        }

        public User Register(RegisterRequest model)
        {
            var id = _users.Max(p => p.Id) + 1;
            var user = new User
            {
                Id = id,
                UserName = model.userName,
                PassWord = model.passWord,
                Role = "user"
            };
            _users.Add(user);
            return user;
        }
    }
}
