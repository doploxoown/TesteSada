using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskManagementAPI.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var errors = new Dictionary<string, string[]>();

            if (context.Items.ContainsKey("ModelState"))
            {
                if (context.Items["ModelState"] is ModelStateDictionary modelState && modelState != null && !modelState.IsValid)
                {
                    errors = modelState
                        .Where(x => x.Value?.Errors != null && x.Value.Errors.Count > 0)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                        );

                    var response = new { Errors = errors };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }
            }

            await _next(context);
        }
    }
}
