using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog.Events;
using WildlifeMortalities.Data;
using WildlifeMortalities.Shared.Services;

Log.Logger = new LoggerConfiguration().MinimumLevel
    .Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web host");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host
        .UseSerilog(
            (context, services, configuration) =>
                configuration.ReadFrom
                    .Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
        )
        .UseWindowsService();

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddMudServices();

    builder.Services.AddScoped<MortalityService>();
    builder.Services.AddScoped<ClientService>();
    builder.Services.AddScoped<ConservationOfficerService>();

    var configuration = builder.Configuration;

    // Add authentication services
    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.LoginPath = $"/{configuration["AuthNProvider:LoginPath"]}";
            options.LogoutPath = $"/{configuration["AuthNProvider:LogoutPath"]}";
        })
        .AddOpenIdConnect(
            configuration["AuthNProvider:Name"],
            options =>
            {
                options.Authority = $"https://{configuration["AuthNProvider:Domain"]}";
                options.ClientId = configuration["AuthNProvider:ClientId"];
                options.ClientSecret = configuration["AuthNProvider:ClientSecret"];
                options.ResponseType = "code";

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");

                options.CallbackPath = configuration["AuthNProvider:CallbackPath"];
                options.SignedOutCallbackPath = configuration[
                    "AuthNProvider:SignedOutCallbackPath"
                ];
                options.SignedOutRedirectUri = configuration["AuthNProvider:SignedOutRedirectUri"];
                options.ClaimsIssuer = configuration["AuthNProvider:Name"];

                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProviderForSignOut = context =>
                    {
                        var logoutUri =
                            $"https://{configuration["AuthNProvider:Domain"]}{configuration["AuthNProvider:FederatedLogoutPartialUri"]}{configuration["AuthNProvider:ClientId"]}";

                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                var request = context.Request;
                                postLogoutUri =
                                    request.Scheme
                                    + "://"
                                    + request.Host
                                    + request.PathBase
                                    + postLogoutUri;
                            }

                            logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();

                        return Task.CompletedTask;
                    },
                    OnSignedOutCallbackRedirect = context =>
                    {
                        context.Response.Redirect(options.SignedOutRedirectUri);
                        context.HandleResponse();

                        return Task.CompletedTask;
                    }
                };
            }
        );

#if DEBUG
    builder.Services.AddDbContextFactory<AppDbContext>(
        options =>
            options
                .UseSqlServer(
                    configuration.GetConnectionString("AppDbContext"),
                    options => options.EnableRetryOnFailure()
                )
                .UseEnumCheckConstraints()
                .EnableSensitiveDataLogging()
    );
#else
    builder.Services.AddDbContextFactory<AppDbContext>(
        options =>
            options
                .UseSqlServer(
                    configuration.GetConnectionString("AppDbContext"),
                    options => options.EnableRetryOnFailure()
                )
                .UseEnumCheckConstraints()
    );
#endif

    //builder.Services.AddSwaggerDoc();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    //app.UseOpenApi();
    //app.UseSwaggerUi3(s => s.ConfigureDefaults());

    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    Console.WriteLine(ex.Message);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
