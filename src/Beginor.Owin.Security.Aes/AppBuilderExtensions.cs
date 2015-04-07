using System;
using Owin;
using Microsoft.Owin.Security.DataProtection;

namespace Beginor.Owin.Security.Aes {
    
    public static class AppBuilderExtensions {

        public static void UseAesDataProtectionProvider(this IAppBuilder app) {
            const string hostAppNameKey = "host.AppName";
            if (app.Properties.ContainsKey(hostAppNameKey)) {
                var appName = app.Properties[hostAppNameKey].ToString();
                app.SetDataProtectionProvider(new AesDataProtectionProvider(appName));
            }
            else {
                app.SetDataProtectionProvider(new AesDataProtectionProvider());
            }
        }

    }
}

