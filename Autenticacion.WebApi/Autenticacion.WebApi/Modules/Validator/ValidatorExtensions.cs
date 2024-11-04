using Autenticacion.WebApi.Aplicacion.Validadores;

namespace Autenticacion.WebApi.Modules.Validator;


public static class ValidatorExtensions
{

    public static IServiceCollection AddValidator(this IServiceCollection services)
    {
        services.AddTransient<UsuarioDtoValidador>();
        services.AddTransient<UsuarioLoginDtoValidador>();

        return services;
    }
}
