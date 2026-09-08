using Microsoft.EntityFrameworkCore;
using VirtualPet.Application.Repositories;
using VirtualPet.Application.Services;
using VirtualPet.Domain.Services;
using VirtualPet.Infrastructure.Data;
using VirtualPet.Infrastructure.Repositories;

namespace VirtualPet.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVirtualPetApplication(this IServiceCollection services)
        {
            services.AddScoped<PetApplicationService>();
            services.AddScoped<UserApplicationService>();
            services.AddSingleton<PetEvolutionService>();

            return services;
        }

        public static IServiceCollection AddVirtualPetInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("VirtualPetDatabase")
                ?? throw new InvalidOperationException("Connection string 'VirtualPetDatabase' was not found.");

            services.AddDbContext<VirtualPetDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPetHistoryRepository, PetHistoryRepository>();

            return services;
        }
    }
}
