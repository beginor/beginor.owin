using System;
using Owin;
using Microsoft.Owin.Builder;
using Nowin;
using Beginor.Owin.StaticFile;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
using Beginor.Owin.Security.Aes;

namespace OwinSecurityTest {

    class Program {

        private static void Configure(IAppBuilder app) {
            app.Properties["host.AppName"] = "OwinSecurityTest";
            app.Properties["security.DataProtectionProvider"] = new AesDataProtectionProvider("OwinSecurityTest");
            // static file
            app.UseStaticFile(new StaticFileMiddlewareOptions {
                RootDirectory = @"../Website",
                DefaultFile = "index.html",
                MimeTypeProvider = new MimeTypeProvider(),
                EnableETag = true,
                ETagProvider = new LastWriteTimeETagProvider()
            });

            // cookie auth;
            app.UseCookieAuthentication(new CookieAuthenticationOptions{
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                //TicketDataFormat = new TicketDataFormat(new AesDataProtector("MySecurityAesKey"))
            });
            // web-api
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }

        static void Main(string[] args) {
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

    }
}
