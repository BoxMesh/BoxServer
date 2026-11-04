using EdgePanel.API.Response;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace EdgePanel.API.Filter
{
    public class ExceptionFilter : IExceptionHandler
    {
        public static IResult NotFound(HttpContext _, ProblemDetailsFactory __)
        {
            return Results.Json(new APIError<object?> { Error = "Not Found" }, statusCode: 404);
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsJsonAsync(new APIError<object?> { Error = "Internal Server Error" }, cancellationToken);
            return true;
        }

    }
}