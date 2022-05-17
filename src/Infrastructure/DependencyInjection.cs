using System.IdentityModel.Tokens.Jwt;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Infrastructure.Persistence;
using bejebeje.admin.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace bejebeje.admin.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        IdentityModelEventSource.ShowPII = true;
        string authority = configuration["Cognito:Authority"];
        string clientId = configuration["Cognito:ClientId"];
        string clientSecret = configuration["Cognito:ClientSecret"];
        string connectionString = configuration["ConnectionString"];

        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = authority;
                options.RequireHttpsMetadata = false;
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.ClaimActions.MapUniqueJsonKey("role", "role");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "cognito:user", RoleClaimType = "cognito:groups"
                };
            });

        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}