using DotNetEnv;
using Hotaku.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hotaku.Persistence
{
    public static class ServiceExtension
    {
        public static void AddDatabaseServices(this IServiceCollection services)
        {
            Env.Load("../Hotaku.Persistence/.env");
            var connectionString = Environment.GetEnvironmentVariable("HOTAKU_DB_CONNECTION");
            services.AddDbContext<HotakuContext>(opt => opt.UseNpgsql(connectionString));
        }

        public static void AddAllGenericTypes(this IServiceCollection services
            , Type t
            , Assembly[] assemblies
            , bool additionalRegisterTypesByThemself = false
            , ServiceLifetime lifetime = ServiceLifetime.Transient
        )
        {
            var genericType = t;
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType)));

            foreach (var type in typesFromAssemblies)
            {
                services.Add(new ServiceDescriptor(t, type, lifetime));
                if (additionalRegisterTypesByThemself)
                    services.Add(new ServiceDescriptor(type, type, lifetime));
            }
        }
    }
}
