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
using System;
using System.Linq;
using System.Web.Mvc;

namespace Posy.V2.MVC.Controllers
{
    [Authorize]
    [ExceptionHandler]
    [UserCurrent]
    public class AmigoController : BaseControllerPerfil
    {
        IAmizadeService _amizadeService;

        public AmigoController(IUnitOfWork uow,
                                ICacheService cacheService,
                                IPerfilService perfilService,
                                IAmizadeService amizadeService,
                                IGlobalBaseController globalBaseController) :
            base(globalBaseController, uow, cacheService, perfilService)
        {
            _amizadeService = amizadeService;
        }

        // GET: Amigo
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/amigo")]
        public ActionResult AddAmigo(AdicionarAmigoModel model)
        {
            using (_uow)
            {
                _amizadeService.AdicionarSolicitacaoAmizade(model.UsuarioIdSolicitado);
                _uow.Commit();
            }

            var perfilView = PerfilView;// _perfilService.Obter(model.UsuarioIdSolicitado);
            var retorno = @"<li class='menu-li'><div class='icon-aguradamigo'></div><a cp='" + model.UsuarioIdSolicitado.ToString() + "' href='javascript:void(0);'>Esperando " + perfilView.Nome + " aceitar</a></li>";

            return Json(retorno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/amigo/aceitar")]
        public ActionResult AceitarAmigo(AceitarSolicitacaoAmizadeModel model)
        {
            using (_uow)
            {
                _amizadeService.AceitarSolicitacaoAmizade(model.UsuarioIdAceitar);
                _uow.Commit();

                var perfilAprovado = _perfilService.Obter(model.UsuarioIdAceitar);

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;
                var v_html_tooltip = string.Empty;
                var v_html_blocoamigos = string.Empty;

                #region TOOLTIP MENU DE OPCOES

                v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + perfilAprovado.Id.ToString() + "' opcao='AE'>Excluir amigo</div></a>";

                v_html_tooltip = @"
                    <div class='popr tooltip-dropmenu-container' codperfil='" + perfilAprovado.Id.ToString() + @"'>
                        <div class='button-group minor-group'>

                        <div class='tooltip-dropmenu'>
                            <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                            <div class='tooltip-dropmenu-html-container' codperfil='" + perfilAprovado.Id.ToString() + @"'>
                                <div class='tooltip-dropmenu-menu' codperfil='" + perfilAprovado.Id.ToString() + @"'>
                                    " + v_html_menuitens + @"
                                </div>
                            </div>
                            <div class='clear'></div>
                        </div>

                        </div>
                    </div>
                ";

                #endregion

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilAprovado.Alias);
                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfilAprovado.Id.ToString() + "/1.jpg", false);

                v_html = @"
                    <li class='list-li' codperfil='" + perfilAprovado.Id.ToString() + @"'>
                        <img class='list-img' src='" + urlResolve + @"'>
                        <h3 class='list-h3'><a href='" + urlEncryptadaPerfil + "'>" + perfilAprovado.Nome + " " + perfilAprovado.Sobrenome + @"</a></h3>
                        <p class='list-p'>" + perfilAprovado.Sexo.GetDescription() + @"<span></span></p>
                        " + v_html_tooltip + @"
                    </li>
                ";

                #region ITEM DO BLOCO DE AMIGOS

                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfilAprovado.Id.ToString() + "/1.jpg", false);

                v_html_blocoamigos = @"
                    <div class='gl-zoom tooltip2'  title='" + perfilAprovado.Nome + " " + perfilAprovado.Sobrenome + @"'>
                        <a href='" + urlEncryptadaPerfil + "' cp='" + perfilAprovado.Id.ToString() + @"'>
                            <span class='gl-zoom-caption'>
                                <h3>" + perfilAprovado.Nome + " " + perfilAprovado.Sobrenome + @"</h3>
                            </span>
                            <img class='gl-zoom-img' src='" + urlResolve + @"' />
                        </a>
                    </div>
                ";

                #endregion

                object[] arrRetorno = new object[2];
                arrRetorno[0] = v_html;
                arrRetorno[1] = v_html_blocoamigos;

                return Json(arrRetorno);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/amigo/recusar")]
        public ActionResult RecusarAmigo(UsuarioIdRecusarModel model)
        {
            using (_uow)
            {
                _amizadeService.RecusarSolicitacaoAmizade(model.UsuarioIdRecusar);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/amigo/excluir")]
        public ActionResult ExcluirAmigo(ExcluirAmigoModel model)
        {
            using (_uow)
            {
                _amizadeService.ExcluirAmigo(model.UsuarioIdParaExcluir);
                _uow.Commit();

                var retorno = "<li class='menu-li'><div class='icon-addamigo'></div><a cp='" + model.UsuarioIdParaExcluir.ToString() + "' class='btnViewPerfil' load='add' href='javascript:void(0);'>" + UIConfig.AdicionarAmigo + "</a></li>";

                return Json(retorno);
            }
        }

        [HttpGet]
        [Route("ajax/amigo/template/view")]
        public ActionResult GetPerfilAmigos()
        {
            var validacao = new PerfilValidacao(PerfilLogged.Id, PerfilView.Id);

            string v_codperfil;
            var v_html = string.Empty;
            var v_html_menuitens = string.Empty;
            var v_html_tooltip = string.Empty;
            var v_html_titulo = string.Empty;
            var v_html_paginacao = string.Empty;

            if (validacao.IsMeuPerfil() || validacao.IsAmigo())
            {
                #region LISTA AMIGOS JA ADICIONADOS

                int totalRecords;
                var amigos = _amizadeService.ObterAmigos(PerfilView.Id, PAGE_NUMBER, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                v_html = @"
                        <div id='containerAmigosAdicionados' class='ls-cnt-default poseydon-amigos-list-size'>
                            <ul class='list-ul'>
                    ";

                foreach (var amigo in amigos)
                {
                    v_codperfil = amigo.Id.ToString();

                    #region TOOLTIP MENU DE OPCOES

                    if (validacao.IsMeuPerfil())
                    {
                        #region HTML

                        v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + v_codperfil.ToString() + "' opcao='AE'>Excluir amigo</div></a>";

                        v_html_tooltip = @"
                                <div class='popr tooltip-dropmenu-container' codperfil='" + v_codperfil.ToString() + @"'>
                                    <div class='button-group minor-group'>

                                    <div class='tooltip-dropmenu'>
                                        <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                        <div class='tooltip-dropmenu-html-container' codperfil='" + v_codperfil.ToString() + @"'>
                                            <div class='tooltip-dropmenu-menu' codperfil='" + v_codperfil.ToString() + @"'>
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

                    #endregion

                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(amigo.Perfil.Alias);

                    urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + v_codperfil + "/1.jpg", false);

                    #region HTML

                    v_html += @"
                            <li class='list-li' codperfil='" + v_codperfil + @"'>
                                <img class='list-img' src='" + urlResolve + @"'>
                                <h3 class='list-h3'><a href='" + urlEncryptadaPerfil + "'>" + amigo.Perfil.Nome + " " + amigo.Perfil.Sobrenome + @"</a></h3>
                                <p class='list-p'>" + amigo.Perfil.Sexo.GetDescription() + @"<span></span></p>
                                " + v_html_tooltip + @"
                            </li>
                        ";

                    #endregion
                }

                v_html += @"
                            </ul>
                        </div>
                    ";

                #endregion

                #region TITULO

                v_html_titulo = @"
                        <div class='ms-cnt-tit-blc'>
                            <h1 class='ms-tit-blc-txt icon-people'>"+ UIConfig.Amigos +" <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                #endregion

                var perfilView = _perfilService.Obter(PerfilView.Id);

                #region PAGINACAO

                v_html_paginacao = FuncaoSite.getPaginacaoPerfil(PAGE_NUMBER, totalRecords, perfilView, FuncaoSite.NomePagina.AMIGO);

                #endregion

            }
            else
            {
                #region TITULO

                v_html_titulo = @"
                    <div class='ms-cnt-tit-blc'>
                        <h1 class='ms-tit-blc-txt icon-people'>Amigos</h1>
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
        [Route("ajax/amigo/pendente/template/view")]
        public ActionResult GetPerfilAmigosPendente()
        {
            int v_codperfil;
            var v_html = string.Empty;
            var v_html_menuitens = string.Empty;
            var v_html_tooltip = string.Empty;
            var v_html_titulo = string.Empty;
            var v_html_paginacao = string.Empty;

            int totalRecords;
            var solicitacoesRecebidasPendentes =
                _amizadeService.SolicitacoesRecebidasPendentes(PAGE_NUMBER, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

            #region LISTA SOLICITACOES DE AMIZADE

            v_html = @"
                            <div class='ls-cnt-default poseydon-amigos-list-size amigos-pendente'>
                                <ul class='list-ul'>
                        ";

            foreach (var solicitacao in solicitacoesRecebidasPendentes)
            {
                var usuarioSolicitacao = solicitacao.SolicitadoPor;
                v_codperfil = usuarioSolicitacao.Id;

                #region TOOLTIP MENU DE OPCOES

                v_html_menuitens = @"
                                <a href='javascript:void(0);'><div class='menuitem btnMenuItem' cppend='" + v_codperfil.ToString() + @"' opcao='AA'>Aceitar amizade</div></a>
                                <a href='javascript:void(0);'><div class='menuitem btnMenuItem' cppend='" + v_codperfil.ToString() + @"' opcao='AR'>Recusar amizade</div></a>
                            ";

                v_html_tooltip = @"
                                <div class='popr tooltip-dropmenu-container' cppend='" + v_codperfil.ToString() + @"'>
                                    <div class='button-group minor-group'>

                                    <div class='tooltip-dropmenu'> 
                                        <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                        <div class='tooltip-dropmenu-html-container' cppend='" + v_codperfil.ToString() + @"'>
                                            <div class='tooltip-dropmenu-menu' cppend='" + v_codperfil.ToString() + @"'>
                                                " + v_html_menuitens + @"
                                            </div>
                                        </div>
                                        <div class='clear'></div>
                                    </div>

                                    </div>
                                </div>
                            ";

                #endregion

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioSolicitacao.Perfil.Alias);
                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioSolicitacao.Id.ToString() + "/1.jpg", false);

                v_html += @"
                                <li class='list-li pendente' cppend='" + usuarioSolicitacao.Id.ToString() + @"'>
                                    <img class='list-img' src='" + urlResolve + @"' />
                                    <h3 class='list-h3'><a href='" + urlEncryptadaPerfil + "'>" + usuarioSolicitacao.Perfil.Nome + @" " + usuarioSolicitacao.Perfil.Sobrenome + @"</a></h3>
                                    <p class='list-p'>" + usuarioSolicitacao.Perfil.Sexo.GetDescription() + @"<span></span></p>
                                    " + v_html_tooltip + @"
                                </li>
                            ";
            }

            v_html += @"
                                </ul>
                            </div>
                        ";

            #endregion

            var totalRegistros = totalRecords;

            #region TITULO

            v_html_titulo = @"
                        <div class='ms-cnt-tit-blc'>
                            <h1 class='ms-tit-blc-txt icon-people'>"+ UIConfig.AmigosPendentes +" <span class='ms-tit-blc-btn'>" + totalRegistros.ToString() + @"</span></h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

            #endregion

            var perfilView = PerfilLogged; // _perfilServico.Obter(PerfilLogged);

            #region PAGINACAO

            v_html_paginacao = FuncaoSite.getPaginacaoPerfil(PAGE_NUMBER, totalRegistros, perfilView, FuncaoSite.NomePagina.AMIGOPENDENTE);

            #endregion

            object[] arrRetorno = new object[3];
            arrRetorno[0] = v_html_titulo;
            arrRetorno[1] = v_html;
            arrRetorno[2] = v_html_paginacao;

            return Json(arrRetorno, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _amizadeService.Dispose();
        }
    }
}