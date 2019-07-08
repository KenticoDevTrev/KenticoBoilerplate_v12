using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using App_Start;
using Boilerplate.Controllers.PageTypes;
using CMS.Helpers;
using Controllers.PageTemplates;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;


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

        RegisterPageTemplateFilters();
    }

    private void RegisterPageTemplateFilters()
    {
        PageBuilderFilters.PageTemplates.Add(new TemplatedPageTemplateFilter());
        PageBuilderFilters.PageTemplates.Add(new KMVCHelper_GenericWidgetPageTemplateFilter());
        // Must be last!
        PageBuilderFilters.PageTemplates.Add(new EmptyPageTemplateFilter());
    }
}
