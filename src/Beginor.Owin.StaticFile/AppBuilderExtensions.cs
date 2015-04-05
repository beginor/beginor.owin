using System;
using Owin;

namespace Beginor.Owin.StaticFile {

    public static class AppBuilderExtensions {

        public static void UseStaticFile(this IAppBuilder app, StaticFileMiddlewareOptions options) {
            if (options == null) {
                throw new ArgumentNullException("options");
            }

            if (options.MimeTypeProvider == null) {
                options.MimeTypeProvider = new MimeTypeProvider();
            }
            if (options.EnableETag) {
                if (options.ETagProvider == null) {
                    options.ETagProvider = new LastWriteTimeETagProvider();
                }
                app.Use(typeof(ETagMiddleware), options);
            }
            app.Use(typeof(StaticFileMiddleware), options);
        }
    }
}

