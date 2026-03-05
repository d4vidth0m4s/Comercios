using Comercios.Domain.IRepository;
using Comercios.Infrastructure.Data;
using Comercios.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Comercios.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ComerciosDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("supaComercios")));

            services.AddScoped<ICrearComerciosRepository, ComerciosRepository>();

            return services;
        }
    }
}
