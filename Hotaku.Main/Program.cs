using Hotaku.Main.Services;
using Hotaku.Persistence;
using Hotaku.Persistence.Repositories;
using Hotaku.Persistence.Entities;

namespace Hotaku.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddDatabaseServices();
            //builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddAllGenericTypes(typeof(IRepository<>), new[] { typeof(Repository<User>).Assembly });

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
