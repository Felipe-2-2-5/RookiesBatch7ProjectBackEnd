using Backend.Application.IRepositories;
using Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

public class CheckUserMidleware
{
    private readonly RequestDelegate _next;

    public CheckUserMidleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            var tokenRole = jwtToken?.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var userId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var user = await userRepository.GetByIdAsync(Int16.Parse(userId!));
            var userModifiedAt = user!.Type.ToString();

            if (tokenRole != userModifiedAt)
            {
                throw new ForbiddenException("Your information has been modified");
            }
            if (user.IsDeleted == true)
            {
                throw new ForbiddenException("Your accout has been disabled");
            }
        }

        await _next(context);
    }
}