using System;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace Beginor.Owin.WebApi.Windsor {

    public static class Extensions {

        public static void UseWindsorContainer(this HttpConfiguration config, IWindsorContainer container) {
            if (config == null) {
                throw new ArgumentNullException("config");
            }
            if (container == null) {
                throw new ArgumentNullException("container");
            }
            // register windsor dependency resolver.
            container.Register(
                Component.For<IDependencyResolver>().ImplementedBy<WindsorDependencyResolver>()
            );
            config.DependencyResolver = container.Resolve<IDependencyResolver>();
        }

    }

}