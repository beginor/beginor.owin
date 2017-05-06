using Castle.Core.Logging;
using System.Web.Http.ExceptionHandling;

namespace Beginor.Owin.WebApi.Windsor {

    public class CastleExceptionLogger : ExceptionLogger {

        private ILogger logger = NullLogger.Instance;

        public ILogger Logger {
            get { return logger; }
            set { logger = value; }
        }

        public override void Log(ExceptionLoggerContext context) {
            var ex = context.Exception;
            var request = context.Request;
            var message = $"Exception caught processing request {request.RequestUri}";
            Logger.Error(message, context.Exception);
        }

    }

}