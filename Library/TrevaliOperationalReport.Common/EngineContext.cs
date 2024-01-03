using StructureMap;
using System.Web;

namespace TrevaliOperationalReport.Common
{
    /// <summary>
    /// Class EngineContext.
    /// </summary>
    public static class EngineContext
    {
        /// <summary>
        /// Resoves the specified service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        public static T Resolve<T>()
        {
            return Container.GetInstance<T>();
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public static IContainer Container
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Items.Contains("_Container"))
                    {
                        var container = (IContainer)HttpContext.Current.Items["_Container"];
                        return container;
                    }
                }
                return IoC.Initialize().GetNestedContainer();
            }
            set
            {
                HttpContext.Current.Items["_Container"] = value;
            }
        }
    }
}
