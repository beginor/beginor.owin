using System.Web.Http.ExceptionHandling;
using Castle.Windsor;
using Microsoft.Owin.Logging;
using NUnit.Framework;
using Beginor.Owin.StaticFile;
using TestWithNowin.Controllers;

namespace UnitTest {

    public class ConfigTest : WindsorTest {

        [Test]
        public void TestResolveLoggerFactory() {
            var loggerFactory = Container.Resolve<ILoggerFactory>();
            Assert.IsNotNull(loggerFactory);
            var logger = loggerFactory.Create(GetType().ToString());
            Assert.IsNotNull(logger);
        }

        [Test]
        public void CanResolveContainer() {
            var container = Container.Resolve<IWindsorContainer>();
            Assert.IsNotNull(container);
        }

        [Test]
        public void CanResolveStaticFileMiddlewareOptions() {
            var options = Container.Resolve<StaticFileMiddlewareOptions>();
            Assert.IsNotNull(options);
            Assert.AreEqual("index.html", options.DefaultFile);
            Assert.IsTrue(options.EnableETag);
        }

        [Test]
        public void CanResolveSampleController() {
            var controller = Container.Resolve<SampleController>();
            Assert.IsNotNull(controller);
        }

        [Test]
        public void CanResolveExceptionLogger() {
            var exLogger = Container.Resolve<IExceptionLogger>();
            Assert.IsNotNull(exLogger);
        }
    }
}
