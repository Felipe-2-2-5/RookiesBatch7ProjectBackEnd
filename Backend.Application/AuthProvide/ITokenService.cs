using Backend.Domain.Entities;
using System.Security.Claims;

namespace Backend.Application.AuthProvide
{
    public interface ITokenService
    {
        string GenerateJWT(IEnumerable<Claim>? additionalClaims = null);
        string GenerateJWTWithUser(User user, IEnumerable<Claim>? additionalClaims = null);
    }
}
