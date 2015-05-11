using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.Owin.Logging;

namespace LogTestApp {

    [TestFixture]
    public class LoggerTest {

        private IWindsorContainer container;

        [SetUp]
        public void SetUp() {
            container = new WindsorContainer();
            container.Install(
                Configuration.FromXmlFile("windsor.config")
            );
        }

        [Test]
        public void TestConfig() {
            Assert.IsNotNull(container);
        }

        [Test]
        public void TestResolveLoggerFactory() {
            var loggerFactory = container.Resolve<ILoggerFactory>();
            Assert.IsNotNull(loggerFactory);
            var logger = loggerFactory.Create(GetType().ToString());
            Assert.IsNotNull(logger);
        }
    }
}
