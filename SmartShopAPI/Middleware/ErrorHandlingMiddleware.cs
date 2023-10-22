using Newtonsoft.Json;
using SmartShopAPI.Exceptions;

namespace SmartShopAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new { error = notFoundException.Message });
                await context.Response.WriteAsync(result);

            }
            catch (BadRequestException badRequestException)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new { error = badRequestException.Message });
                await context.Response.WriteAsync(result);
            }
            catch(ForbidException forbidException)
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType= "application/json";
                var result = JsonConvert.SerializeObject(new { error = forbidException.Message });
                await context.Response.WriteAsync(result);
            }
        }

    }
}
