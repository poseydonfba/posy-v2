using Microsoft.Owin;
using Microsoft.Owin.Security;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using Posy.V2.Infra.CrossCutting.IoC;
using Posy.V2.WF.Helpers;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Diagnostics;
using SimpleInjector.Integration.Web;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Services;
using System.Web.UI;

namespace Posy.V2.WF
{
    public class Global : HttpApplication
    {
        public static Container container;

        public static void InitializeHandler(IHttpHandler handler)
        {
            #region ORIGINAL

            //container.GetRegistration(handler.GetType(), true).Registration
            //    .InitializeInstance(handler);

            #endregion

            // PARA USAR EM USER CONTROLS
            // https://stackoverflow.com/questions/29341192/resolving-asp-net-web-forms-image-control-derived-class-registered-via-simple-in
            if (handler is Page)
                Global.InitializePage((Page)handler);

            if (handler is WebService)
                container.GetRegistration(handler.GetType(), true).Registration
                    .InitializeInstance(handler);
        }

        // https://stackoverflow.com/questions/29341192/resolving-asp-net-web-forms-image-control-derived-class-registered-via-simple-in
        private static void InitializePage(Page page)
        {
            container.GetRegistration(page.GetType(), true).Registration
                .InitializeInstance(page);

            page.InitComplete += delegate { Global.InitializeControl(page); };
        }

