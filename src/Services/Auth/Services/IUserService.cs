using WangZhen.Techniques.Auth.Api.Entities;
using WangZhen.Techniques.Auth.Api.Models;

using System.Collections.Generic;

namespace WangZhen.Techniques.Auth.Api.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);

        IEnumerable<User> GetAll();

        User GetById(int id);

        User Register(RegisterRequest model); 
    }
}
