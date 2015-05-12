using Castle.Windsor;
using Castle.Windsor.Installer;
using NUnit.Framework;

namespace LogTestApp {

    //[TestFixture]
    public abstract class WindsorTest {

        private IWindsorContainer container;

        protected IWindsorContainer Container {
            get { return container; }
        }

        [SetUp]
        public virtual void SetUp() {
            container = new WindsorContainer();
            container.Install(
                Configuration.FromXmlFile("windsor.config")
            );
        }

        [Test]
        public void TestConfig() {
            Assert.IsNotNull(container);
        }
    }

    public abstract class WindsorTest<TTarget> : WindsorTest {

        public TTarget Target { get; set; }

        public override void SetUp() {
            base.SetUp();
            Target = Container.Resolve<TTarget>();
        }

    }

}