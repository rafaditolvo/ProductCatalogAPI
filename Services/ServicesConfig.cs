// ServicesConfig.cs

using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// Classe estática para centralizar as configurações de serviços
public static class ServicesConfig
{
    // Configuração dos serviços relacionados a controllers
    public static void ConfigureControllers(this IServiceCollection services)
    {
        // Adiciona suporte a controllers na injeção de dependência
        services.AddControllers();
        
        // Adiciona suporte a geração de documentação da API com Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "Cadastro de Produtos", Version = "v1" });
        });
    }

    // Configuração do cliente HTTP
    public static void ConfigureHttpClient(this IServiceCollection services)
    {
        // Adiciona um cliente HTTP na injeção de dependência
        services.AddHttpClient("HttpClient").ConfigurePrimaryHttpMessageHandler(() =>
        {
            // Configura o tratamento de certificados para aceitar todos (usado para desenvolvimento)
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
        });
    }

    // Configuração do contexto do banco de dados
   public static void ConfigureDbContext(this IServiceCollection services, string? connectionString)
    {
        // Adiciona o contexto do banco de dados na injeção de dependência, usando o SQL Server
        services.AddDbContext<AppDbContext>(options =>
        {
            // Configuração da conexão com o SQL Server, incluindo opções de tratamento de falhas
            options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
            });
        });
    }

    // Configuração de validação de certificados SSL
    public static void ConfigureCertificateValidationCallback()
    {
        // Configura a lógica de validação de certificados SSL
        ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
        {
            // Aceita certificados se não houver erros de política
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;


            // Rejeita certificados em caso de erros de política
            return false;
        };
    }
}
