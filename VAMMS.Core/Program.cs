using dotenv.net;
using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Prometheus;
using Serilog;
using VAMMS.Core.Data;
using VAMMS.Core.Extensions;
using VAMMS.Core.Repositories;
using VAMMS.Core.Repositories.Interfaces;
using VAMMS.Core.Services;
using VAMMS.Core.Services.Interfaces;
using VAMMS.Core.Utils;
using VAMMS.Core.Validators;
using VAMMS.Shared.Dtos;
using VATSIM.Connect.AspNetCore.Server.Extensions;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry(options =>
{
    options.TracesSampleRate = 1.0;
});

builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
builder.Logging.AddSerilog(logger, dispose: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Format is the word Bearer, then a space, followed by the token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Environment.GetEnvironmentVariable("REDIS_HOST") ??
        throw new ArgumentNullException("REDIS_HOST env variable not found");
    options.InstanceName = "vzme_api";
});

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING") ??
       throw new ArgumentNullException("CONNECTION_STRING env variable not found"));
});

builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddScoped<IValidator<VisitorApplicationDto>, VisitorApplicationValidator>();

builder.Services.AddVatsimConnect<AuthenticationService>(options =>
{
    options.JwtBearerConfig.Secret = Environment.GetEnvironmentVariable("JWT_SECRET") ??
        throw new ArgumentNullException("JWT_SECRET env variable not found");
    options.JwtBearerConfig.Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ??
        throw new ArgumentNullException("JWT_ISSUER env variable not found");
    options.JwtBearerConfig.Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ??
        throw new ArgumentNullException("JWT_AUDIENCE env variable not found");
    options.JwtBearerConfig.AccessTokenExpiration = int.Parse(Environment.GetEnvironmentVariable("JWT_ACCESS_EXPIRATION") ??
        throw new ArgumentNullException("JWT_ACCESS_EXPIRATION env variable not found"));
    options.JwtBearerConfig.RefreshTokenExpiration = int.Parse(Environment.GetEnvironmentVariable("JWT_REFRESH_EXPIRATION") ??
        throw new ArgumentNullException("JWT_REFRESH_EXPIRATION env variable not found"));
    options.VatsimTokenRequestOptions.ClientId = Environment.GetEnvironmentVariable("VATSIM_CLIENT_ID") ??
        throw new ArgumentNullException("VATSIM_CLIENT_ID env variable not found");
    options.VatsimTokenRequestOptions.ClientSecret = Environment.GetEnvironmentVariable("VATSIM_CLIENT_SECRET") ??
        throw new ArgumentNullException("VATSIM_CLIENT_SECRET env variable not found");
    options.VatsimTokenRequestOptions.GrantType = Environment.GetEnvironmentVariable("VATSIM_GRANT_TYPE") ??
        throw new ArgumentNullException("VATSIM_GRANT_TYPE env variable not found");
    options.VatsimTokenRequestOptions.RedirectUri = Environment.GetEnvironmentVariable("VATSIM_REDIRECT_URL") ??
        throw new ArgumentNullException("VATSIM_REDIRECT_URL env variable not found");
    options.VatsimTokenRequestOptions.VatsimAuthUri = Environment.GetEnvironmentVariable("VATSIM_AUTH_URL") ??
        throw new ArgumentNullException("VATSIM_AUTH_URL env variable not found");
});
builder.Services.AddAuthorization(options =>
{
    options.AddAuthorizationPolicies();
});

builder.Services.AddScoped<IWebsiteLogService, WebsiteLogService>();
builder.Services.AddScoped<IVatsimService, VatsimService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseSentryTracing();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto
});

app.UseMetricServer();
app.UseHttpMetrics();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
