using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using Posy.V2.Infra.CrossCutting.Common.Extensions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.MVC.Attributes;
using Posy.V2.MVC.Controllers.Base;
using Posy.V2.MVC.Helpers;
using Posy.V2.MVC.Models;
using Posy.V2.MVC.Validacao;
using System.Linq;
using System.Web.Mvc;

namespace Posy.V2.MVC.Controllers
{
    [Authorize]
    [ExceptionHandler]
    [UserCurrent]
    public class ModeradorController : BaseControllerComunidade
    {
        IMembroService _membroService;
        ITopicoService _topicoService;
        IModeradorService _moderadorService;

        public ModeradorController(IUnitOfWork uow,
                                    ICacheService cacheService,
                                    IPerfilService perfilService,
                                    IComunidadeService comunidadeService,
                                    IMembroService membroService,
                                    ITopicoService topicoService,
                                    IModeradorService moderadorService,
                                    IGlobalBaseController globalBaseController) :
            base(globalBaseController, uow, cacheService, perfilService, comunidadeService)
        {
            _membroService = membroService;
            _topicoService = topicoService;
            _moderadorService = moderadorService;
        }

        // GET: Moderador
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/moderador")]
        public ActionResult AddCmmModerador(AdicionarModeradorModel model)
        {
            using (_uow)
            {
                _moderadorService.AdicionarModerador(ComunidadeView.Id, model.UsuarioId);
                _uow.Commit();
            }
            return Json("");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/moderador/delete")]
        public ActionResult ExcluirModerador(ExcluirModeradorModel model)
        {
            using (_uow)
            {
                _moderadorService.ExcluirModerador(ComunidadeView.Id, model.ModeradorId);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpGet]
        [Route("ajax/moderador/template/view")]
        public ActionResult GetCmmModeradores()
        {
            var validacao = new CmmValidacao(PerfilLogged, ComunidadeView);

            var v_html = string.Empty;
            var v_html_menuitens = string.Empty;
            var v_html_titulo = string.Empty;
            var v_html_paginacao = string.Empty;

            if (validacao.IsDono || validacao.IsModerador || validacao.IsMembro || validacao.IsPublica)
            {
                #region LISTA DE MODERADORES

                int totalRecords;
                var moderadores = _moderadorService.ObterModeradores(ComunidadeView.Id, PAGE_NUMBER, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                if (moderadores.Count > 0)
                {
                    v_html = @"
                        <div class='ls-cnt-default poseydon-membros-list-size'>
                            <ul class='list-ul'>
                    ";

                    foreach (var moderador in moderadores)
                    {
                        var perfilModerador = moderador.UsuarioModerador.Perfil; // perfilDao.getObjeto(objeto.CodPerfilModerador);

                        urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilModerador.Alias);
                        urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfilModerador.Id.ToString() + "/1.jpg", false);

                        v_html += @"
                            <li class='list-li' cp='" + perfilModerador.Id.ToString() + @"'>
                                <img class='list-img' src='" + urlResolve + @"' />
                                <h3 class='list-h3'><a href='" + urlEncryptadaPerfil + "'>" + perfilModerador.Nome + " " + perfilModerador.Sobrenome + @"</a></h3>
                                <p class='list-p'>" + perfilModerador.Sexo.GetDescription() + @"<span></span></p>
                        ";

                        #region TOOLTIP MENU DE OPCOES

                        if (validacao.IsDono || (validacao.IsModerador && perfilModerador.Id == PerfilLogged.Id))
                        {
                            v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' cp='" + perfilModerador.Id.ToString() + "' opcao='E'>Excluir moderador</div></a>";

                            v_html += @"
                                <div class='popr tooltip-dropmenu-container' cp='" + perfilModerador.Id.ToString() + @"'>
                                    <div class='button-group minor-group'>

                                        <div class='tooltip-dropmenu'> 
                                            <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                            <div class='tooltip-dropmenu-html-container' cp='" + perfilModerador.Id.ToString() + @"'>
                                                <div class='tooltip-dropmenu-menu' cp='" + perfilModerador.Id.ToString() + @"'>
                                                    " + v_html_menuitens + @"
                                                </div>
                                            </div>
                                            <div class='clear'></div>
                                        </div>

                                    </div>
                                </div>
                            ";
                        }

                        #endregion

                        v_html += "</li>";
                    }

                    v_html += @"
                        </ul>
                    </div>
                ";
                }

                #endregion

                #region TITULO

                v_html_titulo = @"
                    <div class='ms-cnt-tit-blc'>
                        <h1 class='ms-tit-blc-txt icon-people'>"+ UIConfig.Moderadores +@" <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                        <div class='ms-tit-blc-line'></div>
                    </div>
                ";

                #endregion

                #region PAGINACAO

                v_html_paginacao = FuncaoSite.getPaginacaoCmm(PAGE_NUMBER, totalRecords, ComunidadeView, null, FuncaoSite.NomePagina.MODERADORES);

                #endregion
            }
            else
            {
                #region TITULO

                v_html_titulo = @"
                    <div class='ms-cnt-tit-blc'>
                        <h1 class='ms-tit-blc-txt icon-people'>Moderadores</h1>
                        <div class='ms-tit-blc-line'></div>
                    </div>
                ";

                #endregion
            }

            object[] arrRetorno = new object[3];
            arrRetorno[0] = v_html_titulo;
            arrRetorno[1] = v_html;
            arrRetorno[2] = v_html_paginacao;

            return Json(arrRetorno, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _membroService.Dispose();
            _topicoService.Dispose();
            _moderadorService.Dispose();
        }
    }
}