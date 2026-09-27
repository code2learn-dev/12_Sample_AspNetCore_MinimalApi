using System.Net;
using System.Net.Mime;

namespace MinimalApiSample.MiddleWares
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if(context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                context.Response.ContentType = MediaTypeNames.Text.Plain;
                await context.Response.WriteAsync("Page Not Found");
            }
        }
    }
}
