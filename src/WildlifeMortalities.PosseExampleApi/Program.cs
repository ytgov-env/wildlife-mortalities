global using FastEndpoints;
using System.Text.Json.Serialization;
using FastEndpoints.Swagger;
using NSwag;
using WildlifeMortalities.PosseExampleApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization(o => o.AddPolicy("ApiKey", b => b.RequireAssertion(_ => true)));
builder.Services.AddSwaggerDoc(
    s =>
    {
        s.PostProcess = document =>
        {
            document.Info.Version = "v1";
            document.Info.Title = "posse example api";
            document.Info.Contact = new OpenApiContact
            {
                Name = "Jon Hodgins",
                Email = "jon.hodgins@yukon.ca",
                Url = "https://github.com/ytgov-env/wildlife-mortalities"
            };
        };

        s.AddAuth(
            "ApiKey",
            new OpenApiSecurityScheme
            {
                Name = "api_key", In = OpenApiSecurityApiKeyLocation.Header, Type = OpenApiSecuritySchemeType.ApiKey
            }
        );

        // s.GenerateEnumMappingDescription = true;
    },
    excludeNonFastEndpoints: true,
    addJWTBearerAuth: false,
    shortSchemaNames: true,
    removeEmptySchemas: true
);

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();
app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    // c.Endpoints.ShortNames = true;
    c.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
});
app.UseSwaggerGen();
app.Run();
