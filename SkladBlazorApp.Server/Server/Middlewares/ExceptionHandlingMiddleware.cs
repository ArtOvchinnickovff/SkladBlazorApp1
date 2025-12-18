using SkladBlazorApp.Shared.Exceptions;
using System.Net;
using System.Text.Json;

namespace SkladBlazorApp.Server.Server.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await WriteError(context, ex.Message);
            }
            catch (BusinessException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await WriteError(context, ex.Message);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                await WriteError(context, "Внутренняя ошибка сервера");
            }
        }

        private static async Task WriteError(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(new { error = message });
            await context.Response.WriteAsync(json);
        }
    }
}

