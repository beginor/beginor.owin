using System;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using System.Web.Http.ExceptionHandling;

namespace Beginor.Owin.WebApi.Windsor {

    public static class Extensions {

        public static void UseWindsorContainer(this HttpConfiguration config, IWindsorContainer container) {
            if (config == null) {
                throw new ArgumentNullException("config");
            }
            if (container == null) {
                throw new ArgumentNullException("container");
            }

            container.Register(
                Component.For<IDependencyResolver>().ImplementedBy<WindsorDependencyResolver>()
            );

            var resolver = container.Resolve<IDependencyResolver>();
            // check web api service registions, is custom impl is not registered, register default impl
            CheckWebApiServiceRegistions(container, resolver);
            config.DependencyResolver = resolver;
        }

        static void CheckWebApiServiceRegistions(IWindsorContainer container, IDependencyResolver resolver) {
            if (resolver.GetService(typeof(IExceptionLogger)) == null) {
                container.Register(Component.For<IExceptionLogger>().ImplementedBy<CastleExceptionLogger>());
            }

        }
    }

}