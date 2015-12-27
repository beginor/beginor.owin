using System;
using Owin;
using Microsoft.Owin.Builder;
using Nowin;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Beginor.Owin.Windsor;
using Beginor.Owin.StaticFile;
using Beginor.Owin.WebApi.Windsor;
using System.Web.Http;
using Beginor.Owin.Security.Aes;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.Cookies;
using Castle.MicroKernel.Registration;
using Microsoft.Owin.Security;

namespace TestWithNowin {

    class MainClass {

        public static void Main(string[] args) {
            // create a new AppBuilder
            IAppBuilder app = new AppBuilder();
            // init nowin's owin server factory.
            OwinServerFactory.Initialize(app.Properties);

            Configure(app);

            var serverBuilder = new ServerBuilder();
            const string ip = "127.0.0.1";
            const int port = 8888;
            serverBuilder.SetAddress(IPAddress.Parse(ip)).SetPort(port)
                .SetOwinApp(app.Build())
                .SetOwinCapabilities((IDictionary<string, object>)app.Properties[OwinKeys.ServerCapabilitiesKey]);

            using (var server = serverBuilder.Build()) {

                var serverRef = new WeakReference<INowinServer>(server);

                Task.Run(() => {
                    INowinServer nowinServer;
                    if (serverRef.TryGetTarget(out nowinServer)) {
                        nowinServer.Start();
                    }
                });

                var baseAddress = "http://" + ip + ":" + port + "/";
                Console.WriteLine("Nowin server listening {0}, press ENTER to exit.", baseAddress);

                Console.ReadLine();
            }
        }

        public static void Configure(IAppBuilder app) {
            app.UseWindsorContainer("windsor.config");
            app.UseWindsorMiddleWare();

            var container = app.GetWindsorContainer();
            container.Register(
                Component.For<IAuthenticationManager>()
                         .FromOwinContext()
                         .LifestyleTransient()
            );

            var loggerFactory = container.Resolve<ILoggerFactory>();
            app.SetLoggerFactory(loggerFactory);

            var options = container.Resolve<StaticFileMiddlewareOptions>();
            app.UseStaticFile(options);

            app.UseAesDataProtectionProvider();

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                
            });

            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.UseWindsorContainer(container);

            app.UseWebApi(config);
        }
    }
}
