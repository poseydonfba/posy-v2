using Posy.V2.Domain.Entities;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Posy.V2.MVC.Controllers
{
    [Authorize]
    [ExceptionHandler]
    [UserCurrent]
    public class RecadoController : BaseControllerPerfil
    {
        IRecadoService _recadoService;
        IRecadoComentarioService _recadoComentarioService;
        IPostPerfilBloqueadoService _perfilBloqueadoService;
        IPrivacidadeService _privacidadeService;

        public RecadoController(IUnitOfWork uow,
                                ICacheService cacheService,
                                IPerfilService perfilService,
                                IRecadoService recadoService,
                                IRecadoComentarioService recadoComentarioService,
                                IPostPerfilBloqueadoService perfilBloqueadoService,
                                IPrivacidadeService privacidadeService,
                                IGlobalBaseController globalBaseController) :
            base(globalBaseController, uow, cacheService, perfilService)
        {
            _recadoService = recadoService;
            _recadoComentarioService = recadoComentarioService;
            _perfilBloqueadoService = perfilBloqueadoService;
            _privacidadeService = privacidadeService;
        }

        // GET: Recado
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/recado")]
        public ActionResult SalvarRecado(SalvarRecadoModel model)
        {
            Recado recado = null;
            using (_uow)
            {
                recado = _recadoService.EnviarRecado(PerfilView.UsuarioId, model.RecadoHtml);
                _uow.Commit();

                var v_codrecado = recado.RecadoId.ToString();

                var perfilRecebeuRecado = PerfilView; // _perfilServico.Obter(perfilView);

                #region TEMPLATE POST

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;
                var v_html_paginacao = string.Empty;

                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + PerfilLogged.UsuarioId.ToString() + "/1.jpg", false);

                v_html = @"
                        <div class='tl-cnt-default-block postado-recente' codrecado='" + recado.RecadoId.ToString() + "' codperfil='" + PerfilLogged.UsuarioId.ToString() + @"'>
                            <div class='tl-cnt-default-img cd-picture'>
                                    <img src='" + urlResolve + @"' alt='Picture' />
                            </div> <!-- tl-cnt-default-img -->

                            <div class='tl-cnt-default-content'>
                    ";

                #region TOOLTIP MENU DE OPCOES

                if (PerfilView.UsuarioId == PerfilLogged.UsuarioId)
                {
                    v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codrecado='" + v_codrecado.ToString() + "' opcao='RE'>" + UIConfig.Excluirrecado + @"</div></a>";
                }
                else
                {
                    v_html_menuitens = @"
                        <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codrecado='" + v_codrecado.ToString() + @"' opcao='RE'>" + UIConfig.Excluirrecado + @"</div></a>
                        <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + PerfilLogged.UsuarioId.ToString() + @"' opcao='RB'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
                    ";
                }

                v_html += @"
                    <div class='popr tooltip-dropmenu-container' codrecado='" + v_codrecado.ToString() + @"'>
                        <div class='button-group minor-group'>

                        <div class='tooltip-dropmenu'> 
                            <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                            <div class='tooltip-dropmenu-html-container' codrecado='" + v_codrecado.ToString() + @"'>
                                <div class='tooltip-dropmenu-menu' codrecado='" + v_codrecado.ToString() + @"'>
                                    " + v_html_menuitens + @"
                                </div>
                            </div>
                            <div class='clear'></div>
                        </div>

                        </div>
                    </div>
                ";

                #endregion

                //v_html += "<input type='checkbox' class='tl-checkbox' value='" + recado.RecadoId.ToString() + "' />";

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilRecebeuRecado.Alias);
                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfilRecebeuRecado.UsuarioId.ToString() + "/1.jpg", false);

                v_html += "<h2 class='tl-h2'><a href='" + urlEncryptadaPerfil + "'>" + PerfilLogged.Nome + " " + PerfilLogged.Sobrenome + "</a><span data-utime='1371248446' class='post-dt'>" + FuncaoSite.getTempoPost(recado.DataRecado) + "</span></h2>";
                v_html += @"
                                <h3 class='tl-h3'><span>&nbsp;</span></h3>

                            <div class='tl-cnt-default-img-para'>
                            <span>" + UIConfig.Para + @": </span>
                            <img src='" + urlResolve + @"' class='cmt-cnt-img-perfil-tooltip-big' alt='Picture' />
                        ";

                //urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioRecebeuRecado.Perfil.Alias, usuarioRecebeuRecado.UsuarioId);

                #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfilRecebeuRecado.UsuarioId.ToString() + "/1.jpg", false);

                v_html += @"
                                        <div class='tooltip-dropmenu-html-container'>
                                            <div class='tooltip'>
                                                <span class='tooltip-content'>
                                                    <img src='" + urlResolve + @"' alt='' />
                                                    <span class='tooltip-text'>
                                                        <a class='tooltip-text-a' href='" + urlEncryptadaPerfil + "'>" + perfilRecebeuRecado.Nome + " " + perfilRecebeuRecado.Sobrenome + "</a><hr><span class='tooltip-text-span'>" + perfilRecebeuRecado.FraseHtml + @"</span> 
                                                    </span>
                                                </span>
                                            </div>
                                        </div>
                                    ";

                #endregion

                v_html += @"
                            <h2 class='tl-h2'><a href='" + urlEncryptadaPerfil + "'>" + perfilRecebeuRecado.Nome + " " + perfilRecebeuRecado.Sobrenome + @"</a></h2>
                            </div>

                                    <div class='tl-text'>" + recado.RecadoHtml + @"</div>
                        ";

                #region CONTAINER EDITOR

                var usuarioDoRecadoBloqueadoParaPostarNoUsuarioPost =
                    _perfilBloqueadoService.ObterPerfilBloqueado(perfilRecebeuRecado.UsuarioId, PerfilLogged.UsuarioId);

                // Verifica se o usuario esta bloqueado para postar bloqueado 0(Pode postar) e 1
                // Sempre faz a verificação pois estão misturados posts de outros usuarios com os meus, logo os meus não precisam esta bloqueados
                if (usuarioDoRecadoBloqueadoParaPostarNoUsuarioPost != null)
                    v_html += " <div class='ed-cnt-cmt' bloqueado='1' >";
                else
                    v_html += " <div class='ed-cnt-cmt' bloqueado='0' >";

                #region COMENTARIOS DO POST

                //var comentarios = _recadoComentarioService.ObterComentarios(recado.RecadoId, 1, FuncaoSite.TOTAL_COMENT_PAGE);

                v_html += "<div class='ed-cnt-cmt-coments'>";

                if (1 == 2)
                {

                    //foreach (var comentario in comentarios)
                    //{
                    //    var usuarioComentario = comentario.Usuario;

                    //    urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                    //    v_html += @"
                    //                                <div class='cmt-cnt' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
                    //                                    <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                    //                            ";

                    //    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioComentario.Perfil.Alias, usuarioComentario.UsuarioId);

                    //    #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                    //    urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                    //    v_html += @"
                    //                                <div class='tooltip-dropmenu-html-container'>
                    //                                    <div class='tooltip'>
                    //                                        <span class='tooltip-content'>
                    //                                            <img src='" + urlResolve + @"' alt='' />
                    //                                            <span class='tooltip-text'>
                    //                                                <a class='tooltip-text-a' href='" + urlEncryptadaPerfil + "'>" + usuarioComentario.Perfil.Nome + " " + usuarioComentario.Perfil.Sobrenome + "</a><hr><span class='tooltip-text-span'>" + usuarioComentario.Perfil.FraseHtml + @"</span> 
                    //                                            </span>
                    //                                        </span>
                    //                                    </div>
                    //                                </div>
                    //                            ";

                    //    #endregion

                    //    v_html += "<div class='thecom'>";

                    //    #region TOOLTIP MENU DE OPCOES

                    //    if (usuarioComentario.UsuarioId == PerfilLogged.UsuarioId)
                    //    {
                    //        v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.RecadoComentarioId.ToString() + "' opcao='RCE'>" + UIConfig.ExcluirComentário + @"</div></a>";
                    //    }
                    //    else
                    //    {
                    //        v_html_menuitens = @"
                    //                                    <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.RecadoComentarioId.ToString() + @"' opcao='RCE'>" + UIConfig.ExcluirComentário + @"</div></a>
                    //                                    <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + usuarioComentario.UsuarioId.ToString() + @"' opcao='RCB'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
                    //                                ";
                    //    }

                    //    v_html += @"
                    //                                <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
                    //                                    <div class='button-group minor-group'>

                    //                                    <div class='tooltip-dropmenu'> 
                    //                                        <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                    //                                        <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
                    //                                            <div class='tooltip-dropmenu-menu' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
                    //                                                " + v_html_menuitens + @"
                    //                                            </div>
                    //                                        </div>
                    //                                        <div class='clear'></div>
                    //                                    </div>

                    //                                    </div>
                    //                                </div>
                    //                            ";

                    //    #endregion

                    //    v_html += @"
                    //                                         <h5><a href='" + urlEncryptadaPerfil + "'>" + usuarioComentario.Perfil.Nome + " " + usuarioComentario.Perfil.Sobrenome + "</a></h5><span data-utime='1371248446' class='com-dt'>" + FuncaoSite.getTempoPost(comentario.DataComentario) + @"</span>
                    //                                         <div class='com-text'>" + comentario.ComentarioHtml + @"</div>
                    //                                     </div>
                    //                                </div><!-- end 'cmt-cnt' -->
                    //                            ";
                    //}

                    //var proximosComentarios = _recadoComentarioService.ObterComentarios(recado.RecadoId, nextPage, FuncaoSite.TOTAL_COMENT_PAGE).ToList();
                    //if (proximosComentarios.Count > 0)
                    //{
                    //    v_html += @"
                    //                                <div class='ed-cnt-cmt-more button-group minor-group'>
                    //                                     <a href='javascript:void(0);' class='button minor1 btnVerMais' codrecado='" + recado.RecadoId.ToString() + @"' page='" + nextPage.ToString() + @"'>" + BTN_VER_MAIS_COMENTS + @"</a>
                    //                                </div>
                    //                            ";
                    //}

                }

                v_html += "</div>";

                #endregion

                v_html += " </div><!-- end of comments container 'ed-cnt-cmt' -->";

                #endregion

                v_html += @"
                                    <a href='javascript:void(0);' class='tl-read-more'>Read more</a>
                                    <span class='tl-date'>há 29 segundos</span>
                                </div><!-- tl-cnt-default-content -->
                            </div><!-- tl-cnt-default-block -->
                        ";

                #endregion

                #region PAGINACAO

                v_html_paginacao = FuncaoSite.getPaginacaoPerfil(1, 1, PerfilLogged, FuncaoSite.NomePagina.RECADO);

                #endregion

                object[] arrRetorno = new object[2];
                arrRetorno[0] = v_html;
                arrRetorno[1] = v_html_paginacao;

                return Json(arrRetorno);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/recado/delete")]
        public ActionResult ExcluirRecado(ExcluirRecadoModel model)
        {
            using (_uow)
            {
                _recadoService.ExcluirRecado(model.RecadoIdParaExcluir);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/recado/delete/multiple")]
        public ActionResult ExcluirMultiplosRecados(List<ExcluirRecadoModel> model)
        {
            using (_uow)
            {
                foreach (var recado in model)
                    _recadoService.ExcluirRecado(recado.RecadoIdParaExcluir);

                _uow.Commit();
            }
            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/recado/comentario")]
        public ActionResult SalvarRecadoComentario(SalvarRecadoComentarioModel model)
        {
            using (_uow)
            {
                var recado = _recadoService.ObterRecado(model.RecadoId);
                var usuarioComPostBloqueado = _perfilBloqueadoService.ObterPerfilBloqueado(recado.EnviadoPorId, PerfilLogged.UsuarioId);
                if (usuarioComPostBloqueado != null)
                    throw new Exception(Errors.ErroAoEnviarComentario);

                var comentario = _recadoComentarioService.Comentar(model.RecadoId, model.Comentario);

                _uow.Commit();

                #region TEMPLATE COMENTARIO

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;

                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + PerfilLogged.UsuarioId.ToString() + "/1.jpg", false);

                v_html += @"
                        <div class='cmt-cnt' codcoment='" + comentario.RecadoComentarioId.ToString() + @"' style='display:none;'>
                            <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                    ";

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(PerfilLogged.Alias);

                #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + PerfilLogged.UsuarioId.ToString() + "/1.jpg", false);

                v_html += @"
                    <div class='tooltip-dropmenu-html-container'>
                        <div class='tooltip'>
                            <span class='tooltip-content'>
                                <img src='" + urlResolve + @"' alt='' />
                                <span class='tooltip-text'>
                                    <a class='tooltip-text-a' href='" + urlEncryptadaPerfil + "'>" + PerfilLogged.Nome + " " + PerfilLogged.Sobrenome + "</a><hr><span class='tooltip-text-span'>" + PerfilLogged.FraseHtml + @"</span> 
                                </span>
                            </span>
                        </div>
                    </div>
                ";

                #endregion

                v_html += "<div class='thecom'>";

                #region TOOLTIP MENU DE OPCOES

                v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.RecadoComentarioId.ToString() + "' opcao='RCE'>" + UIConfig.ExcluirComentário + @"</div></a>";

                v_html += @"
                    <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
                        <div class='button-group minor-group'>

                        <div class='tooltip-dropmenu'> 
                            <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                            <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
                                <div class='tooltip-dropmenu-menu' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
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
                                 <h5><a href='" + urlEncryptadaPerfil + "'>" + PerfilLogged.Nome + " " + PerfilLogged.Sobrenome + "</a></h5><span data-utime='1371248446' class='com-dt'>" + FuncaoSite.getTempoPost(DateTime.Now) + @"</span>
                                 <div class='com-text'>" + comentario.ComentarioHtml + @"</div>
                             </div>
                        </div><!-- end 'cmt-cnt' -->
                    ";

                #endregion

                return Json(v_html);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/recado/comentario/delete")]
        public ActionResult ExcluirRecadoComentario(ExcluirRecadoComentarioModel model)
        {
            using (_uow)
            {
                _recadoComentarioService.ExcluirComentario(model.RecadoComentarioId);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpGet]
        [Route("ajax/recado/template/view")]
        public ActionResult GetListRecados()
        {
            var validacao = new PerfilValidacao(PerfilLogged.UsuarioId, PerfilView.UsuarioId);
            var isAmigo = validacao.IsAmigo();

            string v_html = string.Empty;
            var v_html_menuitens = string.Empty;
            var v_html_titulo = string.Empty;
            var v_html_paginacao = string.Empty;
            var v_html_editor = string.Empty;
            var v_html_btntop = string.Empty;
            var v_html_btnvermais = string.Empty;

            int totalRecords = 0;
            var nextPage = PAGE_NUMBER + 1;

            using (_uow)
            {
                var priv = _privacidadeService.Obter();

                if (validacao.IsMeuPerfil() || isAmigo)
                {
                    var recados = _recadoService.ObterRecadosEnviadosERecebidos(PerfilView.UsuarioId, PAGE_NUMBER, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                    if (recados.Count > 0)
                    {
                        #region TIMELINE

                        v_html = "<section class='tl-cnt-default'>";

                        foreach (var recado in recados)
                        {
                            var usuarioRecado = recado.EnviadoPor;
                            var usuarioRecebeuRecado = recado.EnviadoPara;
                            var perfilBloqueado = _perfilBloqueadoService.ObterPerfilBloqueado(usuarioRecebeuRecado.UsuarioId, usuarioRecado.UsuarioId);

                            #region CONTAINER

                            urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioRecado.UsuarioId.ToString() + "/1.jpg", false);

                            #region HTML

                            v_html += @"
                                        <div class='tl-cnt-default-block' codrecado='" + recado.RecadoId.ToString() + @"' style='display:none;'>
                                            <div class='tl-cnt-default-img cd-picture'>
                                                    <img src='" + urlResolve + @"' alt='Picture' />
                                            </div> <!-- tl-cnt-default-img -->

                                            <div class='tl-cnt-default-content'>
                                    ";

                            #endregion

                            #region TOOLTIP MENU DE OPCOES

                            if (usuarioRecado.UsuarioId == PerfilLogged.UsuarioId)
                            {
                                v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codrecado='" + recado.RecadoId.ToString() + @"' opcao='RE'>" + UIConfig.Excluirrecado + @"</div></a>";
                            }
                            else if (validacao.IsMeuPerfil())
                            {
                                v_html_menuitens = @"
                                            <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codrecado='" + recado.RecadoId.ToString() + @"' opcao='RE'>" + UIConfig.Excluirrecado + @"</div></a>
                                        ";

                                if (perfilBloqueado == null)
                                    v_html_menuitens += @"
                                            <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codrecado='" + recado.RecadoId.ToString() + @"' codperfil='" + usuarioRecado.UsuarioId.ToString() + @"' opcao='RB'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
                                        ";
                            }

                            if (v_html_menuitens.ToString().Length > 0)
                            {
                                v_html += @"
                                            <div class='popr tooltip-dropmenu-container' codrecado='" + recado.RecadoId.ToString() + @"'>
                                                <div class='button-group minor-group'>

                                                <div class='tooltip-dropmenu'> 
                                                    <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                                    <div class='tooltip-dropmenu-html-container' codrecado='" + recado.RecadoId.ToString() + @"'>
                                                        <div class='tooltip-dropmenu-menu' codrecado='" + recado.RecadoId.ToString() + @"'>
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

                            if (validacao.IsMeuPerfil())
                                v_html += "<input type='checkbox' class='tl-checkbox' value='" + recado.RecadoId.ToString() + "' />";

                            urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioRecado.Perfil.Alias);
                            urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioRecebeuRecado.UsuarioId.ToString() + "/1.jpg", false);

                            v_html += "<h2 class='tl-h2'><a href='" + urlEncryptadaPerfil + "'>" + usuarioRecado.Perfil.Nome + " " + usuarioRecado.Perfil.Sobrenome + "</a><span data-utime='1371248446' class='post-dt'>" + FuncaoSite.getTempoPost(recado.DataRecado) + "</span></h2>";
                            v_html += @"
                                             <h3 class='tl-h3'><span>&nbsp;</span></h3>

                                        <div class='tl-cnt-default-img-para'>
                                        <span>"+ UIConfig.Para +@": </span>
                                        <img src='" + urlResolve + @"' class='cmt-cnt-img-perfil-tooltip-big' alt='Picture' />
                                    ";

                            urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioRecebeuRecado.Perfil.Alias);

                            #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                            urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioRecebeuRecado.UsuarioId.ToString() + "/1.jpg", false);

                            v_html += @"
                                        <div class='tooltip-dropmenu-html-container'>
                                            <div class='tooltip'>
                                                <span class='tooltip-content'>
                                                    <img src='" + urlResolve + @"' alt='' />
                                                    <span class='tooltip-text'>
                                                        <a class='tooltip-text-a' href='" + urlEncryptadaPerfil + "'>" + usuarioRecebeuRecado.Perfil.Nome + " " + usuarioRecebeuRecado.Perfil.Sobrenome + "</a><hr><span class='tooltip-text-span'>" + usuarioRecebeuRecado.Perfil.FraseHtml + @"</span> 
                                                    </span>
                                                </span>
                                            </div>
                                        </div>
                                    ";

                            #endregion

                            v_html += @"
                                        <h2 class='tl-h2'><a href='" + urlEncryptadaPerfil + "'>" + usuarioRecebeuRecado.Perfil.Nome + " " + usuarioRecebeuRecado.Perfil.Sobrenome + @"</a></h2>
                                        </div>

                                             <div class='tl-text'>" + recado.RecadoHtml + @"</div>
                                    ";

                            // So comenta se for um recado enviado para o usuario logado
                            // ou se foi enviado pelo usuario logado
                            if (usuarioRecado.UsuarioId == PerfilLogged.UsuarioId ||
                                usuarioRecebeuRecado.UsuarioId == PerfilLogged.UsuarioId)
                            {

                                #region CONTAINER EDITOR

                                var usuarioDoRecadoBloqueadoParaPostarNoUsuarioPost =
                                    _perfilBloqueadoService.ObterPerfilBloqueado(usuarioRecado.UsuarioId, PerfilLogged.UsuarioId);

                                // Verifica se o usuario esta bloqueado para postar bloqueado 0(Pode postar) e 1
                                // Sempre faz a verificação pois estão misturados posts de outros usuarios com os meus, logo os meus não precisam esta bloqueados
                                if (usuarioDoRecadoBloqueadoParaPostarNoUsuarioPost != null)
                                    v_html += " <div class='ed-cnt-cmt' bloqueado='1' >";
                                else
                                    v_html += " <div class='ed-cnt-cmt' bloqueado='0' >";

                                #region COMENTARIOS DO POST

                                var comentarios = _recadoComentarioService.ObterComentarios(recado.RecadoId, 1, FuncaoSite.TOTAL_COMENT_PAGE);

                                v_html += "<div class='ed-cnt-cmt-coments'>";

                                foreach (var comentario in comentarios)
                                {
                                    var usuarioComentario = comentario.Usuario;

                                    urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                                    v_html += @"
                                                <div class='cmt-cnt' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
                                                    <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                                            ";

                                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioComentario.Perfil.Alias);

                                    #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                                    urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                                    v_html += @"
                                                <div class='tooltip-dropmenu-html-container'>
                                                    <div class='tooltip'>
                                                        <span class='tooltip-content'>
                                                            <img src='" + urlResolve + @"' alt='' />
                                                            <span class='tooltip-text'>
                                                                <a class='tooltip-text-a' href='" + urlEncryptadaPerfil + "'>" + usuarioComentario.Perfil.Nome + " " + usuarioComentario.Perfil.Sobrenome + "</a><hr><span class='tooltip-text-span'>" + usuarioComentario.Perfil.FraseHtml + @"</span> 
                                                            </span>
                                                        </span>
                                                    </div>
                                                </div>
                                            ";

                                    #endregion

                                    v_html += "<div class='thecom'>";

                                    #region TOOLTIP MENU DE OPCOES

                                    if (usuarioComentario.UsuarioId == PerfilLogged.UsuarioId)
                                    {
                                        v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.RecadoComentarioId.ToString() + "' opcao='RCE'>" + UIConfig.ExcluirComentário + @"</div></a>";
                                    }
                                    else
                                    {
                                        v_html_menuitens = @"
                                                    <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.RecadoComentarioId.ToString() + @"' opcao='RCE'>" + UIConfig.ExcluirComentário + @"</div></a>
                                                    <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + usuarioComentario.UsuarioId.ToString() + @"' opcao='RCB'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
                                                ";
                                    }

                                    v_html += @"
                                                <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
                                                    <div class='button-group minor-group'>

                                                    <div class='tooltip-dropmenu'> 
                                                        <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                                        <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
                                                            <div class='tooltip-dropmenu-menu' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
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
                                                         <h5><a href='" + urlEncryptadaPerfil + "'>" + usuarioComentario.Perfil.Nome + " " + usuarioComentario.Perfil.Sobrenome + "</a></h5><span data-utime='1371248446' class='com-dt'>" + FuncaoSite.getTempoPost(comentario.DataComentario) + @"</span>
                                                         <div class='com-text'>" + comentario.ComentarioHtml + @"</div>
                                                     </div>
                                                </div><!-- end 'cmt-cnt' -->
                                            ";
                                }

                                var proximosComentarios = _recadoComentarioService.ObterComentarios(recado.RecadoId, nextPage, FuncaoSite.TOTAL_COMENT_PAGE).ToList();
                                if (proximosComentarios.Count > 0)
                                {
                                    v_html += @"
                                                <div class='ed-cnt-cmt-more button-group minor-group'>
                                                     <a href='javascript:void(0);' class='button minor1 btnVerMais' codrecado='" + recado.RecadoId.ToString() + @"' page='" + nextPage.ToString() + @"'>" + BTN_VER_MAIS_COMENTS + @"</a>
                                                </div>
                                            ";
                                }

                                v_html += "</div>";

                                #endregion

                                v_html += " </div><!-- end of comments container 'ed-cnt-cmt' -->";

                                #endregion

                            }

                            v_html += @"
                                    		    <a href='javascript:void(0);' class='tl-read-more'>Read more</a>
                                             <span class='tl-date'>há 29 segundos</span>
                                         </div><!-- tl-cnt-default-content -->
                                        </div><!-- tl-cnt-default-block -->
                                    ";

                            #endregion

                            if (usuarioRecebeuRecado.UsuarioId == PerfilLogged.UsuarioId)
                                _recadoService.MarcarComoLido(recado.RecadoId);
                        }

                        v_html += "</section>";

                        #endregion

                        #region BOTAO DO TOP

                        if (validacao.IsMeuPerfil())
                        {
                            v_html_btntop = @"
                                <div class='ed-cnt-cmt-btn'>
                                    <a href='javascript:void(0);' class='button big icon remove btnExcluirRecadoCheck'>" + UIConfig.ExcluirRecadosSelecionados + @"</a>
                                </div>
                            ";
                        }

                        #endregion

                        var proximosPosts = _recadoService.ObterRecadosEnviadosERecebidos(PerfilView.UsuarioId, nextPage, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                        #region BOTAO VER MAIS

                        if (proximosPosts.Count > 0)
                        {
                            urlResolve = Funcao.ResolveServerUrl("~/Images/cd-icon-location.svg", false);
                            v_html_btnvermais = @"<div class='tl-cnt-default-img-vermais cd-location btnVerMaisPost' page='" + nextPage.ToString() + @"'>
                                            <img src='" + urlResolve + @"' alt='Location'>
                                         </div>";
                        }

                        #endregion
                    }

                    #region TITULO

                    v_html_titulo = @"
                        <div class='ms-cnt-tit-blc'>
                            <h1 class='ms-tit-blc-txt icon-recado'>"+ UIConfig.MeusRecados +" <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                    #endregion

                    var perfilView = _perfilService.Obter(PerfilView.UsuarioId);

                    #region PAGINACAO

                    v_html_paginacao = FuncaoSite.getPaginacaoPerfil(PAGE_NUMBER, totalRecords, perfilView, FuncaoSite.NomePagina.RECADO);

                    #endregion
                }
                else
                {
                    #region TITULO

                    v_html_titulo = @"
                        <div class='ms-cnt-tit-blc'>
                            <h1 class='ms-tit-blc-txt icon-recado'>Recados</span></h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                    #endregion
                }

                #region EDITOR

                if ((priv.EscreverRecado == 0) || (priv.EscreverRecado == 1 && isAmigo))
                {
                    v_html_editor = "<div id='cmtRecadoFrame' class='ed-cnt-cmt'></div>";
                }

                #endregion

                _uow.Commit(); // Para leitura
            }

            object[] arrRetorno = new object[7];
            arrRetorno[0] = v_html_titulo;
            arrRetorno[1] = v_html;
            arrRetorno[2] = v_html_paginacao;
            arrRetorno[3] = v_html_editor;
            arrRetorno[4] = v_html_btntop;
            arrRetorno[5] = v_html_btnvermais;
            arrRetorno[6] = totalRecords;

            return Json(arrRetorno, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ajax/recado/comentario/template/view")]
        public ActionResult GetListRecadoComentarios(Guid recadoid, int page)
        {
            var v_html = string.Empty;
            var v_html_menuitens = string.Empty;

            var nextPage = page + 1;

            #region COMENTARIOS DO POST

            var comentarios = _recadoComentarioService.ObterComentarios(recadoid, page, FuncaoSite.TOTAL_COMENT_PAGE);

            foreach (var comentario in comentarios)
            {
                var usuarioComentario = comentario.Usuario;

                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                v_html += @"
                        <div class='cmt-cnt' codcoment='" + comentario.RecadoComentarioId.ToString() + @"' style='display:none;'>
                            <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                    ";

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioComentario.Perfil.Alias);

                #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                v_html += @"
                        <div class='tooltip-dropmenu-html-container'>
                            <div class='tooltip'>
                                <span class='tooltip-content'>
                                    <img src='" + urlResolve + @"' alt='' />
                                    <span class='tooltip-text'>
                                        <a class='tooltip-text-a' href='" + urlEncryptadaPerfil + "'>" + usuarioComentario.Perfil.Nome + " " + usuarioComentario.Perfil.Sobrenome + "</a><hr><span class='tooltip-text-span'>" + usuarioComentario.Perfil.FraseHtml + @"</span> 
                                    </span>
                                </span>
                            </div>
                        </div>
                    ";

                #endregion

                v_html += @"
                         <div class='thecom'>
                    ";

                #region TOOLTIP MENU DE OPCOES

                if (usuarioComentario.UsuarioId == PerfilLogged.UsuarioId)
                {
                    v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.RecadoComentarioId.ToString() + "' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>";
                }
                else
                {
                    v_html_menuitens = @"
                            <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.RecadoComentarioId.ToString() + @"' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>
                            <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + usuarioComentario.UsuarioId.ToString() + @"' opcao='B'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
                        ";
                }

                v_html += @"
                        <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
                            <div class='button-group minor-group'>

                            <div class='tooltip-dropmenu'> 
                                <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
                                    <div class='tooltip-dropmenu-menu' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
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
                                 <h5><a href='" + urlEncryptadaPerfil + "'>" + usuarioComentario.Perfil.Nome + " " + usuarioComentario.Perfil.Sobrenome + "</a></h5><span data-utime='1371248446' class='com-dt'>" + FuncaoSite.getTempoPost(comentario.DataComentario) + @"</span>
                                 <div class='com-text'>" + comentario.ComentarioHtml + @"</div>
                             </div>
                        </div><!-- end 'cmt-cnt' -->
                    ";
            }

            var proximosComentarios = _recadoComentarioService.ObterComentarios(recadoid, nextPage, FuncaoSite.TOTAL_COMENT_PAGE).ToList();
            if (proximosComentarios.Count > 0)
            {
                v_html += @"
                                <div class='ed-cnt-cmt-more button-group minor-group'>
                                        <a href='javascript:void(0);' class='button minor1 btnVerMais' codrecado='" + recadoid.ToString() + "' page='" + nextPage.ToString() + "'>" + BTN_VER_MAIS_COMENTS + @"</a>
                            ";

                if (page > 1)
                    v_html += @"<a href='javascript:void(0);' class='button minor1 btnVerMenos' codrecado='" + recadoid.ToString() + "' >" + BTN_VER_MENOS_COMENTS + "</a>";

                v_html += @"
                            </div>
                        ";
            }
            else if (page > 1)
            {
                v_html += @"
                                <div class='ed-cnt-cmt-more button-group minor-group'>
                                     <a href='javascript:void(0);' class='button minor1 btnVerMenos' codrecado='" + recadoid.ToString() + "' >" + BTN_VER_MENOS_COMENTS + @"</a>
                                </div>
                            ";
            }

            #endregion

            return Json(v_html);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _recadoService.Dispose();
            _recadoComentarioService.Dispose();
            _perfilBloqueadoService.Dispose();
            _privacidadeService.Dispose();
        }
    }
}