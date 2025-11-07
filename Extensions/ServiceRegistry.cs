using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace ElectronicBidding.Extensions
{
    public static class ServiceRegistery
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration config)
        {

            AddSwaggerConfiguration(services, config);
            return services;
        }

        private static void AddSwaggerConfiguration(IServiceCollection services, IConfiguration config)
        {
            
            services.AddAuthentication(configureOptions: x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                var secret = config.GetValue<string>("Authentication:Secret");
                Console.WriteLine(secret);
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.ASCII.GetBytes(secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EWaste Bidding", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT authorization header using the bearer scheme.
                Enter 'Bearer' [Space] and then your token in the text input below
                Example: 'Bearer 12343abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                      new OpenApiSecurityScheme
                       {
                            Reference = new OpenApiReference
                            {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                            },
                                Scheme = "0auth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                       },

                        new List<string>()
                }});

            });
        }
    }
}
