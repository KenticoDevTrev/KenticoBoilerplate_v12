using Kentico.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Core;
using Autofac.Builder;
using System.Web.Mvc;
using System.Reflection;

namespace Boilerplate.App_Start
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterDependencies()
        {
            // Initializes the Autofac builder instance
            var builder = new ContainerBuilder();

            // Adds a custom registration source (IRegistrationSource) that provides all services from the Kentico API
            builder.RegisterSource(new CMSRegistrationSource());

            var allServiceInterfaces = Assembly.GetExecutingAssembly().GetTypes().Where(x => !x.IsInterface && x.Name.EndsWith("Service"));
            var serviceClasses = allServiceInterfaces.Select(i => new ServiceRegistration { Service = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAssignableFrom(i) && !x.IsInterface).FirstOrDefault(), Interface = i });

            foreach(var serviceClass in serviceClasses)
            {
                builder.RegisterType(serviceClass.Service).As(serviceClass.Interface);
            }

            // Resolves the dependencies
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
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