        // https://stackoverflow.com/questions/29341192/resolving-asp-net-web-forms-image-control-derived-class-registered-via-simple-in
        private static void InitializeControl(Control control)
        {
            if (control is UserControl)
            {
                container.GetRegistration(control.GetType(), true).Registration
                    .InitializeInstance(control);
            }
            foreach (Control child in control.Controls)
            {
                Global.InitializeControl(child);
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrap();

            Application.Add("UsuariosOnLine", Convert.ToInt32(0));
            Application.Add("CaptchaValue", string.Empty);
            Application.Add("perfilView", null);
            Application.Add("comunidadeView", null);
            Application.Add("PageNumber", Convert.ToInt32(1));
            Application.Add("TopicoNumber", Guid.Empty);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static void Bootstrap()
        {
            // 1. Create a new Simple Injector container.
            var container = new Container();

            var scope = new WebRequestLifestyle();
            container.Options.DefaultScopedLifestyle = scope;

            // Register a custom PropertySelectionBehavior to enable property injection.
            container.Options.PropertySelectionBehavior =
                new ImportAttributePropertySelectionBehavior();

            // 2. Configure the container (register)
            var hybrid = Lifestyle.CreateHybrid(
                defaultLifestyle: new AsyncScopedLifestyle(),
                fallbackLifestyle: new WebRequestLifestyle());

            BootStrapper.RegisterServices(container, Lifestyle.Scoped);

            container.Register(() =>
                container.IsVerifying
                    ? new OwinContext().Authentication
                    : HttpContext.Current.GetOwinContext().Authentication,
                    Lifestyle.Scoped);

            // Register your Page classes to allow them to be verified and diagnosed.
            RegisterWebPages(container);

            // 3. Store the container for use by Page classes.
            Global.container = container;

            // 3. Verify the container's configuration.
            container.Verify();
        }

        private static void RegisterWebPages(Container container)
        {
            #region ORIGINAL

            //    var pageTypes =
            //        from assembly in BuildManager.GetReferencedAssemblies().Cast<Assembly>()
            //        where !assembly.IsDynamic
            //        where !assembly.GlobalAssemblyCache
            //        from type in assembly.GetExportedTypes()
            //        where type.IsSubclassOf(typeof(Page))
            //        where !type.IsAbstract && !type.IsGenericType
            //        select type;

            //    foreach (Type type in pageTypes)
            //    {
            //        var reg = Lifestyle.Transient.CreateRegistration(type, container);
            //        reg.SuppressDiagnosticWarning(
            //            DiagnosticType.DisposableTransientComponent,
            //            "ASP.NET creates and disposes page classes for us.");
            //        container.AddRegistration(type, reg);
            //    }

            #endregion

            // https://stackoverflow.com/questions/29341192/resolving-asp-net-web-forms-image-control-derived-class-registered-via-simple-in
            var pageTypes =
                from assembly in BuildManager.GetReferencedAssemblies().Cast<Assembly>()
                where !assembly.IsDynamic
                where !assembly.GlobalAssemblyCache
                from type in assembly.GetExportedTypes()
                where type.IsSubclassOf(typeof(Page)) || type.IsSubclassOf(typeof(UserControl)) /*|| type.IsSubclassOf(typeof(WebService))*/
                where !type.IsAbstract && !type.IsGenericType
                select type;

            foreach (Type type in pageTypes)
            {
                var reg = Lifestyle.Transient.CreateRegistration(type, container);
                reg.SuppressDiagnosticWarning(
                    DiagnosticType.DisposableTransientComponent,
                    "ASP.NET creates and disposes page classes for us.");
                container.AddRegistration(type, reg);
            }
        }

        class ImportAttributePropertySelectionBehavior : IPropertySelectionBehavior
        {
            public bool SelectProperty(Type implementationType, PropertyInfo property)
            {
                #region ORIGINAL

                // Makes use of the System.ComponentModel.Composition assembly
                //return typeof(Page).IsAssignableFrom(implementationType) &&
                //    property.GetCustomAttributes(typeof(ImportAttribute), true).Any();

                #endregion

                // https://stackoverflow.com/questions/29341192/resolving-asp-net-web-forms-image-control-derived-class-registered-via-simple-in
                return (typeof(Page).IsAssignableFrom(implementationType) ||
                        typeof(UserControl).IsAssignableFrom(implementationType)
                        /*typeof(WebService).IsAssignableFrom(implementationType)*/) &&
                        property.GetCustomAttributes<ImportAttribute>().Any();
            }
        }

        //void Application_Start(object sender, EventArgs e)
        //{
        //    //// Code that runs on application startup
        //    //RouteConfig.RegisterRoutes(RouteTable.Routes);
        //    //BundleConfig.RegisterBundles(BundleTable.Bundles);

        //    #region CONTAINER DE INJEÇÂO DE DEPENDENCIA

        //    ////DependencyResolverWF.Resolve();

        //    //Container container = new Container();
        //    //container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

        //    //var hybrid = Lifestyle.CreateHybrid(
        //    //    defaultLifestyle: new AsyncScopedLifestyle(),
        //    //    fallbackLifestyle: new WebRequestLifestyle());

        //    //BootStrapper.RegisterServices(container, hybrid);

        //    //container.Verify();

        //    ////DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

        //    ////HttpConfiguration config = GlobalConfiguration.Configuration;

        //    //Global.container = container;

        //    #endregion

        //    //config.Formatters.JsonFormatter
        //    //    .SerializerSettings
        //    //    .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

        //    Application.Add("UsuariosOnLine", Convert.ToInt32(0));
        //    Application.Add("CaptchaValue", string.Empty);
        //    Application.Add("perfilView", null);
        //    Application.Add("comunidadeView", null);
        //    Application.Add("PageNumber", Convert.ToInt32(1));
        //    Application.Add("TopicoNumber", Guid.Empty);

        //    // Quando o usuario mudar de perfil nas rotas, excluir cache pra aparecer o perfil view
        //    //Application.Add("changeMethodCache", false);

        //    //Context.Items.Remove("AfterPageUnloaded");


        //    // Configure SignalR
        //    //RouteTable.Routes.MapHubs("~/signalr");
        //    //RouteTable.Routes.MapHubs();

        //    RegisterRoutes(RouteTable.Routes);
        //    BundleConfig.RegisterBundles(BundleTable.Bundles);
        //}

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Evita após logout o usuario voltar a página pelo botão do navegador
            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();

            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(5));
            //Response.Cache.SetLastModified(DateTime.UtcNow.AddMinutes(5));
            //Response.Cache.SetCacheability(HttpCacheability.Public);
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            ValidaAutenticacao(sender);
        }

