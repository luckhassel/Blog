using Blog.Application.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Blog.CrossCutting.DependencyInjection
{
    public static class AuthenticationDI
    {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationSettings = new ApplicationSettings();
            configuration.GetSection(nameof(ApplicationSettings)).Bind(applicationSettings);

            var encodedKey = Encoding.ASCII.GetBytes(applicationSettings.SecuritySettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(encodedKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationSettings)));
        }
    }
}
