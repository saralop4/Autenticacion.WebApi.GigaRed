using Autenticacion.WebApi.Aplicacion.Interfaces;
using Autenticacion.WebApi.Aplicacion.Servicios;
using Autenticacion.WebApi.Dominio.Interfaces;
using Autenticacion.WebApi.Dominio.Persistencia;
using Autenticacion.WebApi.Infraestructura.Repositorios;
using Autenticacion.WebApi.Transversal.Interfaces;
using Autenticacion.WebApi.Transversal.Logging;

namespace Autenticacion.WebApi.Modules.Injection;

public static class InjectionExtensions
{

    public static IServiceCollection AddInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration);
        services.AddSingleton<DapperContext>();
        services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
        services.AddScoped<IUsuarioServicio, UsuarioServicio>();
        services.AddScoped<IMenuRepositorio, MenuRepositorio>();


        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

        return services;
    }


}
