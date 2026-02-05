using Explorer.API.FileStorage;
using Explorer.API.Middleware;
using Explorer.API.Startup;
using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.API.BackgroundJobs;
using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var cultureInfo = new CultureInfo("en-US");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

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
        var idStr = httpContext.User?.FindFirst("id")?.Value;
        var hasUser = long.TryParse(idStr, out var userId);

        var baseKey =
            hasUser ? idStr! :
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

        var isPremium = false;

        if (hasUser)
        {
            
            using var scope = httpContext.RequestServices.CreateScope();
            var premiumService = scope.ServiceProvider.GetRequiredService<Explorer.Stakeholders.API.Public.Users.IPremiumService>();
            isPremium = premiumService.IsPremium(userId);
        }

        var permitLimit = isPremium ? 200 : 5;

        var tier = isPremium ? "p" : "f";
        var partitionKey = $"{baseKey}:{tier}";

        return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                Window = TimeSpan.FromHours(24),
                QueueLimit = 0,
                AutoReplenishment = true
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