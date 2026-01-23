using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using RentACarServer.Application;
using RentACarServer.Infrastructure;
using RentACarServer.WebAPI;
using RentACarServer.WebAPI.Middlewares;
using RentACarServer.WebAPI.Modules;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;
using RentACarServer.WebAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddRateLimiter(cfr =>
{
    cfr.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.QueueLimit = 100;
        opt.Window = TimeSpan.FromSeconds(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("login-fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("forgot-password-fixed", opt =>
    {
        opt.PermitLimit = 2;
        opt.Window = TimeSpan.FromMinutes(5);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("reset-password-fixed", opt =>
    {
        opt.PermitLimit = 3;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("check-forgot-password-code-fixed", opt =>
    {
        opt.PermitLimit = 2;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddControllers()
    .AddOData(opt =>
    {
        opt.Select().Filter().Count().Expand().OrderBy().SetMaxTop(null)
            .AddRouteComponents("odata", MainODataController.GetEdmModel());
    });

builder.Services.AddCors();
builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); }); 
builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();
builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});

builder.Services.AddTransient<CheckTokenMiddleware>();
builder.Services.AddHostedService<CheckLoginTokenBackgroundService>();

var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetPreflightMaxAge(TimeSpan.FromMinutes(10)));

app.UseResponseCompression();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.UseMiddleware<CheckTokenMiddleware>();

app.UseRateLimiter();

app.MapControllers()
    .RequireRateLimiting("fixed")
    .RequireAuthorization();

app.MapAuth();
app.MapBranch();
app.MapRole();
app.MapPermission();
app.MapUser();
app.MapCategory();
app.MapProtectionPackage();
app.MapRentalExtra();
app.MapVehicle();
app.MapSeedData();

app.MapGet("/", () => Results.Ok("Hello World")).RequireAuthorization();

//await app.CreateFirstUser();
await app.CleanRemovedPermissionsFromRoleAsync();
app.Run();
