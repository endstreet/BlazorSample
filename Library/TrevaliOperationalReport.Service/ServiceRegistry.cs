using StructureMap;
using StructureMap.Web;
using TrevaliOperationalReport.Common;
using TrevaliOperationalReport.Data;
using TrevaliOperationalReport.Data.Repository;
using TrevaliOperationalReport.Service.Caching;

namespace TrevaliOperationalReport.Service
{
    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });

            For<IDbContext>().HybridHttpOrThreadLocalScoped().Use(() => new TrevaliOperationalReportObjectContext(ConfigItems.ConnectionStringName));
            For(typeof(IRepository<>)).Use(typeof(EfRepository<>));
            For<ICacheManager>().Use<MemoryCacheManager>();
        }
    }
}
