using Microsoft.Owin;
using Microsoft.Owin.Security;
using Posy.V2.Infra.CrossCutting.Identity.Configuration;
using Posy.V2.Infra.CrossCutting.IoC;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Http;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Posy.V2.Api.App_Start.SimpleInjectorInitializer), "Initialize")]
namespace Posy.V2.Api.App_Start
{
    public static class SimpleInjectorInitializer
    {
        public static Container container = new Container();

        public static void Initialize()
        {
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            var hybrid = Lifestyle.CreateHybrid(
                defaultLifestyle: new AsyncScopedLifestyle(),
                fallbackLifestyle: new WebRequestLifestyle());

            // Chamada dos módulos do Simple Injector
            InitializeContainer(container, hybrid);

            // Necessário para registrar o ambiente do Owin que é dependência do Identity
            // Feito fora da camada de IoC para não levar o System.Web para fora
            container.Register<IAuthenticationManager>(() =>
                    AdvancedExtensions.IsVerifying(container)
                        ? new OwinContext(new Dictionary<string, object>()).Authentication
                        : HttpContext.Current.GetOwinContext().Authentication,
                        hybrid);

            //container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            //DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void InitializeContainer(Container container, ScopedLifestyle hybrid)
        {
            BootStrapper.RegisterServices(container, hybrid);
            container.Register<ISecureDataFormat<AuthenticationTicket>>(() => new FakeTicket(), hybrid);
            //container.Register<IChatService, ChatService>(hybrid);
            //container.Register<Controllers.Base.IGlobalBaseController, Controllers.Base.GlobalBaseController>(hybrid);

            //var activator = new SimpleInjectorHubActivator(container);
            //GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => activator);
        }
    }
}