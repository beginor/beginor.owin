﻿using System;
using Owin;
using Microsoft.Owin.Builder;
using Nowin;
using Beginor.Owin.StaticFile;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestWithNowin {

    class MainClass {

        public static void Main(string[] args) {
            // create a new AppBuilder
            IAppBuilder app = new AppBuilder();
            // init nowin's owin server factory.
            OwinServerFactory.Initialize(app.Properties);

            app.UseStaticFile(new StaticFileMiddlewareOptions {
                RootDirectory = @"C:\inetpub\wwwroot",
                DefaultFile = "iisstart.htm",
                EnableETag = true,
                MimeTypeProvider = new MimeTypeProvider()
            });

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
