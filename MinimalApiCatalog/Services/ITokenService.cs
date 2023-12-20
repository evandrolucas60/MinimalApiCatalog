using MinimalApiCatalog.Models;

namespace MinimalApiCatalog.Services
{
    public interface ITokenService
    {
        string GenerateToken(string key, string issuer, string audience, UserModel user);
    }
}
