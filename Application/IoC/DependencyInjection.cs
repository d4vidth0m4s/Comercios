using Comercios.Application.Interfaz;
using Comercios.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Comercios.Application.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IComercioServices, CrearComercioServices>();

            return services;
        }
    }
}
