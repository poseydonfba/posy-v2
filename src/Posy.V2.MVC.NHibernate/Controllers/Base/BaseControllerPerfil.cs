using Microsoft.AspNet.Identity;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using Posy.V2.MVC.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Posy.V2.MVC.Controllers
{
    public class BaseControllerPerfil : BaseController
    {

        public BaseControllerPerfil(IGlobalBaseController globalBaseController,
                                      IUnitOfWork uow,
                                      ICacheService cacheService,
                                      IPerfilService perfilService) :
            base(globalBaseController, uow, cacheService, perfilService)
        {
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (!requestContext.HttpContext.User.Identity.IsAuthenticated)
                return;

            VerifyCultureInfo(requestContext);

            PerfilLogged = _perfilService.Obter(int.Parse(requestContext.HttpContext.User.Identity.GetUserId()));

            var routeData = requestContext.RouteData;
            var controller = routeData.Values["controller"].ToString();

            /// Se for a controller inicio o usuario logado é o mesmo da pagina que esta visualizando
            if (controller.ToLower().Equals("inicio"))
            {
                PerfilView = PerfilLogged;
                return;
            }

            if (routeData.Values.ContainsKey("MS_DirectRouteMatches"))
            {
                var MS_DirectRouteMatches = ((IEnumerable<RouteData>)routeData.Values["MS_DirectRouteMatches"]).First();
                if (MS_DirectRouteMatches.Values.ContainsKey("idview"))
                {
                    var idview = MS_DirectRouteMatches.Values["idview"];

                    /// Verifica se o paramentro é a vew edit, para edição do perfil
                    if (idview.ToString().ToLower().Equals("edit"))
                        PerfilView = PerfilLogged;
                    else
                        PerfilView = GetByIsOrAliasPerfil(idview.ToString());
                }

                if (MS_DirectRouteMatches.Values.ContainsKey("page"))
                {
                    if (!int.TryParse(MS_DirectRouteMatches.Values["page"].ToString(), out int page))
                        page = 1;

                    PAGE_NUMBER = page;
                }
            }

            PAGE_NUMBER = PAGE_NUMBER < 1 ? 1 : PAGE_NUMBER;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewBag.GetTemplateMenuVerticalPerfilHtml = _globalBaseController.GetTemplateMenuVerticalPerfilHtml(PerfilLogged, PerfilView);
            ViewBag.GetTemplateVisitantesHtml = _globalBaseController.GetTemplateVisitantesHtml(PerfilLogged, PerfilView);
            ViewBag.GetTemplateAmigosHtml = _globalBaseController.GetTemplateAmigosHtml(PerfilLogged, PerfilView);
            ViewBag.GetTemplateCmmHtml = _globalBaseController.GetTemplateCmmHtml(PerfilLogged, PerfilView);
        }
    }
}