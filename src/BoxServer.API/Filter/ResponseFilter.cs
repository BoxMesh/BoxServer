using BoxServer.API.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BoxServer.API.Filter
{
    public class ResponseFilter : IAsyncAlwaysRunResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult result)
            {
                if (result.Value is ProblemDetails problemDetails)
                {
                    result.Value = new APIError<object?> { Error = problemDetails.Title };
                    result.ContentTypes.Clear();
                }
                else
                {
                    if (result.StatusCode >= 200 && result.StatusCode < 300)
                    {
                        result.Value = new APIResponse<object?> { Data = result.Value };    
                    }
                    else
                    {
                        result.Value = new APIError<object?> { Error = result.Value };    
                    }
                    
                }
                result.DeclaredType = result.Value.GetType();
            }
            await next();
        }
    }
}