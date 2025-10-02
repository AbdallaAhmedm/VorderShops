using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Vorder.Application.Interfaces.Repositories;
using Vorder.Infrastructure.Data;
using Vorder.Infrastructure.Data.Repositories;
using Vorder.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

#region log
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Vorder API",
        Version = "v1",
        Description = "Vorder Web API"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
});

builder.Services.AddControllers();

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddScoped<IRequestLogRepository, RequestLogRepository>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireRole(ApplicationRoles.Admin))
    .AddPolicy("ShopOwnerPolicy", policy => policy.RequireRole(ApplicationRoles.ShopOwner))
    .AddPolicy("CustomerPolicy", policy => policy.RequireRole(ApplicationRoles.Customer));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/Vorder/swagger/v1/swagger.json", "My API V1");
//    c.RoutePrefix = string.Empty;
//});

var roleManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
if (!await roleManager.RoleExistsAsync(ApplicationRoles.Admin))
    await roleManager.CreateAsync(new IdentityRole<Guid>(ApplicationRoles.Admin) { NormalizedName = ApplicationRoles.AdminNormalizedName });
if (!await roleManager.RoleExistsAsync(ApplicationRoles.ShopOwner))
    await roleManager.CreateAsync(new IdentityRole<Guid>(ApplicationRoles.ShopOwner) { NormalizedName = ApplicationRoles.ShopOwnerNormalizedName });
if (!await roleManager.RoleExistsAsync(ApplicationRoles.Customer))
    await roleManager.CreateAsync(new IdentityRole<Guid>(ApplicationRoles.Customer) { NormalizedName = ApplicationRoles.CustomerNormalizedName });

app.UseHttpsRedirection();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ShopSubdomainValidationMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<ApplicationUser>();

app.Run();