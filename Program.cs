// Program.cs

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "DefaultFallbackConnection";

// Configuração dos serviços da aplicação usando os métodos de ServicesConfig
builder.Services.ConfigureControllers();
builder.Services.ConfigureHttpClient();
builder.Services.ConfigureDbContext(connectionString);
ServicesConfig.ConfigureCertificateValidationCallback();

var app = builder.Build();

// Adiciona a configuração de middleware para ambientes de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cadastro de Produtos"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
