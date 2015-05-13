using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace Beginor.Owin.WebApi.Windsor {

    public class WindsorDependencyResolver : WindsorDependencyScope, IDependencyResolver {

        public WindsorDependencyResolver(IWindsorContainer container) : base(container) { }

        public IDependencyScope BeginScope() {
            return new WindsorDependencyScope(Container);
        }
    }

}