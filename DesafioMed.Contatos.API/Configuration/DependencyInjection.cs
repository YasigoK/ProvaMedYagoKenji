using DesafioMed.Contatos.Application.Service;
using DesafioMed.Contatos.Application.Service.Interface;
using DesafioMed.Contatos.Infrastructure.Context;
using DesafioMed.Contatos.Infrastructure.Repositories;
using DesafioMed.Contatos.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace DesafioMed.Contatos.API.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddDbContext<ContatoContext>(opt =>
        {
            var cs = config.GetConnectionString("BdConnection");
            opt.UseSqlServer(cs);
        });

        services.AddScoped<IContatoRepository, ContatoRepository>();
        services.AddScoped<IContatoService, ContatoService>();



        return services;

    }
}