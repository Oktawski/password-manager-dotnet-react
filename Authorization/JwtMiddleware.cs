using PasswordManager.Services;

namespace PasswordManager.Authorization
{
    public class JwtMiddleware 
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IUserService userService, IJwtHelper jwtHelper)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtHelper.ValidateToken(token);

            if (userId is not null) 
            {
                httpContext.Items["User"] = userService.GetById((Guid) userId);
            }

            await _next(httpContext);
        }
    }
}