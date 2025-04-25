using balance.domain;
using System.Security.Claims;

namespace balance.services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(Applicationuser user, IList<string> roles);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
        string GenerateRefreshToken();
    }
}
