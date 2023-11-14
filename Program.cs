var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "DefaultFallbackConnection";

builder.Services.ConfigureControllers();
builder.Services.ConfigureHttpClient();
builder.Services.ConfigureDbContext(connectionString);
ServicesConfig.ConfigureCertificateValidationCallback();


builder.Services.AddScoped<IProductRepository, ProductRepository>();


builder.Services.AddScoped<ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cadastro de Produtos"));
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
