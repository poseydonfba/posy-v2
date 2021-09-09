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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Posy.V2.MVC.Controllers
{
    [RequireHttps]
    [Authorize]
    [ExceptionHandler]
    [UserCurrent]
    public class InicioController : BaseControllerPerfil
    {
        IPostPerfilService _postService;
        IPostPerfilBloqueadoService _perfilBloqueadoService;
        IPostPerfilComentarioService _postPerfilComentarioService;
        IPostOcultoService _postOcultoService;
        IAmizadeService _amizadeService;
        IMembroService _membroService;

        public InicioController(IUnitOfWork uow,
                                ICacheService cacheService,
                                IPerfilService perfilService,
                                IPostPerfilService postService,
                                IPostPerfilComentarioService postPerfilComentarioService,
                                IPostPerfilBloqueadoService perfilBloqueadoService,
                                IPostOcultoService postOcultoService,
                                IAmizadeService amizadeService,
                                IMembroService membroService,
                                IGlobalBaseController globalBaseController) :
            base(globalBaseController, uow, cacheService, perfilService)
        {
            _postService = postService;
            _perfilBloqueadoService = perfilBloqueadoService;
            _postPerfilComentarioService = postPerfilComentarioService;
            _postOcultoService = postOcultoService;
            _amizadeService = amizadeService;
            _membroService = membroService;
        }

        // GET: Inicio
        //[Route("")]

        //[ExcludeFromAntiForgeryValidation]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/inicio/post")]
        public ActionResult SalvarPost(PostPerfilModel model)
        {
            var v_html_view = string.Empty;
            var v_html_btntop = string.Empty;
            var v_html_menuitens = string.Empty;

            PostPerfil post = null;

            using (_uow)
            {
                post = _postService.Postar(model.PostHtml);
                _uow.Commit();
            }

            #region TEMPLATE POST

            urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + PerfilLogged.Id.ToString() + @"/1.jpg", false);

            v_html_view += @"
                    <div class='tl-cnt-default-block postado-recente' postId='" + post.Id.ToString() + @"' codperfil='" + PerfilLogged.Id.ToString() + @"'>

                        <div class='tl-cnt-default-img cd-picture'>
                            <img src='" + urlResolve + @"' alt='Picture' />
                        </div> <!-- tl-cnt-default-img -->

                        <div class='tl-cnt-default-content'>
                ";

            #region TOOLTIP MENU DE OPCOES

            v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' postId='" + post.Id.ToString() + "' opcao='E'>" + UIConfig.ExcluirPostagem + @"</div></a>";

            v_html_view += @"
                    <div class='popr tooltip-dropmenu-container' postId='" + post.Id.ToString() + @"'>
                        <div class='button-group minor-group'>

                        <div class='tooltip-dropmenu'>
                            <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                            <div class='tooltip-dropmenu-html-container' postId='" + post.Id.ToString() + @"'>
                                <div class='tooltip-dropmenu-menu' postId='" + post.Id.ToString() + @"'>
                                    " + v_html_menuitens + @"
                                </div>
                            </div>
                            <div class='clear'></div>
                        </div>

                        </div>
                    </div>
                ";

            #endregion

            v_html_view += @"<input type='checkbox' class='tl-checkbox' value='" + post.Id.ToString() + "' />";

            urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(PerfilLogged.Alias);

            v_html_view += @"
                            <h2 class='tl-h2'><a href='" + urlEncryptadaPerfil + @"'>" + PerfilLogged.Nome + " " + PerfilLogged.Sobrenome + @"</a><span data-utime='1371248446' class='post-dt'>" + FuncaoSite.getTempoPost(DateTime.Now) + @"</span></h2>
                            <h3 class='tl-h3'><span>" + PerfilLogged.FraseHtml + @"</span></h3>
                            <div class='tl-text'>" + post.PostHtml + @"</div>

                            <div class='ed-cnt-cmt' >
                                <div class='ed-cnt-cmt-coments'>

                                </div><!-- end of 'ed-cnt-cmt-coments' -->
                            </div><!-- end of comments container 'ed-cnt-cmt' -->

                            <a href='javascript:void(0);' class='tl-read-more'>Read more</a>
                            <span class='tl-date'>há 29 segundos</span>
                        </div><!-- tl-cnt-default-content -->
                    </div><!-- tl-cnt-default-block -->
                ";

            #endregion

            #region BOTAO VER MAIS

            v_html_btntop = @"<div class='ed-cnt-cmt-btn'>
                                    <a href='javascript:void(0);' class='button big icon remove btnExcluirPostCheck'>" + UIConfig.ExcluirPostagensSelecionadas + @"</a>
                                </div>";

            #endregion

            object[] arrRetorno = new object[2];
            arrRetorno[0] = v_html_view;
            arrRetorno[1] = v_html_btntop;

            return Json(arrRetorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/inicio/post/delete")]
        public ActionResult ExcluirPost(ExcluirPostPerfilModel model)
        {
            using (_uow)
            {
                _postService.ExcluirPost(model.PostId);
                _uow.Commit();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/inicio/post/delete/multiple")]
        public ActionResult ExcluirMultiplosPost(List<ExcluirPostPerfilModel> model)
        {
            using (_uow)
            {
                foreach (var post in model)
                    _postService.ExcluirPost(post.PostId);

                _uow.Commit();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/inicio/post/ocultar")]
        public ActionResult OcultarPost(OcultarPostPerfilModel model)
        {
            using (_uow)
            {
                _postOcultoService.OcultarPost(model.PostId);
                _uow.Commit();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/inicio/post/bloquear")]
        public ActionResult BloquearPost(BloquearPerfilModel model)
        {
            using (_uow)
            {
                _perfilBloqueadoService.BloquearPostPerfil(model.UsuarioIdBloqueado);
                _uow.Commit();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/inicio/comentario")]
        public ActionResult SalvarPostComentario(ComentarioPerfilModel model)
        {
            PostPerfilComentario comentario = null;

            using (_uow)
            {
                comentario = _postPerfilComentarioService.Comentar(model.PostId, model.Comentario);
                _uow.Commit();
            }

            #region TEMPLATE COMENTARIO

            var v_html_view = string.Empty;
            var v_html_menuitens = string.Empty;

            urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + PerfilLogged.Id.ToString() + @"/1.jpg", false);

            v_html_view += @"
                            <div class='cmt-cnt' codcoment='" + comentario.Id.ToString() + @"' style='display:none;'>

                                <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                        ";

            #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

            urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(PerfilLogged.Alias);
            urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + PerfilLogged.Id.ToString() + @"/1.jpg", false);

            v_html_view += @"
                            <div class='tooltip-dropmenu-html-container'>
                                <div class='tooltip'>
                                    <span class='tooltip-content'>
                                        <img src='" + urlResolve + @"' alt='' />
                                        <span class='tooltip-text'>
                                            <a class='tooltip-text-a' href='" + urlEncryptadaPerfil + @"'>" + PerfilLogged.Nome + " " + PerfilLogged.Sobrenome + "</a><hr><span class='tooltip-text-span'>" + PerfilLogged.FraseHtml + @"</span> 
                                        </span>
                                    </span>
                                </div>
                            </div>
                        ";

            #endregion

            v_html_view += @"<div class='thecom'>";

            #region TOOLTIP MENU DE OPCOES

            v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.Id.ToString() + "' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>";

            v_html_view += @"
                            <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.Id.ToString() + @"'>
                                <div class='button-group minor-group'>

                                    <div class='tooltip-dropmenu'>
                                        <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                        <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.Id.ToString() + @"'>
                                            <div class='tooltip-dropmenu-menu' codcoment='" + comentario.Id.ToString() + @"'>
                                            " + v_html_menuitens + @"
                                            </div>
                                        </div>
                                        <div class='clear'></div>
                                    </div>

                                </div>
                            </div>
                        ";

            #endregion

            v_html_view += @"
                                    <h5><a href='" + urlEncryptadaPerfil + "'>" + PerfilLogged.Nome + " " + PerfilLogged.Sobrenome + @"</a></h5><span data-utime='1371248446' class='com-dt'>" + FuncaoSite.getTempoPost(DateTime.Now) + @"</span>
                                    <div class='com-text'>" + comentario.ComentarioHtml + @"</div>
                                </div>
                            </div><!-- end 'cmt-cnt' -->
                        ";

            #endregion

            return Json(v_html_view, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/inicio/comentario/delete")]
        public ActionResult ExcluirPostComentario(ExcluirComentarioPerfilModel model)
        {
            using (_uow)
            {
                _postPerfilComentarioService.ExcluirComentario(model.ComentarioId);
                _uow.Commit();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ajax/inicio/feed")]
        [OutputCache(Duration = 5, VaryByParam = "page")]
        public ActionResult GetListInicioAtualizacao(int page)
        {
            var posts = _postService.ObterPosts(PerfilLogged.Id, page, FuncaoSite.TOTAL_POST_PAGE).ToList();

            var v_html = string.Empty;
            var v_html_menuitens = string.Empty;
            var v_html_btntop = string.Empty;
            var v_html_editor = string.Empty;
            var v_html_btnvermais = string.Empty;

            var totalPostsParaVer = 0;
            var nextPage = page + 1;

            if (posts.Count > 0)
            {
                #region TIMELINE

                if (page == 1)
                    v_html += @"<section class='tl-cnt-default'>";

                foreach (var post in posts)
                {
                    var usuarioPost = post.Usuario;
                    var postId = post.Id.ToString();

                    urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioPost.Id.ToString() + @"/1.jpg", false);

                    #region HTML

                    v_html += @"
                                <div class='tl-cnt-default-block' postId='" + postId + @"' codperfil='" + usuarioPost.Id.ToString() + @"' style='display:none;'>

                                    <div class='tl-cnt-default-img cd-picture'>
                                        <img src='" + urlResolve + @"' alt='Picture' />
                                    </div> <!-- tl-cnt-default-img -->

                                    <div class='tl-cnt-default-content'>
                            ";

                    #endregion

                    #region TOOLTIP MENU DE OPCOES

                    if (post.UsuarioId == PerfilLogged.Id)
                    {
                        v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' postId='" + postId + "' opcao='E'>" + UIConfig.ExcluirPostagem + @"</div></a>";
                    }
                    else
                    {
                        v_html_menuitens = @"
                                <a href='javascript:void(0);'><div class='menuitem btnMenuItem' postId='" + postId + @"' opcao='O'>" + UIConfig.OcultarPostagem + @"</div></a>
                                <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + usuarioPost.Id.ToString() + @"' opcao='B'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
                            ";
                    }

                    v_html += @"
                            <div class='popr tooltip-dropmenu-container' postId='" + postId + @"'>
                                <div class='button-group minor-group'>
                                    <div class='tooltip-dropmenu'>
                                        <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                        <div class='tooltip-dropmenu-html-container' postId='" + postId + @"'>
                                            <div class='tooltip-dropmenu-menu' postId='" + postId + @"'>
                                            " + v_html_menuitens + @"
                                            </div>
                                        </div>
                                        <div class='clear'></div>
                                    </div>
                                </div>
                            </div>
                        ";

                    #endregion

                    if (usuarioPost.Id == PerfilLogged.Id)
                        v_html += @"<input type='checkbox' class='tl-checkbox' value='" + postId + "' />";

                    //v_html.Append("     <iframe class='tl-text' src='data:text/html;charset=utf-8," + objeto.Post + "'></iframe>");

                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioPost.Perfil.Alias);

                    #region HTML

                    v_html += @"
                            <h2 class='tl-h2'><a href='" + urlEncryptadaPerfil + @"'>" + usuarioPost.Perfil.Nome + " " + usuarioPost.Perfil.Sobrenome + @"</a><span data-utime='1371248446' class='post-dt'>" + FuncaoSite.getTempoPost(post.DataPost) + @"</span></h2>
                            <h3 class='tl-h3'><span>" + usuarioPost.Perfil.FraseHtml + @"</span></h3>
                            <div class='tl-text' >" + post.PostHtml + @"</div>
                        ";

                    #endregion

                    #region CONTAINER EDITOR

                    var usuarioLogadoBloqueadoParaPostarNoUsuarioPost =
                        _perfilBloqueadoService.ObterPerfilBloqueado(usuarioPost.Id, PerfilLogged.Id);

                    // Verifica se o usuario esta bloqueado para postar bloqueado 0(Pode postar) e 1
                    // Sempre faz a verificação pois estão misturados posts de outros usuarios com os meus, logo os meus não precisam esta bloqueados
                    if (usuarioLogadoBloqueadoParaPostarNoUsuarioPost != null)
                        v_html += @"<div class='ed-cnt-cmt' bloqueado='1' >";
                    else
                        v_html += @"<div class='ed-cnt-cmt' bloqueado='0' >";

                    #region COMENTARIOS DO POST

                    var comentarios = _postPerfilComentarioService.ObterComentarios(post.Id, 1, FuncaoSite.TOTAL_COMENT_PAGE);

                    v_html += @"<div class='ed-cnt-cmt-coments'>";

                    var ultimo_codcoment = Guid.Empty;

                    foreach (var comentario in comentarios)
                    {
                        var usuarioComentario = comentario.Usuario;

                        urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioComentario.Id.ToString() + @"/1.jpg", false);

                        v_html += @"<div class='cmt-cnt' codcoment='" + comentario.Id.ToString() + @"'>

                                                    <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />";

                        #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                        urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioComentario.Perfil.Alias);
                        urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioComentario.Id.ToString() + @"/1.jpg", false);

                        v_html += @"
                                    <div class='tooltip-dropmenu-html-container'>
                                        <div class='tooltip'>
                                            <span class='tooltip-content'>
                                                <img src='" + urlResolve + @"' alt='' />
                                                <span class='tooltip-text'>
                                                    <a class='tooltip-text-a' href='" + urlEncryptadaPerfil + @"'>" + usuarioComentario.Perfil.Nome + @" " + usuarioComentario.Perfil.Sobrenome + @"</a><hr><span class='tooltip-text-span'>" + usuarioComentario.Perfil.FraseHtml + @"</span> 
                                                </span>
                                            </span>
                                        </div>
                                    </div>
                                ";

                        #endregion

                        v_html += @"<div class='thecom'>";

                        #region TOOLTIP MENU DE OPCOES


                        if (usuarioComentario.Id == PerfilLogged.Id)
                        {
                            v_html_menuitens = @"<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.Id.ToString() + "' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>";
                        }
                        else
                        {
                            v_html_menuitens = @"
                                        <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.Id.ToString() + @"' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>
                                        <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + usuarioComentario.Id.ToString() + @"' opcao='B'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
                                    ";
                        }

                        v_html += @"
                                    <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.Id.ToString() + @"'>
                                        <div class='button-group minor-group'>

                                            <div class='tooltip-dropmenu'> 
                                                <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                                <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.Id.ToString() + @"'>
                                                    <div class='tooltip-dropmenu-menu' codcoment='" + comentario.Id.ToString() + @"'>
                                                    " + v_html_menuitens + @"
                                                    </div>
                                                </div>
                                                <div class='clear'></div>
                                            </div>

                                        </div>
                                    </div>
                                ";

                        v_html_menuitens.Remove(0, v_html_menuitens.ToString().Length);


                        #endregion

                        v_html += @"
                                            <h5><a href='" + urlEncryptadaPerfil + @"'>" + usuarioComentario.Perfil.Nome + " " + usuarioComentario.Perfil.Sobrenome + @"</a></h5><span data-utime='1371248446' class='com-dt'>" + FuncaoSite.getTempoPost(comentario.Data) + @"</span>

                                            <div class='com-text'>" + comentario.ComentarioHtml + @"</div>

                                        </div>
                                    </div><!-- end 'cmt-cnt' -->
                                ";
                    }

                    var proximosComentarios = _postPerfilComentarioService.ObterComentarios(post.Id, 2, FuncaoSite.TOTAL_COMENT_PAGE).ToList();
                    if (proximosComentarios.Count > 0)
                    {
                        v_html += @"
                                <div class='ed-cnt-cmt-more button-group minor-group'>
                                    <a href='javascript:void(0);' class='button minor1 btnVerMais' postId='" + post.Id.ToString() + @"' page='" + nextPage.ToString() + @"'>" + BTN_VER_MAIS_COMENTS + @"</a>
                                </div>
                                  ";
                    }

                    v_html += @"</div>";


                    #endregion

                    v_html += @"</div><!-- end of comments container 'ed-cnt-cmt' -->";


                    #endregion

                    #region HTML

                    v_html += @"
                                    <a href='javascript:void(0);' class='tl-read-more'>Read more</a>
                                    <span class='tl-date'>há 29 segundos</span>
                                </div><!-- tl-cnt-default-content -->
                            </div><!-- tl-cnt-default-block -->
                        ";

                    #endregion
                }

                if (page == 1)
                    v_html += @"</section>";

                #endregion

                #region BOTAO DO TOPO

                if (page == 1)
                    v_html_btntop = @"<div class='ed-cnt-cmt-btn'>
                                        <a href='javascript:void(0);' class='button big icon remove btnExcluirPostCheck'>" + UIConfig.ExcluirPostagensSelecionadas + @"</a>
                                    </div>";

                #endregion

                var proximosPosts = _postService.ObterPosts(PerfilLogged.Id, nextPage, FuncaoSite.TOTAL_POST_PAGE).ToList();

                totalPostsParaVer = proximosPosts.Count;

                #region BOTAO VER MAIS

                if (totalPostsParaVer > 0)
                {
                    urlResolve = Funcao.ResolveServerUrl("~/Images/cd-icon-location.svg", false);
                    v_html_btnvermais = @"<div class='tl-cnt-default-img-vermais cd-location btnVerMaisPost' page='" + nextPage.ToString() + @"'>
                                            <img src='" + urlResolve + @"' alt='Location'>
                                         </div>";
                }

                #endregion
            }

            #region EDITOR

            v_html_editor = @"<div id='cmtInicioPessoal' class='ed-cnt-cmt'></div>";

            #endregion

            object[] arrRetorno = new object[7];
            arrRetorno[0] = "";
            arrRetorno[1] = v_html;
            arrRetorno[2] = "";
            arrRetorno[3] = v_html_editor;
            arrRetorno[4] = v_html_btntop;
            arrRetorno[5] = v_html_btnvermais;
            arrRetorno[6] = totalPostsParaVer;

            return Json(arrRetorno, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ajax/inicio/post/comentarios")]
        public ActionResult GetListPostComentarios(int postid, int page)
        {
            string v_html = string.Empty;
            string v_html_menuitens = string.Empty;

            var nextPage = page + 1;

            var comentarios = _postPerfilComentarioService.ObterComentarios(postid, page, FuncaoSite.TOTAL_COMENT_PAGE);

            #region COMENTARIOS DO POST

            foreach (var comentario in comentarios)
            {
                var usuarioComentario = comentario.Usuario;

                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioComentario.Id.ToString() + "/1.jpg", false);

                v_html += @"
                        <div class='cmt-cnt' codcoment='" + comentario.Id.ToString() + @"' style='display:none;'>
                            <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                    ";

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioComentario.Perfil.Alias);

                #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioComentario.Id.ToString() + "/1.jpg", false);

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

                if (usuarioComentario.Id == PerfilLogged.Id)
                {
                    v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.Id.ToString() + "' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>";
                }
                else
                {
                    v_html_menuitens = @"
                            <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.Id.ToString() + @"' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>
                            <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + usuarioComentario.Id.ToString() + @"' opcao='B'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
                        ";
                }

                v_html += @"
                        <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.Id.ToString() + @"'>
                            <div class='button-group minor-group'>

                            <div class='tooltip-dropmenu'> 
                                <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.Id.ToString() + @"'>
                                    <div class='tooltip-dropmenu-menu' codcoment='" + comentario.Id.ToString() + @"'>
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
                                 <h5><a href='" + urlEncryptadaPerfil + "'>" + usuarioComentario.Perfil.Nome + " " + usuarioComentario.Perfil.Sobrenome + "</a></h5><span data-utime='1371248446' class='com-dt'>" + FuncaoSite.getTempoPost(comentario.Data) + @"</span>
                                 <div class='com-text'>" + comentario.ComentarioHtml + @"</div>
                             </div>
                        </div><!-- end 'cmt-cnt' -->
                    ";
            }

            var proximosComentarios = _postPerfilComentarioService.ObterComentarios(postid, nextPage, FuncaoSite.TOTAL_COMENT_PAGE).ToList();
            if (proximosComentarios.Count > 0)
            {
                v_html += @"
                                <div class='ed-cnt-cmt-more button-group minor-group'>
                                        <a href='javascript:void(0);' class='button minor1 btnVerMais' postId='" + postid.ToString() + "' page='" + nextPage.ToString() + "'>" + BTN_VER_MAIS_COMENTS + @"</a>
                            ";

                if (page > 1)
                    v_html += @"<a href='javascript:void(0);' class='button minor1 btnVerMenos' postId='" + postid.ToString() + "' >" + BTN_VER_MENOS_COMENTS + "</a>";

                v_html += @"
                            </div>
                        ";
            }
            else if (page > 1)
            {
                v_html += @"
                                <div class='ed-cnt-cmt-more button-group minor-group'>
                                     <a href='javascript:void(0);' class='button minor1 btnVerMenos' postId='" + postid.ToString() + "' >" + BTN_VER_MENOS_COMENTS + @"</a>
                                </div>
                            ";
            }

            #endregion

            return Json(v_html.ToString(), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _postService.Dispose();
            _perfilBloqueadoService.Dispose();
            _postPerfilComentarioService.Dispose();
            _postOcultoService.Dispose();
        }
    }
}