using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "DefaultFallbackConnection";

        builder.Services.ConfigureControllers();
        builder.Services.ConfigureHttpClient();
        builder.Services.ConfigureDbContext(connectionString);

        ConfigureCertificateValidationCallback();

        builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ProductService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cadastro de Produtos"));
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    // Lógica de validação de certificado personalizada
    private static void ConfigureCertificateValidationCallback()
    {
        ServicePointManager.ServerCertificateValidationCallback += ValidateCertificate;
    }

    private static bool ValidateCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
    {
        // Adicione sua lógica de validação de certificado aqui
        // Por padrão, estamos aceitando todos os certificados (inseguro)
        return true;
    }
}
