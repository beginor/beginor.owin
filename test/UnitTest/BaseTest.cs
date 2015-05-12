using Microsoft.Owin.Logging;
using NUnit.Framework;

namespace LogTestApp {

    public class OwinLoggingTest : WindsorTest {

        [Test]
        public void TestResolveLoggerFactory() {
            var loggerFactory = Container.Resolve<ILoggerFactory>();
            Assert.IsNotNull(loggerFactory);
            var logger = loggerFactory.Create(GetType().ToString());
            Assert.IsNotNull(logger);
        }

    }
}
