using Application.Cards.Services;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Application.List.Services;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Repositories.Boards;
using Infrastructure.Services;
using Infrastructure.Services.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)));

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddSingleton<IBoardRepository, InMemoryBoardRepository>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();

            services.AddScoped<IProjectAuthorizationService, ProjectAuthorizationService>();
            services.AddScoped<IBoardAuthorizationService, BoardAuthorizationService>();
            services.AddScoped<ICardAuthorizationService, CardAuthorizationService>();

            services.AddScoped<ICardPositionService, CardPositionService>();
            services.AddScoped<IListPositionService, ListPositionService>();

            return services;
        }
    }
}
