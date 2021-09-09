using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Cache;
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
    public class DepoimentoController : BaseControllerPerfil
    {
        IDepoimentoService _depoimentoService;

        public DepoimentoController(IUnitOfWork uow,
                                    ICacheService cacheService,
                                    IPerfilService perfilService,
                                    IDepoimentoService depoimentoService,
                                    IGlobalBaseController globalBaseController) :
            base(globalBaseController, uow, cacheService, perfilService)
        {
            _depoimentoService = depoimentoService;
        }

        // GET: Depoimento
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/depoimento")]
        public ActionResult SalvarDepoimento(SalvarDepoimentoModel model)
        {
            using (_uow)
            {
                var depoimento = _depoimentoService.EnviarDepoimento(PerfilView.UsuarioId, model.Depoimento);
                _uow.Commit();

                #region TEMPLATE POST

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;

                var perfilDepoimento = PerfilLogged; // _perfilServico.Obter(PerfilLogged);

                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + perfilDepoimento.UsuarioId.ToString() + "/1.jpg", false);

                v_html += @"
                    <div class='tl-cnt-default-block postado-recente' cd='" + depoimento.DepoimentoId.ToString() + @"'>
                        <div class='tl-cnt-default-img cd-picture'>
                            <img src='" + urlResolve + @"' alt='Picture' />
                        </div> <!-- tl-cnt-default-img -->

                        <div class='tl-cnt-default-content'>
                ";

                #region TOOLTIP MENU DE OPCOES

                v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' cd='" + depoimento.DepoimentoId.ToString() + "' opcao='E'>Excluir</div></a>";

                v_html += @"
                    <div class='popr tooltip-dropmenu-container' cd='" + depoimento.DepoimentoId.ToString() + @"'>
                        <div class='button-group minor-group'>

                        <div class='tooltip-dropmenu'>
                            <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                            <div class='tooltip-dropmenu-html-container' cd='" + depoimento.DepoimentoId.ToString() + @"'>
                                <div class='tooltip-dropmenu-menu' cd='" + depoimento.DepoimentoId.ToString() + @"'>
                                    " + v_html_menuitens + @"
                                </div>
                            </div>
                            <div class='clear'></div>
                        </div>

                        </div>
                    </div>
                ";

                #endregion

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilDepoimento.Alias);

                v_html += @"
                         <h2 class='tl-h2'><a href='" + urlEncryptadaPerfil + "'>" + perfilDepoimento.Nome + " " + perfilDepoimento.Sobrenome + "</a><span data-utime='1371248446' class='post-dt'>" + FuncaoSite.getTempoPost(depoimento.DataDepoimento) + @"</span><i>Depoimento pendente de aceitação<i></h2>
                         <h3 class='tl-h3'><span>" + perfilDepoimento.FraseHtml + @"</span></h3>
                         <div class='tl-text'>" + depoimento.DepoimentoHtml + @"</div>

            		        <a href='javascript:void(0);' class='tl-read-more'>Read more</a>
                         <span class='tl-date'>há 29 segundos</span>
                     </div><!-- tl-cnt-default-content -->
                    </div><!-- tl-cnt-default-block -->
                ";

                #endregion

                return Json(v_html);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/depoimento/delete")]
        public ActionResult ExcluirDepoimento(ExcluirDepoimentoModel model)
        {
            using (_uow)
            {
                _depoimentoService.ExcluirDepoimento(model.DepoimentoId);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/depoimento/aceitar")]
        public ActionResult AceitarDepoimento(AceitarRecusarDepoimentoModel model)
        {
            using (_uow)
            {
                _depoimentoService.AceitarDepoimento(model.DepoimentoId);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/depoimento/recusar")]
        public ActionResult RecusarDepoimento(AceitarRecusarDepoimentoModel model)
        {
            using (_uow)
            {
                _depoimentoService.RecusarDepoimento(model.DepoimentoId);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpGet]
        [Route("ajax/depoimento/template/view")]
        public ActionResult GetListDepoimentos()
        {
            var validacao = new PerfilValidacao(PerfilLogged.UsuarioId, PerfilView.UsuarioId);

            var v_html = string.Empty;
            var v_html_menuitens = string.Empty;
            var v_html_titulo = string.Empty;
            var v_html_paginacao = string.Empty;
            var v_html_editor = string.Empty;

            var status_depo = "";

            if (validacao.IsMeuPerfil() || validacao.IsAmigo())
            {
                int totalRecords;
                var depoimentos = _depoimentoService.ObterDepoimentosRecebidos(PerfilView.UsuarioId, PAGE_NUMBER, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                #region CONTAINER TIMELINE

                v_html += "<section class='tl-cnt-default'>";

                if (depoimentos.Count > 0)
                {
                    foreach (var depoimento in depoimentos)
                    {
                        var usuarioDepoimento = depoimento.EnviadoPor;

                        #region CONTAINER TIMELINE ITEM

                        urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioDepoimento.UsuarioId.ToString() + "/1.jpg", false);

                        v_html += @"
                                <div class='tl-cnt-default-block' cd='" + depoimento.DepoimentoId.ToString() + @"'>
                                    <div class='tl-cnt-default-img cd-picture'>
                                        <img src='" + urlResolve + @"' alt='Picture' />
                                    </div> <!-- tl-cnt-default-img -->

                                    <div class='tl-cnt-default-content'>
                            ";

                        if (validacao.IsMeuPerfil())
                        {
                            if (usuarioDepoimento.UsuarioId != PerfilLogged.UsuarioId)
                            {
                                #region TOOLTIP MENU DE OPCOES ITENS

                                if (depoimento.StatusDepoimento == StatusDepoimento.Pendente)
                                {
                                    v_html_menuitens = @"
                                            <a href='javascript:void(0);'><div class='menuitem btnMenuItem' cd='" + depoimento.DepoimentoId.ToString() + @"' opcao='A'>"+ UIConfig.aceitar +@"</div></a>
                                            <a href='javascript:void(0);'><div class='menuitem btnMenuItem' cd='" + depoimento.DepoimentoId.ToString() + @"' opcao='R'>" + UIConfig.recusar + @"</div></a>
                                        ";

                                    status_depo = "<i>" + UIConfig.VoceAindaNaoAceitouEsteDepoimento + "<i>";
                                }
                                else
                                {
                                    v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' cd='" + depoimento.DepoimentoId.ToString() + "' opcao='E'>" + UIConfig.excluir + @"</div></a>";

                                    status_depo = "";
                                }

                                #endregion
                            }
                            else
                            {
                                #region TOOLTIP MENU DE OPCOES ITENS

                                if (depoimento.StatusDepoimento == StatusDepoimento.Pendente)
                                    status_depo = "<i>Depoimento pendente de aceitação<i>";
                                else
                                    status_depo = "";

                                v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' cd='" + depoimento.DepoimentoId.ToString() + "' opcao='E'>Excluir</div></a>";

                                #endregion
                            }

                            #region TOOLTIP MENU DE OPCOES

                            v_html += @"
                                    <div class='popr tooltip-dropmenu-container' cd='" + depoimento.DepoimentoId.ToString() + @"'>
                                        <div class='button-group minor-group'>

                                            <div class='tooltip-dropmenu'> 
                                                <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                                <div class='tooltip-dropmenu-html-container' cd='" + depoimento.DepoimentoId.ToString() + @"'>
                                                    <div class='tooltip-dropmenu-menu' cd='" + depoimento.DepoimentoId.ToString() + @"'>
                                                        " + v_html_menuitens + @"
                                                    </div>
                                                </div>
                                                <div class='clear'></div>
                                            </div>

                                        </div>
                                    </div>
                                ";

                            #endregion
                        }
                        else
                        {
                            if (usuarioDepoimento.UsuarioId == PerfilLogged.UsuarioId)
                            {
                                #region TOOLTIP MENU DE OPCOES ITENS

                                if (depoimento.StatusDepoimento == StatusDepoimento.Pendente)
                                    status_depo = "<i>Depoimento pendente de aceitação<i>";
                                else
                                    status_depo = "";

                                v_html_menuitens = " <a href='javascript:void(0);'><div class='menuitem btnMenuItem' cd='" + depoimento.DepoimentoId.ToString() + "' opcao='E'>Excluir</div></a>";

                                #endregion

                                #region TOOLTIP MENU DE OPCOES

                                v_html += @"
                                        <div class='popr tooltip-dropmenu-container' cd='" + depoimento.DepoimentoId.ToString() + @"'>
                                            <div class='button-group minor-group'>

                                                <div class='tooltip-dropmenu'> 
                                                    <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                                    <div class='tooltip-dropmenu-html-container' cd='" + depoimento.DepoimentoId.ToString() + @"'>
                                                        <div class='tooltip-dropmenu-menu' cd='" + depoimento.DepoimentoId.ToString() + @"'>
                                                            " + v_html_menuitens + @"
                                                        </div>
                                                    </div>
                                                    <div class='clear'></div>
                                                </div>

                                            </div>
                                        </div>
                                    ";

                                #endregion
                            }
                        }

                        urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioDepoimento.Perfil.Alias);

                        #region HTML

                        v_html += @"
                                        <h2 class='tl-h2'><a href='" + urlEncryptadaPerfil + "'>" + usuarioDepoimento.Perfil.Nome + " " + usuarioDepoimento.Perfil.Sobrenome + "</a><span data-utime='1371248446' class='post-dt'>" + FuncaoSite.getTempoPost(depoimento.DataDepoimento) + "</span>" + status_depo + @"</h2>
                                        <h3 class='tl-h3'><span>" + usuarioDepoimento.Perfil.FraseHtml + @"</span></h3>
                                        <div class='tl-text'>" + depoimento.DepoimentoHtml + @"</div>

                        	            <a href='javascript:void(0);' class='tl-read-more'>Read more</a>
                                        <span class='tl-date'>há 29 segundos</span>
                                    </div><!-- tl-cnt-default-content -->
                                </div><!-- tl-cnt-default-block -->
                            ";

                        #endregion

                        #endregion
                    }
                }

                v_html += "</section>";

                #endregion

                #region TITULO

                v_html_titulo = @"
                        <div class='ms-cnt-tit-blc'>
                            <h1 class='ms-tit-blc-txt icon-depo'>"+ UIConfig.MeusDepoimentos +" <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                #endregion

                var perfilView = _perfilService.Obter(PerfilView.UsuarioId);

                #region PAGINACAO

                v_html_paginacao = FuncaoSite.getPaginacaoPerfil(PAGE_NUMBER, totalRecords, perfilView, FuncaoSite.NomePagina.DEPOIMENTO);

                #endregion

                #region EDITOR

                if (!validacao.IsMeuPerfil())
                {
                    v_html_editor = "<div id='cmtDepoimentoFrame' class='ed-cnt-cmt'></div>";
                }

                #endregion
            }
            else
            {
                #region TITULO

                v_html_titulo = @"
                        <div class='ms-cnt-tit-blc'>
                            <h1 class='ms-tit-blc-txt icon-depo'>"+ UIConfig.Depoimentos +@"</h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                #endregion
            }

            object[] arrRetorno = new object[4];
            arrRetorno[0] = v_html_titulo;
            arrRetorno[1] = v_html;
            arrRetorno[2] = v_html_paginacao;
            arrRetorno[3] = v_html_editor;

            return Json(arrRetorno, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _depoimentoService.Dispose();
        }
    }
}