        protected void Application_PostAuthorizeRequest()
        {
            //Default: Esta é a configuração padrão, o que significa que tudo funciona como antes.
            //Disabled: Turned de Sessão Sate para solicitação atual.
            //ReadOnly: Leia único acesso ao estado da sessão.
            //Required: Ativado estado da sessão para leitura e gravação acesso.

            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        //protected void Session_Start(object sender, EventArgs e)
        //{
        //    lock (typeof(HttpApplication))
        //    {
        //        int usuariosOnline = Convert.ToInt32(Application.Get("UsuariosOnLine"));
        //        usuariosOnline++;
        //        Application.Set("UsuariosOnLine", usuariosOnline);
        //    }
        //    Session.Timeout = 20; // A sessão expira em 43200 minutos, 30 dias

        //    //HttpContext context = HttpContext.Current;
        //    //Visit visit = new Visit();
        //    //visit.UserAgent = context.Request.UserAgent;
        //    //visit.IpAddress = context.Request.UserHostAddress;
        //    //context.Session.Add("visit", visit);
        //}

        //protected void Session_End(object sender, EventArgs e)
        //{
        //    lock (typeof(HttpApplication))
        //    {
        //        int usuariosOnline = Convert.ToInt32(Application.Get("UsuariosOnLine"));
        //        usuariosOnline--;
        //        Application.Set("UsuariosOnLine", usuariosOnline);
        //    }
        //}

        protected void Application_End(object sender, EventArgs e)
        {

        }

        protected void ValidaAutenticacao(object sender)
        {
            HttpContext context = HttpContext.Current;

            RedirectReturnUrl(context);

            PageBase(sender);
        }

        protected void PageBase(object sender)
        {
            // Verificação dos valores das rotas para as paginas

            //var httpContext = new HttpContextWrapper(context);
            //var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(context));

            RequestContext requestContext = Context.Request.RequestContext; // new RequestContext(httpContext, routeData);

            HttpApplication app = (HttpApplication)sender;

            HttpContext context = HttpContext.Current;

            //if (requestContext.RouteData.Route != null && requestContext.RouteData.Values.Count > 0)
            if (app.Context.CurrentHandler is Page)
            {
                string urlRoute = context.Handler.ToString(); //ASP.p_perfil_aspx
                urlRoute = urlRoute.Replace("ASP.", string.Empty).Replace("_aspx", string.Empty);

                string[] route = urlRoute.Split('_');
                urlRoute = (route.Length > 1) ? route[1] : route[0];
                string urlRoutePrefix = (route.Length > 0) ? route[0] : "";

                if (!urlRoute.Equals(FuncaoSite.ROUTE_URL_INICIO) &&
                    !urlRoute.Equals(FuncaoSite.ROUTE_URL_EDITARPERFIL) &&
                    !urlRoute.Equals(FuncaoSite.ROUTE_URL_PESQUISARCMM) &&
                    !urlRoute.Equals(FuncaoSite.ROUTE_URL_CADCMM))
                {
                    if (urlRoutePrefix.Equals("p"))
                    {
                        var alias = requestContext.RouteData.Values["p"] as string;
                        Perfil perfil;

                        if (alias.ToLower() == "perfil") return;

                        var _cacheService = Global.container.GetInstance<ICacheService>();
                        //if (CacheProvider.Get(alias) == null)
                        if (_cacheService.Get<Perfil>(alias) == null)
                        {
                            //var container = HttpContext.Current.Application.GetContainer();
                            //var _perfilServico = container.Resolve<IPerfilServico>();
                            var _perfilService = Global.container.GetInstance<IPerfilService>();

                            #region VERIFICAÇÃO DE CACHE COM CABEÇALHOS DE SOLICITAÇÃO, NAO FUNCIONOU

                            //var perfilAanterior = (Perfil)Application.Get("perfilView");

                            //if (perfilAanterior != null && !perfilAanterior.Alias.Equals(alias))
                            //{
                            //    Application.Set("changeMethodCache", true);

                            //    //Response.Cache.SetNoStore();
                            //    //Response.Cache.AppendCacheExtension("no-cache");
                            //    //Response.Expires = 0;

                            //    //HttpResponse.RemoveOutputCacheItem(Funcao.ResolveServerUrl("~/" + alias, false));
                            //}
                            //else
                            //    Application.Set("changeMethodCache", false);

                            #endregion

                            perfil = _perfilService.Obter(alias);

                            //CacheProvider.Add(alias, perfil, TypeExpirationFlag.AbsoluteExpiration);
                            if (perfil != null)
                                _cacheService.Set(alias, perfil);
                        }
                        else
                        {
                            //perfil = (Perfil)CacheProvider.Get(alias);
                            perfil = (Perfil)_cacheService.Get<Perfil>(alias);
                        }

                        perfil.Foto = Funcao.ResolveServerUrl("~/img/perfil/" + perfil.UsuarioId.ToString() + "/1.jpg", false);

                        #region ROTAS PERFIL

                        if (perfil == null)
                            Response.Redirect(Funcao.ResolveServerUrl("~/" + FuncaoSite.ROUTE_URL_INICIO, false), true);

                        if (urlRoute.Equals(FuncaoSite.ROUTE_URL_AMIGOS) || urlRoute.Equals(FuncaoSite.ROUTE_URL_DEPOIMENTOS) ||
                            urlRoute.Equals(FuncaoSite.ROUTE_URL_RECADOS) || urlRoute.Equals(FuncaoSite.ROUTE_URL_AMIGOSPEND) ||
                            urlRoute.Equals(FuncaoSite.ROUTE_URL_COMUNIDADES) || urlRoute.Equals(FuncaoSite.ROUTE_URL_VIDEOS))
                        {
                            string page = requestContext.RouteData.Values["pg"] as string;

                            if (!Funcao.IsNumeric(page))
                                Response.Redirect(Funcao.ResolveServerUrl("~/" + alias + "/" + urlRoute, false) + "/1", true);

                            Application.Set("PageNumber", page);
                        }

                        #endregion

                        var perfilView = FuncaoJson.CopyObj(perfil) as Perfil;
                        Application.Set("perfilView", perfilView);
                    }
                    else if (urlRoutePrefix.Equals("c"))
                    {
                        var alias = requestContext.RouteData.Values["c"] as string;

                        //var container = HttpContext.Current.Application.GetContainer();
                        //var container = DependencyResolver.Current.GetService<ApplicationUserManager>();
                        //var _comunidadeServico = container.Resolve<IComunidadeService>();
                        var _comunidadeService = Global.container.GetInstance<IComunidadeService>();

                        var comunidade = _comunidadeService.Obter(alias);

                        #region ROTAS CMM

                        if (comunidade == null)
                            Response.Redirect(Funcao.ResolveServerUrl("~/" + FuncaoSite.ROUTE_URL_INICIO, false), true);

                        if (urlRoute.Equals(FuncaoSite.ROUTE_URL_TOPICO))
                        {
                            string page = requestContext.RouteData.Values["pg"] as string;
                            string tpc = requestContext.RouteData.Values["t"] as string;

                            Guid topico;
                            bool isTpcValid = Guid.TryParse(tpc, out topico);

                            if (!isTpcValid)
                                Response.Redirect(Funcao.ResolveServerUrl("~/" + FuncaoSite.ROUTE_URL_PREFIXCMM + "/" + alias + "/" + urlRoute, false) + "/1", true);

                            if (!Funcao.IsNumeric(page))
                                Response.Redirect(Funcao.ResolveServerUrl("~/" + FuncaoSite.ROUTE_URL_PREFIXCMM + "/" + alias + "/" + urlRoute, false) + "/" + topico + "/1", true);

                            Application.Set("PageNumber", page);
                            Application.Set("TopicoNumber", topico);
                        }
                        else if (urlRoute.Equals(FuncaoSite.ROUTE_URL_FORUM) || urlRoute.Equals(FuncaoSite.ROUTE_URL_MEMBROS) ||
                                 urlRoute.Equals(FuncaoSite.ROUTE_URL_MODERADORES) || urlRoute.Equals(FuncaoSite.ROUTE_URL_MEMBROSPENDENTE))
                        {
                            string page = requestContext.RouteData.Values["pg"] as string;
                            string tpc = requestContext.RouteData.Values["t"] as string;

                            Guid topico;
                            bool isTpcValid = Guid.TryParse(tpc, out topico);

                            if (!Funcao.IsNumeric(page))
                                Response.Redirect(Funcao.ResolveServerUrl("~/" + FuncaoSite.ROUTE_URL_PREFIXCMM + "/" + alias + "/" + urlRoute, false) + "/1", true);

                            Application.Set("PageNumber", page);
                            Application.Set("TopicoNumber", topico);
                        }

                        #endregion

                        comunidade.Foto = Funcao.ResolveServerUrl("~/img/cmm/" + comunidade.ComunidadeId.ToString() + "/1.jpg", false);

                        var comunidadeClone = FuncaoJson.CopyObj(comunidade) as Comunidade;
                        Application.Set("comunidadeView", comunidadeClone);
                    }
                }
                else
                {
                    Application.Set("perfilView", null);
                    Application.Set("comunidadeView", null);
                    Application.Set("PageNumber", Convert.ToInt32(1));
                    Application.Set("TopicoNumber", Guid.Empty);
                }
            }
        }

        protected void RedirectReturnUrl(HttpContext context)
        {
            // Se contem ReturnUrl nao esta autenticado

            if (context.Request.Url.OriginalString.Contains("ReturnUrl"))
            {
                string path = Funcao.ResolveServerUrl("~/", false);

                Response.Redirect(path, true);
                Response.End();

                return;
            }
        }

        protected void RegisterRoutes(RouteCollection routes)
        {
            // Code that runs on application startup
            //routes.MapPageRoute("", "inicio", "~/inicio.aspx");          // Quando chamar http://<site>/inicio vai para inicio.aspx
            //routes.MapPageRoute("Default", "{Home}", "~/{Home}.aspx");   // Quando chamar http://<site>/test vai para test.aspx

            routes.Ignore("{*alljs}", new { alljs = @".*\.js(/.*)?" });
            routes.Ignore("{*allpng}", new { allpng = @".*\.png(/.*)?" });
            routes.Ignore("{*allcss}", new { allcss = @".*\.css(/.*)?" });

            routes.MapPageRoute("", "", "~/" + FuncaoSite.ROUTE_FILE_DEFAULT);
            //routes.MapPageRoute("", "", "~/" + FuncaoSite.ROUTE_FILE_INICIO);            

            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_ERRO, "~/" + FuncaoSite.ROUTE_FILE_ERRO);
            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_INICIO, "~/" + FuncaoSite.ROUTE_FILE_INICIO);
            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PESQUISARCMM, "~/" + FuncaoSite.ROUTE_FILE_PESQUISARCMM);
            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_CADCMM, "~/" + FuncaoSite.ROUTE_FILE_CADCMM);
            //routes.MapPageRoute("", FuncaoSite.ROUTE_URL_CADERFIL, "~/" + FuncaoSite.ROUTE_FILE_CADERFIL);
            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_EDITARPERFIL, "~/" + FuncaoSite.ROUTE_FILE_EDITARPERFIL);
            //routes.MapPageRoute("", FuncaoSite.ROUTE_URL_LOGIN, "~/" + FuncaoSite.ROUTE_FILE_LOGIN);
            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_LOGOUT, "~/" + FuncaoSite.ROUTE_FILE_LOGOUT);
            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_CAPTCHA, "~/" + FuncaoSite.ROUTE_FILE_CAPTCHA);

            routes.MapPageRoute("", "{p}", "~/" + FuncaoSite.ROUTE_FILE_PERFIL);
            routes.MapPageRoute("", "{p}/", "~/" + FuncaoSite.ROUTE_FILE_PERFIL);

            routes.MapPageRoute("", "{p}/" + FuncaoSite.ROUTE_URL_AMIGOS + "", "~/" + FuncaoSite.ROUTE_FILE_AMIGOS);
            routes.MapPageRoute("", "{p}/" + FuncaoSite.ROUTE_URL_AMIGOS + "/{pg}", "~/" + FuncaoSite.ROUTE_FILE_AMIGOS);

            routes.MapPageRoute("", "{p}/" + FuncaoSite.ROUTE_URL_AMIGOSPEND + "", "~/" + FuncaoSite.ROUTE_FILE_AMIGOSPEND);
            routes.MapPageRoute("", "{p}/" + FuncaoSite.ROUTE_URL_AMIGOSPEND + "/{pg}", "~/" + FuncaoSite.ROUTE_FILE_AMIGOSPEND);

            routes.MapPageRoute("", "{p}/" + FuncaoSite.ROUTE_URL_RECADOS + "", "~/" + FuncaoSite.ROUTE_FILE_RECADOS);
            routes.MapPageRoute("", "{p}/" + FuncaoSite.ROUTE_URL_RECADOS + "/{pg}", "~/" + FuncaoSite.ROUTE_FILE_RECADOS);

            routes.MapPageRoute("", "{p}/" + FuncaoSite.ROUTE_URL_VIDEOS + "", "~/" + FuncaoSite.ROUTE_FILE_VIDEOS);
            routes.MapPageRoute("", "{p}/" + FuncaoSite.ROUTE_URL_VIDEOS + "/{pg}", "~/" + FuncaoSite.ROUTE_FILE_VIDEOS);

            routes.MapPageRoute("", "{p}/" + FuncaoSite.ROUTE_URL_DEPOIMENTOS + "", "~/" + FuncaoSite.ROUTE_FILE_DEPOIMENTOS);
            routes.MapPageRoute("", "{p}/" + FuncaoSite.ROUTE_URL_DEPOIMENTOS + "/{pg}", "~/" + FuncaoSite.ROUTE_FILE_DEPOIMENTOS);

            routes.MapPageRoute("", "{p}/" + FuncaoSite.ROUTE_URL_COMUNIDADES + "", "~/" + FuncaoSite.ROUTE_FILE_COMUNIDADES);
            routes.MapPageRoute("", "{p}/" + FuncaoSite.ROUTE_URL_COMUNIDADES + "/{pg}", "~/" + FuncaoSite.ROUTE_FILE_COMUNIDADES);



            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PREFIXCMM + "/{c}", "~/" + FuncaoSite.ROUTE_FILE_CMM);
            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PREFIXCMM + "/{c}/" + FuncaoSite.ROUTE_URL_EDITCMM, "~/" + FuncaoSite.ROUTE_FILE_EDITCMM);

            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PREFIXCMM + "/{c}/" + FuncaoSite.ROUTE_URL_MEMBROSPENDENTE, "~/" + FuncaoSite.ROUTE_FILE_MEMBROSPENDENTE);
            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PREFIXCMM + "/{c}/" + FuncaoSite.ROUTE_URL_MEMBROSPENDENTE + "/{pg}", "~/" + FuncaoSite.ROUTE_FILE_MEMBROSPENDENTE);

            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PREFIXCMM + "/{c}/" + FuncaoSite.ROUTE_URL_MEMBROS, "~/" + FuncaoSite.ROUTE_FILE_MEMBROS);
            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PREFIXCMM + "/{c}/" + FuncaoSite.ROUTE_URL_MEMBROS + "/{pg}", "~/" + FuncaoSite.ROUTE_FILE_MEMBROS);

            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PREFIXCMM + "/{c}/" + FuncaoSite.ROUTE_URL_MODERADORES, "~/" + FuncaoSite.ROUTE_FILE_MODERADORES);
            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PREFIXCMM + "/{c}/" + FuncaoSite.ROUTE_URL_MODERADORES + "/{pg}", "~/" + FuncaoSite.ROUTE_FILE_MODERADORES);

            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PREFIXCMM + "/{c}/" + FuncaoSite.ROUTE_URL_FORUM, "~/" + FuncaoSite.ROUTE_FILE_FORUM);
            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PREFIXCMM + "/{c}/" + FuncaoSite.ROUTE_URL_FORUM + "/{pg}", "~/" + FuncaoSite.ROUTE_FILE_FORUM);

            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PREFIXCMM + "/{c}/" + FuncaoSite.ROUTE_URL_TOPICO + "/{t}", "~/" + FuncaoSite.ROUTE_FILE_TOPICO);
            routes.MapPageRoute("", FuncaoSite.ROUTE_URL_PREFIXCMM + "/{c}/" + FuncaoSite.ROUTE_URL_TOPICO + "/{t}/{pg}", "~/" + FuncaoSite.ROUTE_FILE_TOPICO);

        }

    }
}

