using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.MVC.Attributes;
using Posy.V2.MVC.Controllers.Base;
using Posy.V2.MVC.Helpers;
using Posy.V2.MVC.Validacao;
using System.Linq;
using System.Web.Mvc;

namespace Posy.V2.MVC.Controllers
{
    [Authorize]
    [ExceptionHandler]
    [UserCurrent]
    public class VisitanteController : BaseControllerPerfil
    {
        IVisitantePerfilService _visitantePerfilService;

        public VisitanteController(IUnitOfWork uow,
                                   ICacheService cacheService,
                                   IPerfilService perfilService,
                                   IVisitantePerfilService visitantePerfilService,
                                   IGlobalBaseController globalBaseController) :
            base(globalBaseController, uow, cacheService, perfilService)
        {
            _visitantePerfilService = visitantePerfilService;
        }

        // GET: Visitante
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("visitante")]
        public ActionResult VisitarPerfil()
        {
            using (_uow)
            {
                _visitantePerfilService.SalvarVisita(PerfilView.Id);
                _uow.Commit();
            }

            return Json("");
        }

        //[HttpGet]
        //[OutputCache(Duration = 10, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        //[Route("ajax/perfil/template/visitantes")]
        //public ActionResult GetTemplateVisitantes()
        //{
        //    return Json(_globalBaseController.GetTemplateVisitantesHtml(PerfilLogged, PerfilView), JsonRequestBehavior.AllowGet);
        //}

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _visitantePerfilService.Dispose();
        }
    }
}