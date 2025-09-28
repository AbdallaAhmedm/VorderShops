using Microsoft.AspNetCore.Identity;
using Vorder.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireRole(ApplicationRoles.Admin))
    .AddPolicy("ShopOwnerPolicy", policy => policy.RequireRole(ApplicationRoles.ShopOwner))
    .AddPolicy("CustomerPolicy", policy => policy.RequireRole(ApplicationRoles.Customer));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vorder API V1"));

    var roleManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    if(!await roleManager.RoleExistsAsync(ApplicationRoles.Admin))
        await roleManager.CreateAsync(new IdentityRole<Guid>(ApplicationRoles.Admin) { NormalizedName = ApplicationRoles.AdminNormalizedName });
    if (!await roleManager.RoleExistsAsync(ApplicationRoles.ShopOwner))
        await roleManager.CreateAsync(new IdentityRole<Guid>(ApplicationRoles.ShopOwner) { NormalizedName = ApplicationRoles.ShopOwnerNormalizedName });
    if (!await roleManager.RoleExistsAsync(ApplicationRoles.Customer))
        await roleManager.CreateAsync(new IdentityRole<Guid>(ApplicationRoles.Customer) { NormalizedName = ApplicationRoles.CustomerNormalizedName });

}

app.UseHttpsRedirection();
app.UseMiddleware<ShopSubdomainValidationMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<ApplicationUser>();

app.Run();
