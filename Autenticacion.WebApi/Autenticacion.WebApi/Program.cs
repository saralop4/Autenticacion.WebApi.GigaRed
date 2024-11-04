using Autenticacion.WebApi.Modules.Feature;
using Autenticacion.WebApi.Modules.Injection;
using Autenticacion.WebApi.Modules.Mapper;
using Autenticacion.WebApi.Modules.Swagger;
using Autenticacion.WebApi.Modules.Validator;
using Autenticacion.WebApi.Modules.Versioning;
using Autenticacion.WebApi.Modules.WatchDog;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using WatchDog;

namespace Autenticacion.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddVersioning();
       // builder.Services.AddAuthentication(builder.Configuration);
        builder.Services.AddValidator();
        builder.Services.AddMapper();
        builder.Services.AddInjection(builder.Configuration);
        builder.Services.AddSwaggerDocumentation();
        builder.Services.AddWatchDog(builder.Configuration);

        builder.Services.AddCors(option =>
        {
            option.AddPolicy("policyApi", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });


        var app = builder.Build();

        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {

            app.UseDeveloperExceptionPage();
            app.UseSwagger(); //habilitamos el middleware para servir al swagger generated como un endpoint json
            app.UseSwaggerUI( // habilitamos el dashboard de swagger 
                c =>
                {
                    //SwaggerEndpoint ese metodo recibe dos parametros, el primero es la url, el segundo es el nombre del endpoint
                    //  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi Api Empresarial v1");

                    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
        }

        app.UseWatchDogExceptionLogger();
        app.UseHttpsRedirection();
        app.UseCors("policyApi");
        app.MapControllers();
        app.UseWatchDog(opt =>
        {
            opt.WatchPageUsername = builder.Configuration["WatchDog:WatchPageUsername"];
            opt.WatchPagePassword = builder.Configuration["WatchDog:WatchPagePassword"];
            //opt.WatchPageUsername = "admin";
            //opt.WatchPagePassword = "admin@123";
        });
        app.Run();

    }
}
