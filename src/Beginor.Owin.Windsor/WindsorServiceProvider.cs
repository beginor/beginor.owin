using System;
using Castle.Core.Logging;
using Castle.Windsor;

namespace Beginor.Owin.Windsor {

    public class WindsorServiceProvider : IServiceProvider {

        private readonly IWindsorContainer container;

        private ILogger logger = NullLogger.Instance();

        public ILogger Logger {
            get { return logger; }
            set { logger = value; }
        }

        public WindsorServiceProvider(IWindsorContainer container) {
            this.container = container;
        }

        public object GetService(Type serviceType) {
            object service = null;
            try {
                service = container.Resolve(serviceType);
            }
            catch (Exception ex) {
                Logger.Error(string.Format("Can not resolve service of {0}", serviceType), ex);
            }
            return service;
        }
    }

}