using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Services;
using PasswordManager.Services.Authentication;

namespace PasswordManager.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordService, PasswordService>();

        return services;
    }

    public static IServiceCollection AddClaimsPrincipal(this IServiceCollection services)
    {
        services.AddTransient<ClaimsPrincipal>(sp => sp.GetService<IHttpContextAccessor>()!.HttpContext?.User!);

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;  
            options.RequireHttpsMetadata = false;  
            options.TokenValidationParameters = new TokenValidationParameters()  
            {  
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Secret")!)),
                ValidateIssuer = false,  
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }; 
        });

        return services;
    }

    public static IServiceCollection ConfigureSecretOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecretOptions>(
            configuration.GetSection("SecretOptions")
        );

        return services;
    }

    public static IServiceCollection AddTokenService(this IServiceCollection services)
    {
        services.AddScoped<TokenService>();

        return services;
    }
}
