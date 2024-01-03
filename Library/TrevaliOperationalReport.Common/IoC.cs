using StructureMap;

namespace TrevaliOperationalReport.Common
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(
                x => x.Scan
                       (
                           scan =>
                           {
                               scan.Assembly("TrevaliOperationalReport.Service");
                               scan.Assembly("TrevaliOperationalReport");
                               scan.WithDefaultConventions();
                               scan.LookForRegistries();
                           }
                       )
              );
        }
    }
}