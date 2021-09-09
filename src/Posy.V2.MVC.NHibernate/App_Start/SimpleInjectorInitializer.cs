using Microsoft.Owin;
using Microsoft.Owin.Security;
using Posy.V2.Infra.CrossCutting.IoC;
using Posy.V2.MVC.Hub;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Posy.V2.MVC.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace Posy.V2.MVC.App_Start
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

            /**
            * Chamada dos módulos do Simple Injector
            **/
            InitializeContainer(container, hybrid);

            /**
            * Necessário para registrar o ambiente do Owin que é dependência do Identity
            * Feito fora da camada de IoC para não levar o System.Web para fora
            **/
            container.Register<IAuthenticationManager>(() =>
                    AdvancedExtensions.IsVerifying(container)
                        ? new OwinContext(new Dictionary<string, object>()).Authentication
                        : HttpContext.Current.GetOwinContext().Authentication,
                        hybrid);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container, ScopedLifestyle hybrid)
        {
            BootStrapper.RegisterServices(container, hybrid);
            container.Register<IChatService, ChatService>(hybrid);
            container.Register<Controllers.Base.IGlobalBaseController, Controllers.Base.GlobalBaseController>(hybrid);
        }
    }
}