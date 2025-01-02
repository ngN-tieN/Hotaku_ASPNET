using DotNetEnv;
using Hotaku.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Hotaku.Shared.CustomAttribute;
namespace Hotaku.Main.Services
{
   
    public static class ServiceExtension
    {
        public static void AddDatabaseServices(this IServiceCollection services)
        {
            Env.Load("../Hotaku.Persistence/.env");
            var connectionString = Environment.GetEnvironmentVariable("HOTAKU_DB_CONNECTION");
            services.AddDbContext<HotakuContext>(opt => opt.UseNpgsql(connectionString));
        }

        //public static void AddAllGenericTypes(this IServiceCollection services
        //    , Type t
        //    , Assembly[] assemblies
        //    , bool additionalRegisterTypesByThemself = false
        //    , ServiceLifetime lifetime = ServiceLifetime.Transient
        //)
        //{
        //    var genericType = t;
        //    var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces()
        //        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType)));

        //    foreach (var type in typesFromAssemblies)
        //    {
        //        services.Add(new ServiceDescriptor(t, type, lifetime));
        //        if (additionalRegisterTypesByThemself)
        //            services.Add(new ServiceDescriptor(type, type, lifetime));
        //    }
        //}
        public static void RegisterRepositories(this IServiceCollection services, Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.DefinedTypes);

            // Find concrete types with interfaces that match the naming convention
            var interfaceImplementations = types
                .Where(type => !type.IsAbstract && !type.IsInterface) // Only concrete classes
                .Select(type => new
                {
                    Implementation = type,
                    Interfaces = type.GetInterfaces()
                        .Where(i =>
                            i.IsPublic &&
                            i != typeof(IDisposable) && // Ignore common base interfaces
                            i.Name == $"I{type.Name}") // Match naming convention (IUserRepo -> UserRepo)
                        .ToList(),
                    LifetimeAttribute = type.GetCustomAttributes(true) // Get all attributes, including inherited ones
                    .FirstOrDefault(attr => attr is ScopedAttribute || attr is TransientAttribute || attr is SingletonAttribute)
                })
                .Where(x => x.Interfaces.Any() && x.LifetimeAttribute != null); // Ensure there's at least one matching interface

            foreach (var mapping in interfaceImplementations) {

                ServiceLifetime lifetime = mapping.LifetimeAttribute switch
                {
                    ScopedAttribute _ => ServiceLifetime.Scoped,
                    TransientAttribute _ => ServiceLifetime.Transient,
                    SingletonAttribute _ => ServiceLifetime.Singleton,
                    _ => ServiceLifetime.Scoped // Default to Scoped
                };
                foreach (var @interface in mapping.Interfaces)
                {
                    // Handle open generics (like IRepository<T>) separately
                    if (@interface.IsGenericType && mapping.Implementation.IsGenericType)
                    {
                        var genericInterface = @interface.GetGenericTypeDefinition();
                        var genericImplementation = mapping.Implementation.GetGenericTypeDefinition();
                        if (genericInterface == genericImplementation)
                        {
                            services.Add(new ServiceDescriptor(@interface, mapping.Implementation, lifetime));
                        }
                    }
                    else
                    {
                        // Register the normal interfaces
                        services.Add(new ServiceDescriptor(@interface, mapping.Implementation, lifetime));
                    }
                }
            }
        }
    }
}
