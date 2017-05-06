using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Core.Logging;
using Castle.MicroKernel;
using Castle.Windsor;

namespace Beginor.Owin.WebApi.Windsor {

    public class WindsorDependencyScope : IDependencyScope {

        private IWindsorContainer container;

        protected IWindsorContainer Container {
            get { return container; }
        }

        private ILogger logger = NullLogger.Instance;

        public ILogger Logger {
            get { return logger; }
            set { logger = value; }
        }

        public WindsorDependencyScope(IWindsorContainer container) {
            this.container = container;
        }

        public void Dispose() {
            container = null;
        }

        public object GetService(Type serviceType) {
            object service = null;
            try {
                service = container.Resolve(serviceType);
            }
            catch (ComponentNotFoundException ex) {
                Logger.Info($"{serviceType} is not registered.");
            }
            catch (Exception ex) {
                Logger.Warn(string.Format("Exception caught resolving service of {0}, return is null.", serviceType), ex);
            }
            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            var services = container.ResolveAll(serviceType).Cast<object>();
            if (!services.Any()) {
                Logger.Info($"Services of type {serviceType} is not registered.");
            }
            return services;
        }

    }

}
