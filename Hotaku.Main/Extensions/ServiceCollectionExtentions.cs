using DotNetEnv;
using Hotaku.Main.Services;
using Hotaku.Persistence.Entities;
using Hotaku.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotaku.Main.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
        {
            Env.Load("../Hotaku.Persistence/.env");
            var connectionString = Environment.GetEnvironmentVariable("HOTAKU_DB_CONNECTION");

            services.AddDbContext<HotakuContext>(opt => opt.UseNpgsql(connectionString));
            return services;
        }

        public static IServiceCollection AddItemServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();

            return services;
        }

    }
}