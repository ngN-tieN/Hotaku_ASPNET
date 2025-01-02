using Hotaku.Main.Services;
using Hotaku.Persistence;
using Hotaku.Persistence.Repositories;
using Hotaku.Persistence.Entities;
using System.Reflection;

namespace Hotaku.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddDatabaseServices();
            builder.Services.RegisterRepositories([typeof(UserRepository).Assembly]);
            builder.Services.RegisterRepositories([Assembly.GetExecutingAssembly()]);

            //var configure = new AllConfigure();
            //configure.ConfigureServices(builder.Services);

            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();



            app.Run();
        }
    }
}
