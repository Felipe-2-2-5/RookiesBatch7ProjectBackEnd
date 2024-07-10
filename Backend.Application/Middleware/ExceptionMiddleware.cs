using Backend.Application.IRepositories;
using Backend.Domain.Exceptions;
using Backend.Domain.Resource;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.Middleware
{
    public class ExceptionMiddleware
    {
        #region Fields


        private readonly RequestDelegate _next;

        #endregion

        #region Constructors
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Methods


        public async Task Invoke(HttpContext context, IUserRepository userRepository)
        {
            try
            {
                /*if (context.User.Identity?.IsAuthenticated == true)
                {
                    var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

                    var tokenRole = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                    var userId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                    var user = await userRepository.GetByIdAsync(Int16.Parse(userId!));
                    var userRole = user!.Type.ToString();

                    if (tokenRole != userRole)
                    {
                        throw new ForbiddenException("Your information has been modified");
                    }
                    if (user.IsDeleted == true)
                    {
                        throw new ForbiddenException("Your accout has been disabled");
                    }
                    user = null;
                }*/
                await _next(context);
            }
            catch (DataInvalidException ex)
            {
                await HandleExceptionAsync(context, ex, ex.Message, StatusCodes.Status400BadRequest);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, ex.Message, StatusCodes.Status404NotFound);

            }

            catch (ConflictException ex)
            {
                await HandleExceptionAsync(context, ex, ex.Message, StatusCodes.Status409Conflict);

            }
            catch (ForbiddenException ex)
            {
                await HandleExceptionAsync(context, ex, ex.Message, StatusCodes.Status403Forbidden);
            }
            catch (NotAllowedException ex)
            {
                await HandleExceptionAsync(context, ex, ex.Message, StatusCodes.Status405MethodNotAllowed);
            }

            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, ResourceENG.Error_Exception, StatusCodes.Status500InternalServerError);
            }
        }

        private async Task HandleExceptionAsync<T>(HttpContext context, T exception, string userMessage, int statusCode) where T : Exception
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = statusCode;

            var err = new BaseException()
            {
                ErrorCode = statusCode,
                UserMessage = userMessage,
                DevMessage = exception.Message,
                TraceId = context.TraceIdentifier,
                MoreInfo = exception.HelpLink,
                Errors = exception.Data
            };

            await context.Response.WriteAsync(text: err.ToString() ?? "");
        }

        #endregion
    }
}
