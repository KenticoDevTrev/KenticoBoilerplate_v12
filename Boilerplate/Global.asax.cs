using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using CMS.Helpers;
using Controllers;
using Controllers.PageTemplates;
using Kentico.Caching;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;


public class MvcApplication : HttpApplication
{
    protected void Application_Start()
    {
        // Enables and configures selected Kentico ASP.NET MVC integration features
        ApplicationConfig.RegisterFeatures(ApplicationBuilder.Current);

        // Registers routes including system routes for enabled features
        RouteConfig.RegisterRoutes(RouteTable.Routes);

        // Custom Bundles
        Bundles.RegisterBundles(BundleTable.Bundles);

        // Clear cache on application start.
        // CacheHelper.ClearCache();

        RegisterPageTemplateFilters();

        #region "AutoFac Cache and other dependency injections"

        // Register AutoFac Items
        var builder = new ContainerBuilder();

        // Register Dependencies for Cache
        DependencyResolverConfig.Register(builder);

        // Register services and repository implementations
        AutoImplementConfig.RegisterImplementations(builder);

        // Set Autofac Dependency resolver to the builder
        DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));

        #endregion
    }

    private void RegisterPageTemplateFilters()
    {
        PageBuilderFilters.PageTemplates.Add(new TemplatedPageTemplateFilter());
        PageBuilderFilters.PageTemplates.Add(new KMVCHelper_GenericWidgetPageTemplateFilter());
        // Must be last!
        PageBuilderFilters.PageTemplates.Add(new EmptyPageTemplateFilter());
    }
}
