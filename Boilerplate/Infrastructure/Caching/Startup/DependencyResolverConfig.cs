using Autofac;
using Autofac.Extras.DynamicProxy2;
using Autofac.Integration.Mvc;
using CMS.DataEngine;
using CMS.SiteProvider;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MVCCaching.Kentico
{
    /// <summary>
    /// Registers required implementations to the Autofac container and set the container as ASP.NET MVC dependency resolver
    /// </summary>
    public static class DependencyResolverConfig
    {
        public static void Register(ContainerBuilder builder)
        {
            // Adds Kentico dependency logic for the Application
            ConfigureDependencyResolverForMvcApplication(builder);

            AttachCMSDependencyResolver(builder);
        }

        private static void ConfigureDependencyResolverForMvcApplication(ContainerBuilder builder)
        {
            // Enable property injection in view pages
            builder.RegisterSource(new ViewRegistrationSource());

            // Register web abstraction classes
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register repositories that use IRepository, passing culture and LatestVersionEnabled
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly)
                .Where(x => x.IsClass && !x.IsAbstract && typeof(IRepository).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .WithParameter((parameter, context) => parameter.Name == "cultureName", (parameter, context) => CultureInfo.CurrentUICulture.Name)
                .WithParameter((parameter, context) => parameter.Name == "latestVersionEnabled", (parameter, context) => IsPreviewEnabled())
                .EnableInterfaceInterceptors().InterceptedBy(typeof(CachingRepositoryDecorator))
                .InstancePerRequest();

            // Register services that use IService
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly)
                .Where(x => x.IsClass && !x.IsAbstract && typeof(IService).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            // Register providers of additional information about content items
            builder.RegisterType<ContentItemMetadataProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();

            // Register caching decorator for repositories
            builder.Register(context => new CachingRepositoryDecorator(GetCacheItemDuration(), context.Resolve<IContentItemMetadataProvider>(), IsCacheEnabled()))
                .InstancePerRequest();

            // Enable declaration of output cache dependencies in controllers
            builder.Register(context => new OutputCacheDependencies(context.Resolve<HttpResponseBase>(), context.Resolve<IContentItemMetadataProvider>(), IsCacheEnabled()))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            // Register cache implementation for ICacheHelper
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly)
                .Where(x => x.IsClass && !x.IsAbstract && typeof(ICacheHelper).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .InstancePerRequest();
        }

        /// <summary>
        /// Configures Autofac container to use CMS dependency resolver in case it cannot resolve a dependency.  I believe this also hooks up Kentico's Services which include Cache clearing dependency detection
        /// </summary>
        private static void AttachCMSDependencyResolver(ContainerBuilder builder)
        {
            builder.RegisterSource(new CMSRegistrationSource());
        }

        /// <summary>
        /// Helper to detect if Cache should be enabled based on the Preview
        /// </summary>
        /// <returns></returns>
        private static bool IsCacheEnabled()
        {
            return !IsPreviewEnabled();
        }

        /// <summary>
        /// Helper to detect if Preview is enabled for the current request
        /// </summary>
        /// <returns></returns>
        private static bool IsPreviewEnabled()
        {
            return HttpContext.Current.Kentico().Preview().Enabled;
        }

        /// <summary>
        /// Gets the Cache Minute either from the AppSetting, or from the Cache Minutes Setting for data calls
        /// </summary>
        /// <returns></returns>
        private static TimeSpan GetCacheItemDuration()
        {
            var value = ConfigurationManager.AppSettings["RepositoryCacheItemDuration"];

            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int seconds) && seconds > 0)
            {
                return TimeSpan.FromSeconds(seconds);
            }
            else
            {
                try
                {
                    return TimeSpan.FromMinutes(SettingsKeyInfoProvider.GetIntValue("CMSCacheMinutes", new SiteInfoIdentifier(SiteContext.CurrentSiteName)));
                }
                catch (Exception)
                {
                    return TimeSpan.Zero;
                }
            }
        }
    }
}