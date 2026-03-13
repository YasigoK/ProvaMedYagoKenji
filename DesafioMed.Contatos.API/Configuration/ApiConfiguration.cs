using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DesafioMed.Contatos.API.Configuration;

public static class ApiConfiguration
{
    public static IServiceCollection AddApiVersioningAndSwagger(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";        
                options.SubstituteApiVersionInUrl = true; 
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.ConfigureOptions<ConfigureSwaggerByVersionOptions>();

        return services;
    }

    public static WebApplication UseSwaggerByVersion(this WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var desc in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{desc.GroupName}/swagger.json",
                    $"API de Contatos {desc.GroupName}"
                );
            }

            options.RoutePrefix = "swagger";
            options.DocumentTitle = "Api de Contatos";
        });

        return app;
    }
}

public sealed class ConfigureSwaggerByVersionOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerByVersionOptions(IApiVersionDescriptionProvider provider)
        => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var desc in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(desc.GroupName, new OpenApiInfo
            {
                Title = "API Contatos",
                Version = desc.ApiVersion.ToString(),
                Description = "API contatos "
            });
        }
    }
}
