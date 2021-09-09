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
    public class MembroController : BaseControllerComunidade
    {
        IMembroService _membroService;
        ITopicoService _topicoService;

        public MembroController(IUnitOfWork uow,
                                ICacheService cacheService,
                                IPerfilService perfilService,
                                IComunidadeService comunidadeService,
                                IMembroService membroService,
                                ITopicoService topicoService,
                                IGlobalBaseController globalBaseController) :
            base(globalBaseController, uow, cacheService, perfilService, comunidadeService)
        {
            _membroService = membroService;
            _topicoService = topicoService;
        }

        // GET: Membro
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/membro/delete")]
        public ActionResult ExcluirMembro(AceitarMembroModel model)
        {
            using (_uow)
            {
                _membroService.ExcluirMembro(ComunidadeView.Id, model.MembroId);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/membro/pendente/aceitar")]
        public ActionResult AceitarMembro(AceitarMembroModel model)
        {
            using (_uow)
            {
                _membroService.AceitarMembro(ComunidadeView.Id, model.MembroId);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/membro/pendente/recusar")]
        public ActionResult RecusarMembro(RecusarMembroModel model)
        {
            using (_uow)
            {
                _membroService.RecusarMembro(ComunidadeView.Id, model.MembroId);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpGet]
        [Route("ajax/membro/template/view")]
        public ActionResult getCmmMembros()
        {
            var validacao = new CmmValidacao(PerfilLogged, ComunidadeView);

            var v_html = string.Empty;
            var v_html_menuitens = string.Empty;
            var v_html_titulo = string.Empty;
            var v_html_paginacao = string.Empty;

            if (validacao.IsDono || validacao.IsModerador || validacao.IsMembro || validacao.IsPublica)
            {
                #region LISTA DE MEMBROS

                int totalRecords;
                var membros = _membroService.ObterMembros(ComunidadeView.Id, PAGE_NUMBER, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                if (membros.Count > 0)
                {
                    v_html = @"
                        <div class='ls-cnt-default poseydon-membros-list-size'>
                            <ul class='list-ul'>
                    ";

                    foreach (var membro in membros)
                    {
                        var perfilMembro = membro.UsuarioMembro.Perfil;

                        urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilMembro.Alias);
                        urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfilMembro.Id.ToString() + "/1.jpg", false);

                        v_html += @"
                                    <li class='list-li' cp='" + perfilMembro.Id.ToString() + @"' class='pendente'>
                                        <img class='list-img' src='" + urlResolve + @"' />
                                        <h3 class='list-h3'><a href='" + urlEncryptadaPerfil + "'>" + perfilMembro.Nome + " " + perfilMembro.Sobrenome + @"</a></h3>
                                        <p class='list-p'>" + perfilMembro.Sexo.GetDescription() + @"<span></span></p>
                                ";

                        #region TOOLTIP MENU DE OPCOES

                        // Se eu sou dono da cmm e o membro currente não é o dono (Pode excluir)
                        var validacao1 = (validacao.IsDono && ComunidadeView.UsuarioId != perfilMembro.Id);
                        // Se eu sou moderador da cmm e o membro currente não é o dono (Pode excluir)
                        var validacao2 = (validacao.IsModerador && ComunidadeView.UsuarioId != perfilMembro.Id);
                        // Se eu sou membro da cmm e o membro currente não é o meu (Pode excluir) 
                        var validacao3 = (validacao.IsMembro && perfilMembro.Id == PerfilLogged.Id);

                        if (validacao1 || validacao2 || validacao3)
                        {
                            v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' cp='" + perfilMembro.Id.ToString() + "' opcao='E'>Excluir membro</div></a>";

                            v_html += @"
                                        <div class='popr tooltip-dropmenu-container' cp='" + perfilMembro.Id.ToString() + @"'>
                                            <div class='button-group minor-group'>

                                            <div class='tooltip-dropmenu'> 
                                                <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                                <div class='tooltip-dropmenu-html-container' cp='" + perfilMembro.Id.ToString() + @"'>
                                                    <div class='tooltip-dropmenu-menu' cp='" + perfilMembro.Id.ToString() + @"'>
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

                        v_html += @"
                                </li>
                            ";
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
                        <h1 class='ms-tit-blc-txt icon-people'>" + UIConfig.Membros + @" <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                        <div class='ms-tit-blc-line'></div>
                    </div>
                ";

                #endregion

                #region PAGINACAO

                v_html_paginacao = FuncaoSite.getPaginacaoCmm(PAGE_NUMBER, totalRecords, ComunidadeView, null, FuncaoSite.NomePagina.MEMBROS);

                #endregion
            }
            else
            {
                #region TITULO

                v_html_titulo = @"
                    <div class='ms-cnt-tit-blc'>
                        <h1 class='ms-tit-blc-txt icon-people'>Membros</h1>
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

        [HttpGet]
        [Route("ajax/membro/pendente/template/view")]
        public ActionResult getCmmMembrosPendente()
        {
            var validacao = new CmmValidacao(PerfilLogged, ComunidadeView);

            var v_html = string.Empty;
            var v_html_menuitens = string.Empty;
            var v_html_titulo = string.Empty;
            var v_html_paginacao = string.Empty;

            if (validacao.IsDono || validacao.IsModerador)
            {
                #region LISTA SOLICITACOES PENDENTE

                int totalRecords;
                var membrosPendentes = _membroService.ObterMembrosPendentes(ComunidadeView.Id, PAGE_NUMBER, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                if (membrosPendentes.Count > 0)
                {
                    v_html = @"
                            <div class='ls-cnt-default poseydon-membrospend-list-size'>
                                <ul class='list-ul'>
                        ";

                    foreach (var membroPendente in membrosPendentes)
                    {
                        var perfilPendente = membroPendente.UsuarioMembro.Perfil;

                        urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilPendente.Alias);
                        urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfilPendente.Id.ToString() + "/1.jpg", false);

                        v_html += @"
                                <li class='list-li pendente' cppend='" + perfilPendente.Id.ToString() + @"'>
                                    <img class='list-img' src='" + urlResolve + @"' />
                                    <h3 class='list-h3'><a href='" + urlEncryptadaPerfil + "'>" + perfilPendente.Nome + " " + perfilPendente.Sobrenome + @"</a></h3>
                                    <p class='list-p'>" + perfilPendente.Sexo.GetDescription() + @"<span></span></p>
                            ";

                        #region TOOLTIP MENU DE OPCOES

                        v_html_menuitens = @"
                                <a href='javascript:void(0);'><div class='menuitem btnMenuItem' cppend='" + perfilPendente.Id.ToString() + @"' opcao='A'>Aceitar membro</div></a>
                                <a href='javascript:void(0);'><div class='menuitem btnMenuItem' cppend='" + perfilPendente.Id.ToString() + @"' opcao='R'>Recusar membro</div></a>
                            ";

                        v_html += @"
                                <div class='popr tooltip-dropmenu-container' cppend='" + perfilPendente.Id.ToString() + @"'>
                                    <div class='button-group minor-group'>

                                    <div class='tooltip-dropmenu'>
                                        <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                        <div class='tooltip-dropmenu-html-container' cppend='" + perfilPendente.Id.ToString() + @"'>
                                            <div class='tooltip-dropmenu-menu' cppend='" + perfilPendente.Id.ToString() + @"'>
                                                " + v_html_menuitens + @"
                                            </div>
                                        </div>
                                        <div class='clear'></div>
                                    </div>

                                    </div>
                                </div>
                            ";

                        #endregion

                        v_html += @"
                                </li>
                            ";
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
                            <h1 class='ms-tit-blc-txt icon-membropend'>" + UIConfig.MembrosPendentes + @" <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                #endregion

                #region PAGINACAO

                v_html_paginacao = FuncaoSite.getPaginacaoCmm(PAGE_NUMBER, totalRecords, ComunidadeView, null, FuncaoSite.NomePagina.MEMBROSPENDENTE);

                #endregion
            }
            else
            {
                #region TITULO

                v_html_titulo = @"
                        <div class='ms-cnt-tit-blc'>
                            <h1 class='ms-tit-blc-txt icon-membropend'>Membros pendente</h1>
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
        }
    }
}