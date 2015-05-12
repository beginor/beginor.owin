using System;
using Owin;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Beginor.Owin.Windsor {

    public static class Extensions {

        private static readonly string AppContainerName = "Beginor.Owin.Windsor"

        public static void UseWindsorContainer(this IAppBuilder app, IWindsorContainer container) {
            if (app == null) {
                throw new ArgumentNullException("app");
            }
            if (container == null) {
                throw new ArgumentNullException("container");
            }
            if (app.Properties.ContainsKey(AppContainerName)) {
                throw new InvalidOperationException("Container already exists.");
            }
            app.Properties.Add(AppContainerName, container);
        }

        public static void UseWindsorContainer(this IAppBuilder app, string path) {
            if (app == null) {
                throw new ArgumentNullException("app");
            }
            if (string.IsNullOrEmpty(path)) {
                throw new ArgumentNullException("path");
            }
            IWindsorContainer container = new WindsorContainer();
            container.Install(
                Configuration.FromXmlFile("windsor.config")
            );
            app.UseWindsorContainer(app, container);
        }

        public static IWindsorContainer GetWindsorContainer(this IAppBuilder app) {
            if (app == null) {
                throw new ArgumentNullException("app");
            }
            if (!app.Properties.ContainsKey(AppContainerName)) {
                throw new InvalidOperationException("Windsor container is not configured.");
            }
            return (IWindsorContainer)app.Properties[AppContainerName];
        }

    }

}
