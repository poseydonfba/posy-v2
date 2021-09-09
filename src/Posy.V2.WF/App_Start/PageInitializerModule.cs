using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

// Use the SimpleInjector.Integration.Web Nuget package
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Diagnostics;
using SimpleInjector.Integration.Web;

// Makes use of the System.ComponentModel.Composition assembly
using System.ComponentModel.Composition;
using Posy.V2.Infra.CrossCutting.IoC;
using SimpleInjector.Lifestyles;

[assembly: PreApplicationStartMethod(
    typeof(Posy.V2.WF.App_Start.PageInitializerModule),
    "Initialize")]

namespace Posy.V2.WF.App_Start
{
    public sealed class PageInitializerModule : IHttpModule
    {
        public static void Initialize()
        {
            DynamicModuleUtility.RegisterModule(typeof(PageInitializerModule));
        }

        void IHttpModule.Init(HttpApplication app)
        {
            app.PreRequestHandlerExecute += (sender, e) =>
            {
                var handler = app.Context.CurrentHandler;
                if (handler != null)
                {
                    string name = handler.GetType().Assembly.FullName;
                    if (!name.StartsWith("System.Web") &&
                        !name.StartsWith("Microsoft"))
                    {
                        Global.InitializeHandler(handler);
                    }
                }
            };

            // TENTATIVA FALHA DE APLICAR HANDLER PARA WEBSERVICES
            //app.PostResolveRequestCache += (sender, e) =>
            //{
            //    //HttpApplication application = (HttpApplication)sender;
            //    //HttpContext context = application.Context;

            //    var application = app;
            //    var context = app.Context;
            //    var handler = app.Context.CurrentHandler;

            //    if (context.Request.Headers["x-microsoftajax"] == null)
            //    {
            //        //if ((!System.IO.File.Exists(application.Request.PhysicalPath)) &&
            //        //  (!application.Request.Url.ToString().Contains(".axd")) &&
            //        //  (!application.Request.Url.ToString().Contains(".asmx")))
            //        if (application.Request.Url.ToString().Contains(".asmx"))
            //        {
            //            Global.InitializeHandler(handler);
            //            //string newUrl = "~/Search.aspx?q="
            //            //  + context.Server.UrlEncode(application.Request.Url.Segments.Last());
            //            ////...
            //            //context.RewritePath(newUrl);
            //            var webservice = 1;
            //        }
            //    }
            //};

            //app.PostResolveRequestCache +=
            //    (new EventHandler(this.Application_OnAfterProcess));
        }

        private void Application_OnAfterProcess(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            //var handler = app.Context.CurrentHandler;

            if (context.Request.Headers["x-microsoftajax"] == null)
            {
                //if ((!System.IO.File.Exists(application.Request.PhysicalPath)) &&
                //  (!application.Request.Url.ToString().Contains(".axd")) &&
                //  (!application.Request.Url.ToString().Contains(".asmx")))
                if (application.Request.Url.ToString().Contains(".asmx"))
                {
                    //string newUrl = "~/Search.aspx?q="
                    //  + context.Server.UrlEncode(application.Request.Url.Segments.Last());
                    ////...
                    //context.RewritePath(newUrl);
                    var webservice = 1;
                }
            }
        }

        void IHttpModule.Dispose() { }
    }

    //public class Global : HttpApplication
    //{
    //    private static Container container;

    //    public static void InitializeHandler(IHttpHandler handler)
    //    {
    //        container.GetRegistration(handler.GetType(), true).Registration
    //            .InitializeInstance(handler);
    //    }

    //    protected void Application_Start(object sender, EventArgs e)
    //    {
    //        Bootstrap();
    //    }

    //    private static void Bootstrap()
    //    {
    //        // 1. Create a new Simple Injector container.
    //        var container = new Container();

    //        container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

    //        // Register a custom PropertySelectionBehavior to enable property injection.
    //        container.Options.PropertySelectionBehavior =
    //            new ImportAttributePropertySelectionBehavior();

    //        // 2. Configure the container (register)
    //        var hybrid = Lifestyle.CreateHybrid(
    //            defaultLifestyle: new AsyncScopedLifestyle(),
    //            fallbackLifestyle: new WebRequestLifestyle());
    //        BootStrapper.RegisterServices(container, hybrid);
    //        //container.Register<IUserRepository, SqlUserRepository>();
    //        //container.Register<IUserContext, AspNetUserContext>(Lifestyle.Scoped);

    //        // Register your Page classes to allow them to be verified and diagnosed.
    //        RegisterWebPages(container);

    //        // 3. Store the container for use by Page classes.
    //        Global.container = container;

    //        // 3. Verify the container's configuration.
    //        container.Verify();
    //    }

    //    private static void RegisterWebPages(Container container)
    //    {
    //        var pageTypes =
    //            from assembly in BuildManager.GetReferencedAssemblies().Cast<Assembly>()
    //            where !assembly.IsDynamic
    //            where !assembly.GlobalAssemblyCache
    //            from type in assembly.GetExportedTypes()
    //            where type.IsSubclassOf(typeof(Page))
    //            where !type.IsAbstract && !type.IsGenericType
    //            select type;

    //        foreach (Type type in pageTypes)
    //        {
    //            var reg = Lifestyle.Transient.CreateRegistration(type, container);
    //            reg.SuppressDiagnosticWarning(
    //                DiagnosticType.DisposableTransientComponent,
    //                "ASP.NET creates and disposes page classes for us.");
    //            container.AddRegistration(type, reg);
    //        }
    //    }

    //    class ImportAttributePropertySelectionBehavior : IPropertySelectionBehavior
    //    {
    //        public bool SelectProperty(Type implementationType, PropertyInfo property)
    //        {
    //            // Makes use of the System.ComponentModel.Composition assembly
    //            return typeof(Page).IsAssignableFrom(implementationType) &&
    //                property.GetCustomAttributes(typeof(ImportAttribute), true).Any();
    //        }
    //    }
    //}
}