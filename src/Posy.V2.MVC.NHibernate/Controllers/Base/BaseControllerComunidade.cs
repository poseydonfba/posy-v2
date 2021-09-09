using Microsoft.AspNet.Identity;
using Posy.V2.Domain.Entities;
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
    public class BaseControllerComunidade : BaseController
    {
        protected readonly IComunidadeService _comunidadeService;

        public BaseControllerComunidade(IGlobalBaseController globalBaseController,
                                        IUnitOfWork uow,
                                        ICacheService cacheService,
                                        IPerfilService perfilService,
                                        IComunidadeService comunidadeService) :
            base(globalBaseController, uow, cacheService, perfilService)
        {
            _comunidadeService = comunidadeService;
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

            if (routeData.Values.ContainsKey("MS_DirectRouteMatches"))
            {
                var MS_DirectRouteMatches = ((IEnumerable<RouteData>)routeData.Values["MS_DirectRouteMatches"]).First();
                if (MS_DirectRouteMatches.Values.ContainsKey("idview"))
                {
                    var idview = MS_DirectRouteMatches.Values["idview"];

                    ComunidadeView = GetByIsOrAliasComunidade(idview.ToString());
                }

                if (MS_DirectRouteMatches.Values.ContainsKey("page"))
                {
                    if (!int.TryParse(MS_DirectRouteMatches.Values["page"].ToString(), out int page))
                        page = 1;

                    PAGE_NUMBER = page;
                }

                if (MS_DirectRouteMatches.Values.ContainsKey("topicoid"))
                {
                    if (!int.TryParse(MS_DirectRouteMatches.Values["topicoid"].ToString(), out int topicoId))
                        topicoId = 0;

                    TOPICO_NUMBER = topicoId;
                }
            }

            PAGE_NUMBER = PAGE_NUMBER < 1 ? 1 : PAGE_NUMBER;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewBag.GetTemplateMenuVerticalComunidadeHtml = _globalBaseController.GetTemplateMenuVerticalComunidadeHtml(ComunidadeView, PerfilLogged);
            ViewBag.GetTemplateMembrosHtml = _globalBaseController.GetTemplateMembrosHtml(ComunidadeView, PerfilLogged);
        }

        public Comunidade GetByIsOrAliasComunidade(string idview)
        {
            if (int.TryParse(idview, out int id))
                return _comunidadeService.Obter(id);

            return _comunidadeService.Obter(idview);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_comunidadeService != null)
                    _comunidadeService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}