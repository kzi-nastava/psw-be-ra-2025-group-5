using Explorer.API.FileStorage;
using Explorer.API.Middleware;
using Explorer.API.Startup;
using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.API.BackgroundJobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
const string corsPolicy = "_corsPolicy";
builder.Services.ConfigureCors(corsPolicy);
builder.Services.ConfigureAuth();

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

app.MapControllers();

app.Run();

// Required for automated tests
namespace Explorer.API
{
    public partial class Program { }
}