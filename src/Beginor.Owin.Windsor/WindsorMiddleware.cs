using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Beginor.Owin.Windsor {

    public class WindsorMiddleware : OwinMiddleware {

        public WindsorMiddleware(OwinMiddleware next) : base(next) {
        }

        public async override Task Invoke(IOwinContext context) {
            CallContext.LogicalSetData("owinContext", context);
            await Next.Invoke(context);
            CallContext.FreeNamedDataSlot("owinContext");
        }
    }

    public static class ComponentRegistrationExtensions {
        
        public static ComponentRegistration<TService> FromOwinContext<TService>(this ComponentRegistration<TService> registration)
            where TService : class {
            return registration.UsingFactoryMethod(
                factoryMethod: (kernel, model, creationContext) => {
                    IOwinContext owinContext = (IOwinContext) CallContext.LogicalGetData("owinContext");
                    if (owinContext == null) {
                        throw new InvalidOperationException("OwinContext is null!");
                    }
                    if (creationContext.RequestedType == typeof(IAuthenticationManager)) {
                        return (TService)owinContext.Authentication;
                    }
                    throw new NotSupportedException();
                },
                managedExternally: true
            );
        }
    }
}
