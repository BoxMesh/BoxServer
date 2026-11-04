using EdgePanel.API.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EdgePanel.API.Filter
{
    public class ResponseFilter : IAsyncAlwaysRunResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult result)
            {
                if (result.Value is ProblemDetails)
                {
                    result.Value = new APIError<object?> { Error = result.Value };
                    result.ContentTypes.Clear();
                }
                else
                {
                    result.Value = new APIResponse<object?> { Data = result.Value };
                }
                result.DeclaredType = result.Value.GetType();
            }
            await next();
        }
    }
}