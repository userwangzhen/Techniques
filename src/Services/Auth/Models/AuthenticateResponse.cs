using WangZhen.Techniques.Auth.Api.Entities;

namespace WangZhen.Techniques.Auth.Api.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public string Token { get; private set; }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            FirstName = user.UserName;
            LastName = user.UserName;
            Username = user.UserName;
            Token = token;
        }
    }
}
