using Explorer.API.FileStorage;
using Explorer.API.Middleware;
using Explorer.API.Startup;
using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.API.BackgroundJobs;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
const string corsPolicy = "_corsPolicy";
builder.Services.ConfigureCors(corsPolicy);
builder.Services.ConfigureAuth();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("ai-chat", httpContext =>
    {
        var userKey =
            httpContext.User?.FindFirst("id")?.Value
            ?? httpContext.Connection.RemoteIpAddress?.ToString()
            ?? "anonymous";

        var isPremium =
            httpContext.User?.IsInRole("Premium") == true ||
            string.Equals(httpContext.User?.FindFirst("premium")?.Value, "true", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(httpContext.User?.FindFirst("isPremium")?.Value, "true", StringComparison.OrdinalIgnoreCase);

        var permitLimit = isPremium ? 200 : 5;

        return RateLimitPartition.GetFixedWindowLimiter(userKey, _ =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                Window = TimeSpan.FromHours(24),
                QueueLimit = 0
            });
    });
});

builder.Services.RegisterModules();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient();

builder.Services.AddHostedService<TourExpirationWorker>();
builder.Services.AddHostedService<DailyBadgeCheckBackgroundService>();
builder.Services.AddScoped<IImageStorage, FileSystemImageStorage>();

var app = builder.Build();

app.UseStaticFiles();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseRouting();
app.UseCors(corsPolicy);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();

// Required for automated tests
namespace Explorer.API
{
    public partial class Program { }
}