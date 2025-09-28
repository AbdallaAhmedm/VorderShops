using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vorder.Infrastructure.Data;

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

        
            //services.AddScoped<IPatientRepository, PatientRepository>();
            //services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            //services.AddScoped<IDoctorRepository, DoctorRepository>();

            return services;
        }
    }

