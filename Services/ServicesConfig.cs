// ServicesConfig.cs
//remover usings nao utilizados
using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


public static class ServicesConfig
{

    public static void ConfigureControllers(this IServiceCollection services)
    {

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "Cadastro de Produtos", Version = "v1" });
        });
    }


    public static void ConfigureHttpClient(this IServiceCollection services)
    {

        services.AddHttpClient("HttpClient").ConfigurePrimaryHttpMessageHandler(() =>
        {

            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
        });
    }


    public static void ConfigureDbContext(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {

            options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
            });
        });
    }


    public static void ConfigureCertificateValidationCallback()
    {

        ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
        {

            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;



            return false;
        };
    }
}
