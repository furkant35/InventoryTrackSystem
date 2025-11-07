using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace InventoryTrackSystem.WebUI.Middlewares
{
    public class RedirectIfAuthenticatedMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectIfAuthenticatedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                if (path!.Contains("/auth/login") || path.Contains("/auth/register"))
                {
                    context.Response.Redirect("/Home/Index");
                    return;
                }
            }

                await _next(context);
        }
    }
}
