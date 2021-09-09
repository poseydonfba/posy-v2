using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.MVC.Attributes;
using Posy.V2.MVC.Controllers.Base;
using Posy.V2.MVC.Helpers;
using System.Linq;
using System.Web.Mvc;

namespace Posy.V2.MVC.Controllers
{
    [Authorize]
    [ExceptionHandler]
    [UserCurrent]
    public class ChatController : BaseControllerPerfil
    {
        IAmizadeService _amizadeService;

        public ChatController(IUnitOfWork uow,
                              ICacheService cacheService,
                              IPerfilService perfilService,
                              IAmizadeService amizadeService,
                              IGlobalBaseController globalBaseController) :
            base(globalBaseController, uow, cacheService, perfilService)
        {
            _amizadeService = amizadeService;
        }

        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("ajax/chat/membros/template/view")]
        public ActionResult GetChatMembers()
        {
            string v_html = string.Empty;

            int totalRecords;
            var amigos = _amizadeService.ObterAmigos(PerfilLogged.Id, 1, FuncaoSite.TOTAL_ITEM_BLOCO * 1000000, out totalRecords).ToList();

            foreach (var amigo in amigos)
            {
                var pathImg = Server.MapPath("~/Images/perfil/" + amigo.Perfil.Id.ToString() + "/1.jpg");

                string base64Image = Conversion.ImageToBase64(pathImg);

                #region HTML

                v_html += @"
                        <div class='mb-attribution animated slideInUp' cp='" + amigo.Perfil.Id.ToString() + @"'>
                            <p class='mb-author'>" + amigo.Perfil.Nome + " " + amigo.Perfil.Sobrenome + @"</p>
                            <cite>" + amigo.Perfil.FraseHtml + @"</cite>
                            <div class='mb-thumb' style='background-image: url(data:image/jpeg;base64," + base64Image + @");'></div>
                            <div class='mb-notification'><span></span>12:00</div>
                        </div>
                        ";

                #endregion
            }

            return Json(v_html, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _amizadeService.Dispose();
        }
    }
}