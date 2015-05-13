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
            Logger.ErrorFormat(context.Exception, "Exception caught processing request {0}", context.Request.RequestUri);
        }

    }

}