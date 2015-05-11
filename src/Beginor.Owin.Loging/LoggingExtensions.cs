using Microsoft.Owin.Logging;
using Owin;

namespace Beginor.Owin.Logging {

    public static class LoggingExtensions {

        public static void UseCastleLogging(this IAppBuilder app) {
            //app.SetLoggerFactory();
        }
    }
}
