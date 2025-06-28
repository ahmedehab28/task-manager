using Application.Interfaces;
using Infrastructure.Repositories.Boards;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IBoardRepository, InMemoryBoardRepository>();
            return services;
        }
    }
}
