using Auth.API.Models;

namespace Auth.API.Utils.JWT
{
    public interface ITokenManager
    {
        public string GenerateJwt(string key, string issuer, User user);
    }
}