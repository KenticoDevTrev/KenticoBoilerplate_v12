using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using Boilerplate.App_Start;
using CMS.Helpers;
using Kentico.Web.Mvc;

namespace Boilerplate
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // Register services both CMS and custom
            DependencyInjectionConfig.RegisterDependencies();

            // Enables and configures selected Kentico ASP.NET MVC integration features
            ApplicationConfig.RegisterFeatures(ApplicationBuilder.Current);

            // Registers routes including system routes for enabled features
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Custom Bundles
            Bundles.RegisterBundles(BundleTable.Bundles);

            // Clear cache on application start.
            CacheHelper.ClearCache();
        }
    }
}
