using Vorder.Application.Interfaces.Repositories;
using Vorder.Infrastructure.Data;
using Vorder.WebAPI.Helpers;

namespace Vorder.WebAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext _dbContext)
        {
            try
            {
                await _next(context);

            }
            catch (Exception ex)
            {
                await RequestLogService.LogRequest(_dbContext, context.Request, 500, ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
