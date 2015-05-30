using System;
using System.Diagnostics;
using Castle.Core.Logging;

namespace Beginor.Owin.Logging {

    public class CastleLogger : Microsoft.Owin.Logging.ILogger {

        private readonly ILogger logger;

        public CastleLogger(ILogger logger) {
            this.logger = logger;
        }

        public bool WriteCore(TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter) {
            if (!ShouldWrite(eventType)) {
                return false;
            }
            var writer = GetLogWriter(eventType);
            var message = string.Format("eventId: {0}, message: {1}", eventId, formatter(state, exception));
            writer(message);
            return true;
        }

        private Action<string> GetLogWriter(TraceEventType eventType) {
            Action<string> writer;
            switch (eventType) {
                case TraceEventType.Critical:
                    writer = logger.Fatal;
                    break;
                case TraceEventType.Error:
                    writer = logger.Error;
                    break;
                case TraceEventType.Warning:
                    writer = logger.Warn;
                    break;
                case TraceEventType.Information:
                    writer = logger.Info;
                    break;
                default:
                    writer = logger.Debug;
                    break;
            }
            return writer;
        }

        private bool ShouldWrite(TraceEventType eventType) {
            var shouldWrite = false;
            switch (eventType) {
                case TraceEventType.Critical:
                    shouldWrite = logger.IsFatalEnabled;
                    break;
                case TraceEventType.Error:
                    shouldWrite = logger.IsErrorEnabled;
                    break;
                case TraceEventType.Warning:
                    shouldWrite = logger.IsWarnEnabled;
                    break;
                case TraceEventType.Information:
                    shouldWrite = logger.IsInfoEnabled;
                    break;
                default:
                    shouldWrite = logger.IsDebugEnabled;
                    break;
            }
            return shouldWrite;
        }
    }
}
