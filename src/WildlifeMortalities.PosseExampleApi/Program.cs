using NSwag;
using WildlifeMortalities.PosseExampleApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.PostProcess = document =>
    {
        document.Info.Version = "v1";
        document.Info.Title = "Posse example API";
        document.Info.Contact = new OpenApiContact
        {
            Name = "Jon Hodgins",
            Email = "jon.hodgins@yukon.ca",
            Url = "https://github.com/ytgov-env/wildlife-mortalities"
        };
    };

    config.GenerateEnumMappingDescription = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
    app.UseReDoc(config =>
    {
        config.Path = "/redoc";
        config.DocumentPath = "/swagger/v1/swagger.json";
    });
}

app.UseHttpsRedirection();

app.Run();
