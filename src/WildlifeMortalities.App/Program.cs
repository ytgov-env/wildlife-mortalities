using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MudBlazor.Services;
using Serilog.Events;
using WildlifeMortalities.App.Features.Auth;
using WildlifeMortalities.App.HostedServices;
using WildlifeMortalities.Data;
using WildlifeMortalities.Shared.Services;
using WildlifeMortalities.Shared.Services.Posse;
using WildlifeMortalities.Shared.Services.Reports;

Log.Logger = new LoggerConfiguration().MinimumLevel
    .Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
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

    var configuration = builder.Configuration;

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddMudServices();
    builder.Services.AddLocalization();

    builder.Services.AddHostedService<PosseSyncService>();
    builder.Services.AddHttpClient<IPosseService, PosseService>(client =>
    {
        client.Timeout = TimeSpan.FromMinutes(20);
        client.BaseAddress = new Uri(configuration["Posse:ClientService:BaseAddress"]!);
        client.DefaultRequestHeaders.Add("api_key", configuration["Posse:ClientService:ApiKey"]);
    });
    builder.Services.AddScoped<IMortalityService, MortalityService>();
    builder.Services.AddScoped<PdfService>();
    builder.Services.AddScoped<ReportService>();
    builder.Services.AddSingleton<IAppConfigurationService, AppConfigurationService>();

    // Add authentication services
    var isAuthConfigurationMissingValues = configuration
        .GetRequiredSection("AuthNProvider")
        .GetChildren()
        .Any(child => string.IsNullOrWhiteSpace(child.Value));
    if (isAuthConfigurationMissingValues)
    {
        throw new Exception(
            "One or more children of AuthNProvider in appsettings are missing a value."
        );
    }
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
            configuration["AuthNProvider:Name"]!,
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
                options.SignedOutRedirectUri = configuration["AuthNProvider:SignedOutRedirectUri"]!;
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
                .ConfigureWarnings(
                    warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning)
                )
    );
    builder.Services.AddDbContextFactory<ReadOnlyAppDbContext>(
        options =>
            options
                .UseSqlServer(
                    configuration.GetConnectionString("AppDbContext"),
                    options => options.EnableRetryOnFailure()
                )
                .UseEnumCheckConstraints()
                .EnableSensitiveDataLogging()
                .ConfigureWarnings(
                    warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning)
                )
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
    builder.Services.AddDbContextFactory<ReadOnlyAppDbContext>(
        options =>
            options
                .UseSqlServer(
                    configuration.GetConnectionString("AppDbContext"),
                    options => options.EnableRetryOnFailure()
                )
                .UseEnumCheckConstraints()
    );
#endif

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
    app.UseRequestLocalization("en-CA");

    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapBlazorHub();
    app.MapAuthenticationEndpoints();

    app.MapFallbackToPage("/Host");

    Log.Logger = new LoggerConfiguration().MinimumLevel
        .Override("Default", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
#if DEBUG
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
        .WriteTo.Console()
#endif
        .WriteTo.Seq(configuration["Seq:Url"])
        .CreateLogger();

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
