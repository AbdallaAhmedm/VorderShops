using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vorder.Application.Interfaces.Repositories;
using Vorder.Domain.Models;
using Vorder.Infrastructure.Data;
using Vorder.Infrastructure.Data.Repositories;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Vorder.Infrastructure"));

        });


        services.AddScoped<IShopRepository, ShopRepository>();
        services.AddScoped<IRequestLogRepository, RequestLogRepository>();

        return services;
    }
}

