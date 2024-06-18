using Backend.Domain.Entity;
using System.Security.Claims;
using Backend.Domain.Entities;

namespace Backend.Application.AuthProvide
{
    public interface ITokenService
    {
        string GenerateJWT(IEnumerable<Claim>? additionalClaims = null);
        string GenerateJWTWithUser(User user, IEnumerable<Claim>? additionalClaims = null);
    }
}
