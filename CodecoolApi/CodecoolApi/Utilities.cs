using CodecoolApi.Models;
using CodecoolApi.Repository;
using CodecoolApi.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CodecoolApi
{
    public static class Utilities
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Author>, Repository<Author>>();
            services.AddScoped<IRepository<Material>, Repository<Material>>();
            services.AddScoped<IRepository<MaterialType>, Repository<MaterialType>>();
            services.AddScoped<IRepository<Review>, Repository<Review>>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void ConfigureJWTAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
        {
            // AppSettings -> secret
            var appsettingsSection = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appsettingsSection);

            var appsettings = appsettingsSection.Get<AppSettings>();
            byte[] key = Encoding.ASCII.GetBytes(appsettings.Secret);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }
}
