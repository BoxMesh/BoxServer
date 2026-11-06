using System.Text;
using EdgePanel.API.Filter;
using EdgePanel.API.Secret;
using EdgePanel.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
public partial class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddScoped<JWTSecret>();
        builder.Services.AddOpenApi();
        builder.Services.AddExceptionHandler<ExceptionFilter>();
        builder.Services.AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer(
            options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT_ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT_ValidAudience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT_AccessKey"]!)
                    ),
                    ClockSkew = TimeSpan.Zero
                };
            }
        );
        builder.Services.AddAuthorization();
        builder.Services.AddControllers(
            options =>
            {
                options.Filters.Add<ResponseFilter>();
            }
        );
        builder.Services.AddProblemDetails();
        builder.Services.AddInfrastructure(builder.Configuration);
        
        builder.Services.AddCors(
            options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed(_ => true)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            }
        );
        WebApplication app = builder.Build();
        app.UseExceptionHandler();
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "v1");
        });
        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapFallback("{*path}", ExceptionFilter.NotFound);
        app.Run();
    }
}