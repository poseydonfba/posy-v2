using System.Web.Mvc;
using System.Web.Routing;

namespace Posy.V2.MVC.NHibernate
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                defaults: new { controller = "Inicio", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
