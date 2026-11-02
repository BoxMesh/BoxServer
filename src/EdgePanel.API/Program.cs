using EdgePanel.Filter;
using EdgePanel.Infrastructure;
using EdgePanel.Response;

using Microsoft.AspNetCore.Mvc.Infrastructure;

public partial class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        // GRPC Service
        builder.Services.AddGrpc();
        // Restful Service
        builder.Services.AddControllers(
            options =>
            {
                options.Filters.Add<ResponseFilter>();
            }
        );
        builder.Services.AddOpenApi();
        builder.Services.AddInfrastructure(builder.Configuration);
        WebApplication app = builder.Build();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.MapFallback("{*path}",
        (HttpContext http, ProblemDetailsFactory problems) =>
            Results.Json(
                new APIError<object?> { Error = problems.CreateProblemDetails(http, StatusCodes.Status404NotFound).Title, },
                statusCode: StatusCodes.Status404NotFound));
        app.Run();
    }
}