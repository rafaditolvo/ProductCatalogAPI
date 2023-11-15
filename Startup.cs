
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection") ?? "DefaultFallbackConnection";

        services.ConfigureControllers();
        services.ConfigureHttpClient();
        services.ConfigureDbContext(connectionString);


        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cadastro de Produtos", Version = "v1" });
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<RabbitMQService>();



        ServicesConfig.ConfigureCertificateValidationCallback();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cadastro de Produtos"));
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    // Startup.cs


}



