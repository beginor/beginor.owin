using System;
using Owin;
using Microsoft.Owin.Security.DataProtection;

namespace Beginor.Owin.Security.Aes {

    public static class AppBuilderExtensions {

        public static void UseAesDataProtectionProvider(this IAppBuilder app) {
            UseAesDataProtectionProvider(app, null);
        }

        public static void UseAesDataProtectionProvider(this IAppBuilder app, string dataProtectionKey) {
            const string hostAppNameKey = "host.AppName";
            var appName = "OwinNonameApp";
            if (app.Properties.ContainsKey(hostAppNameKey)) {
                appName = app.Properties[hostAppNameKey].ToString();
            }
            var dataProtectionProvider = new AesDataProtectionProvider(appName) {
                Key = dataProtectionKey
            };
            app.SetDataProtectionProvider(dataProtectionProvider);
        }

    }
}

