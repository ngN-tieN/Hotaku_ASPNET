using Hotaku.Persistence;
using Hotaku.Persistence.Repositories;

namespace Hotaku.Main
{
    public class AllConfigure
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAllGenericTypes(typeof(IRepository<>), new[] { typeof(Repository<>).Assembly });
        }
    }
}
