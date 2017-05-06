﻿using System.Web.Http.ExceptionHandling;
using Castle.Windsor;
using Microsoft.Owin.Logging;
using NUnit.Framework;
using Beginor.Owin.StaticFile;
using TestWithNowin.Controllers;
using System.Collections.Generic;
using System;
using System.Web.Http.Metadata;
using System.Web.Http.Metadata.Providers;
using Castle.MicroKernel.Registration;

namespace UnitTest {

    [TestFixture]
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

        [Test]
        public void CanChange() {
            IEnumerable<object> o = new List<string>();
            Action<string> a = new Action<object>(arg => { 
            });
        }

        [Test]
        public void CanResolveAllModelMetadataProvider() {
            Container.Register(
                Component.For<ModelMetadataProvider>()
                    .ImplementedBy<DataAnnotationsModelMetadataProvider>(),
                Component.For<ModelMetadataProvider>()
                    .ImplementedBy<EmptyModelMetadataProvider>()
                    .Named("EmptyModelMetadataProvider"),
                Component.For<ModelMetadataProvider>()
                    .ImplementedBy<DataAnnotationsModelMetadataProvider>()
                    .Named("DataAnnotationsModelMetadataProvider")
            );
            var providers = Container.ResolveAll<ModelMetadataProvider>();
            Assert.AreEqual(2, providers.Length);
        }
    }
}
