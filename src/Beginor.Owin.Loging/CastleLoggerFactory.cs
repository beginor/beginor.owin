using Castle.Core.Logging;

namespace Beginor.Owin.Logging {

    public class CastleLoggerFactory : Microsoft.Owin.Logging.ILoggerFactory {

        private ILoggerFactory loggerFactory;

        public CastleLoggerFactory(ILoggerFactory loggerFactory) {
            this.loggerFactory = loggerFactory;
        }

        public Microsoft.Owin.Logging.ILogger Create(string name) {
            var logger = loggerFactory.Create(name);
            return new CastleLogger(logger);
        }
    }
}