/*     

 * Variáveis de Sessão: 
 * Uma sessão é criada sempre que um navegador faz uma chamada para um servidor web, ou seja, 
 * sempre que o navegador abre um site. Váriaveis de sessão podem ser criadas e ficam disponíveis 
 * em todas as chamadas a páginas do site. A sessão só é encerrada após um tempo de inatividade 
 * entre o navegador e o servidor web. Este tempo é configurável. Essas váriaves só são vista na 
 * sessão do usuário, ou seja, uma sessão não pode ver o conteúdo de outras sessões. 
 * Sessões são criadas no lado do servidor. Essas variáveis são gerenciadas através da classe Session.

 * Variáveis de Aplicação: 
 * Uma variável de aplicação é vista pela aplicação. Isso significa que 
 * enquanto a aplicação tiver ativa a variável está dispníviel. Somente quando o servidor for 
 * reiniciado essas variáveis deixam de exisitir. E mais: variáveis de aplicação são vistas por 
 * todos usuários da aplicação. No momento que o browser chamar alguma página do site, as variáveis 
 * de aplicação estarão disponívies. A classe que gerencia essas variáveis é a classe Application.          


*/

/*    

HttpApplication Eventos:

    Application_AcquireRequestState
    Ocorre quando ASP.NET adquire o estado atual (por exemplo, o estado de sessão), que está associada com a solicitação atual. 

    Application_AuthenticateRequest
    Ocorre quando um módulo de segurança estabeleceu a identidade do usuário. 

    Application_AuthorizeRequest 
    Ocorre quando um módulo de segurança verificou autorização do usuário. 

    Application_BeginRequest 
    Ocorre como o primeiro evento na cadeia de pipeline HTTP de execução quando ASP.NET responde a uma solicitação. 

    Application_Disposed 
    Adiciona um manipulador de eventos para ouvir o Disposed o evento no aplicativo. 

    Application_EndRequest 
    Ocorre como o último evento no pipeline HTTP cadeia de execução quando ASP.NET responde a uma solicitação. 

    Application_Error 
    Ocorre quando uma exceção não tratada é lançada. 

    Application_PostAcquireRequestState 
    Ocorre quando o estado do pedido (por exemplo, o estado de sessão), que está associada com a solicitação atual foi obtida. 

    Application_PostAuthenticateRequest 
    Ocorre quando um segurança módulo estabeleceu a identidade do usuário. 

    Application_PostAuthorizeRequest 
    Ocorre quando o usuário para a solicitação atual tenha sido autorizada. 

    Application_PostMapRequestHandler 
    Ocorre quando ASP.NET mapeou a solicitação atual para o manipulador de eventos apropriado. 

    Application_PostReleaseRequestState 
    Ocorre quando ASP.NET foi concluída a execução de todos os pedidos manipuladores de eventos e os dados de estado de solicitação foi armazenado. 

    Application_PostRequestHandlerExecute 
    Ocorre quando o manipulador de eventos ASP.NET (por exemplo, uma página ou um serviço Web XML) termina a execução. 

    Application_PostResolveRequestCache 
    Ocorre quando ASP.NET ignora a execução do manipulador de eventos atual e permite um módulo de armazenamento em cache para atender a uma solicitação do cache. 

    Application_PostUpdateRequestCache
    Ocorre quando ASP.NET completa atualização módulos de cache e armazenamento de respostas que são usadas para atender solicitações subseqüentes do cache. 

    Application_PreRequestHandlerExecute
    Ocorre antes ASP.NET começa a executar um manipulador de eventos (por exemplo, , uma página ou um serviço Web XML). 

    Application_PreSendRequestContent
    Ocorre antes ASP.NET envia conteúdo para o cliente. 

    Application_PreSendRequestHeaders 
    Ocorre antes ASP.NET envia cabeçalhos HTTP para o cliente. 

    Application_ReleaseRequestState 
    Ocorre depois que termina a execução ASP.NET todos os manipuladores de eventos do pedido. 
    Este evento faz com que os módulos do Estado para salvar os dados de estado atual. 

    Application_ResolveRequestCache 
    Ocorre quando ASP.NET completa um evento de autorização para deixar os módulos de cache atender 
    as solicitações a partir do cache, evitando a execução do manipulador de eventos (por exemplo, uma página ou um serviço Web XML ). 

    Application_UpdateRequestCache 
    Ocorre quando ASP.NET termina a execução de um manipulador de eventos a fim de deixar as respostas armazenar 
    módulos de cache que serão utilizados para atender as solicitações subseqüentes do cache. 

    Application_Init 
    Este método ocorre após _start e é usado para inicializar código. 

    Application_Start 
    Tal como acontece com tradicional ASP, usado para configurar um ambiente de aplicação e apenas chamado quando o aplicativo é iniciado pela primeira vez. 

    Application_End 
    Novamente, como o ASP clássico, usado para limpar as variáveis ​​e memória quando um aplicativo termina.     

Sessão Eventos:

    Session_Start 
    Tal como acontece com o ASP clássico, este evento é acionado quando qualquer novo usuário acessar o site web. 

    Session_End 
    Tal como acontece com o ASP clássico, este evento é acionado quando a sessão de um usuário para fora ou termina. 
    Note que esta pode ser de 20 minutos (o tempo limite de sessão padrão) depois que o usuário realmente deixa o site. 

Eventos de perfil:

    Profile_MigrateAnonymous 
    Ocorre quando o usuário anônimo para um perfil em toras.     

Passport Eventos:

    PassportAuthentication_OnAuthenticate 
    levantada durante a autenticação. Este é um evento Global.asax que deve ser nomeado PassportAuthentication_OnAuthenticate.     

*/
