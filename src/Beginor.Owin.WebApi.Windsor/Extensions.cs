using System;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Metadata;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;
using System.Web.Http.ValueProviders;
using System.Web.Http.ValueProviders.Providers;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Beginor.Owin.WebApi.Windsor {

    public static class Extensions {

        public static void UseWindsorContainer(this HttpConfiguration config, IWindsorContainer container) {
            if (config == null) {
                throw new ArgumentNullException("config");
            }
            if (container == null) {
                throw new ArgumentNullException("container");
            }
            container.Register(
                Component.For<HttpConfiguration>()
                    .Instance(config)
                    .LifestyleSingleton(),
                Component.For<IDependencyResolver>()
                    .ImplementedBy<WindsorDependencyResolver>()
                    .LifestyleSingleton()
            );

            var resolver = container.Resolve<IDependencyResolver>();
            // check web api service registions, is custom impl is not registered, register default impl
            CheckWebApiServiceRegistions(container);
            config.DependencyResolver = resolver;
        }

        private static void RegisterSingleton<TService, TImpl>(
            IWindsorContainer container
        ) where TImpl : class, TService {
            if (!container.Kernel.HasComponent(typeof(TService))) {
                container.Register(
                    Component.For(typeof(TService))
                        .ImplementedBy(typeof(TImpl))
                        .LifestyleSingleton()
                );
            }
        }

        private static void RegisterSingletons<TService>(
            IWindsorContainer container,
            params Type[] types
        ) {
            foreach (var type in types) {
                if (!container.Kernel.HasComponent(type.Name)) {
                    container.Register(
                        Component.For(typeof(TService))
                             .ImplementedBy(type)
                             .Named(type.Name)
                             .LifestyleSingleton()
                    );
                }
            }
        }

        static void CheckWebApiServiceRegistions(IWindsorContainer container) {
            RegisterSingleton<IActionValueBinder, DefaultActionValueBinder>(container);
            RegisterSingleton<IApiExplorer, ApiExplorer>(container);
            RegisterSingleton<IAssembliesResolver, DefaultAssembliesResolver>(container);
            RegisterSingleton<IBodyModelValidator, DefaultBodyModelValidator>(container);
            RegisterSingleton<IContentNegotiator, DefaultContentNegotiator>(container);
            RegisterSingleton<IHttpActionInvoker, ApiControllerActionInvoker>(container);
            // IDocumentationProvider
            // IHostBufferPolicySelector
            RegisterSingleton<IHttpActionSelector, ApiControllerActionSelector>(container);
            RegisterSingleton<IHttpControllerActivator, DefaultHttpControllerActivator>(container);
            RegisterSingleton<IHttpControllerSelector, DefaultHttpControllerSelector>(container);
            RegisterSingleton<IHttpControllerTypeResolver, DefaultHttpControllerTypeResolver>(container);
            // ITraceManager
            // ITraceWriter
            RegisterSingleton<ModelMetadataProvider, DataAnnotationsModelMetadataProvider>(container);
            // IModelValidatorCache
            RegisterSingleton<IExceptionHandler, ExceptionHandler>(container);
            //
            RegisterSingletons<IExceptionLogger>(
                container,
                typeof(CastleExceptionLogger)
            );
            RegisterSingletons<IFilterProvider>(
                container,
                typeof(ConfigurationFilterProvider),
                typeof(ActionDescriptorFilterProvider)
            );
            RegisterSingletons<ModelBinderProvider>(
                container,
                typeof(TypeConverterModelBinderProvider),
                typeof(TypeMatchModelBinderProvider),
                typeof(KeyValuePairModelBinderProvider),
                typeof(ComplexModelDtoModelBinderProvider),
                typeof(ArrayModelBinderProvider),
                typeof(DictionaryModelBinderProvider),
                typeof(CollectionModelBinderProvider),
                typeof(MutableObjectModelBinderProvider)
            );
            RegisterSingletons<ModelValidatorProvider>(
                container,
                typeof(DataAnnotationsModelValidatorProvider),
                typeof(DataMemberModelValidatorProvider)
            );
            RegisterSingletons<ValueProviderFactory>(
                container,
                typeof(QueryStringValueProviderFactory),
                typeof(RouteDataValueProviderFactory)
            );
        }
    }

}