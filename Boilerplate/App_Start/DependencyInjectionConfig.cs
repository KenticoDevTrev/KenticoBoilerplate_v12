using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Core;
using Autofac.Builder;
using System.Web.Mvc;
using System.Reflection;
using Controllers;

namespace App_Start
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterDependencies()
        {
            // Initializes the Autofac builder instance
            var builder = new ContainerBuilder();

            // Adds a custom registration source (IRegistrationSource) that provides all services from the Kentico API
            builder.RegisterSource(new CMSRegistrationSource());
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Find all `Service` classes by their interface and resolve the attribute/class types
            var serviceIntefaces = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsInterface && x.Name.EndsWith("Service"));
            var serviceClasses = Assembly.GetExecutingAssembly().GetTypes().Where(x => !x.IsInterface && x.Name.EndsWith("Service")).Select(x => new ServiceRegistration { Service = x, Interface = x.GetInterfaces().FirstOrDefault(i => serviceIntefaces.Contains(i)) });
            
            foreach(var serviceClass in serviceClasses.Where(x => x.Interface != null))
            {
                builder.RegisterType(serviceClass.Service).As(serviceClass.Interface);
            }

            // Autowire Property Injection for controllers (can't have constructor injection)
            var allControllers = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(BaseController).IsAssignableFrom(type));
            foreach (var controller in allControllers)
            {
                builder.RegisterType(controller).PropertiesAutowired();
            }

            // Resolves the dependencies
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var allRegs = container.ComponentRegistry.Registrations;
        }
    }

    public class ServiceRegistration
    {
        public Type Service { get; set; }
        public Type Interface { get; set; }
    }

    public class CMSRegistrationSource : IRegistrationSource
    {
        /// <summary>
        /// Gets whether the registrations provided by this source are 1:1 adapters on top of other components (I.e. like Meta, Func or Owned.)
        /// </summary>
        public bool IsAdapterForIndividualComponents => false;

        /// <summary>
        /// Retrieves registrations for an unregistered service, to be used by the container.
        /// </summary>
        /// <param name="service">The service that was requested.</param>
        /// <param name="registrationAccessor">A function that will return existing registrations for a service.</param>
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            // Checks whether the container already contains an existing registration for the requested service
            if (registrationAccessor(service).Any())
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            // Checks that the requested service carries valid type information
            var swt = service as IServiceWithType;
            if (swt == null)
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            // Gets an instance of the requested service using the CMS.Core API
            object instance = null;
            if (CMS.Core.Service.IsRegistered(swt.ServiceType))
            {
                instance = CMS.Core.Service.Resolve(swt.ServiceType);
            }

            if (instance == null)
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            // Registers the service instance in the container
            return new[] { RegistrationBuilder.ForDelegate(swt.ServiceType, (c, p) => instance).CreateRegistration() };
        }
    }
}