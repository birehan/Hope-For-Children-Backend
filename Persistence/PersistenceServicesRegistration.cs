using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Application.Contracts.Persistence;

namespace Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            return services;
        }

    }
}