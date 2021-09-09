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
using System.Linq;
using System.Web.Mvc;

namespace Posy.V2.MVC.Controllers
{
    [Authorize]
    [ExceptionHandler]
    [UserCurrent]
    public class VideoController : BaseControllerPerfil
    {
        IVideoService _videoService;
        IVideoComentarioService _videoComentarioService;

        public VideoController(IUnitOfWork uow,
                               ICacheService cacheService,
                               IPerfilService perfilService,
                               IVideoService videoService,
                               IVideoComentarioService videoComentarioService,
                               IGlobalBaseController globalBaseController) :
            base(globalBaseController, uow, cacheService, perfilService)
        {
            _videoService = videoService;
            _videoComentarioService = videoComentarioService;
        }

        // GET: Video
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/video")]
        public ActionResult SalvarVideo(SalvarVideoModel model)
        {
            using (_uow)
            {
                var video = _videoService.SalvarVideo(model.Url, model.Titulo);
                _uow.Commit();

                #region TEMPLATE VIDEO

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;

                v_html += "<div class='video-container' codvideo='" + video.VideoId.ToString() + "'>";

                #region TOOLTIP MENU DE OPCOES

                v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codvideo='" + video.VideoId.ToString() + "' opcao='VE'>Excluir vídeo</div></a>";

                v_html += @"
                    <div class='popr tooltip-dropmenu-container' codvideo='" + video.VideoId.ToString() + @"'>
                        <div class='button-group minor-group'>

                        <div class='tooltip-dropmenu'> 
                            <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                            <div class='tooltip-dropmenu-html-container' codvideo='" + video.VideoId.ToString() + @"'>
                                <div class='tooltip-dropmenu-menu' codvideo='" + video.VideoId.ToString() + @"'>
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
                    <a href='javascript:void(0);' class='image-foto'>
                        <img class='gl-zoom-img' src='http://img.youtube.com/vi/" + video.Url + "/0.jpg' codvideo='" + video.VideoId.ToString() + "' videoid='" + video.Url + "' videotitle='" + video.NomeHtml + @"' />
                    </a>
                    <div class='video-caption'>
                        <h3>" + video.NomeHtml + @"</h3>
                    </div>
                ";

                #region COMENTARIOS DO VIDEO

                v_html += @"
                        <div class='ed-cnt-cmt-coments-temp'>
                    </div>
                ";

                #endregion

                v_html += "</div>";

                #endregion

                return Json(v_html);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/video/delete")]
        public ActionResult ExcluirVideo(ExcluirVideoModel model)
        {
            using (_uow)
            {
                _videoService.ExcluirVideo(model.VideoId);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/video/comentario")]
        public ActionResult SalvarVideoComentario(SalvarVideoComentarioModel model)
        {
            using (_uow)
            {
                var comentario = _videoComentarioService.Comentar(model.VideoId, model.Comentario);
                _uow.Commit();

                #region COMENTARIOS DO VIDEO

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;

                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + PerfilLogged.UsuarioId.ToString() + "/1.jpg", false);

                v_html += @"
                    <div class='cmt-cnt postado-recente' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
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

                v_html += @"
                    <div class='thecom'>
                ";

                #region TOOLTIP MENU DE OPCOES

                v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.VideoComentarioId.ToString() + "' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>";

                v_html += @"
                    <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
                        <div class='button-group minor-group'>

                            <div class='tooltip-dropmenu'> 
                                <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
                                    <div class='tooltip-dropmenu-menu' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
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
        [Route("ajax/video/comentario/delete")]
        public ActionResult ExcluirVideoComentario(ExcluirVideoComentarioModel model)
        {
            using (_uow)
            {
                _videoComentarioService.ExcluirComentario(model.VideoComentarioId);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpGet]
        [Route("ajax/video/template/view")]
        public ActionResult GetListVideos()
        {
            var validacao = new PerfilValidacao(PerfilLogged.UsuarioId, PerfilView.UsuarioId);

            var v_html = string.Empty;
            var v_html_menuitens = string.Empty;
            var v_html_titulo = string.Empty;
            var v_html_paginacao = string.Empty;
            var v_html_form = string.Empty;

            var nextPage = PAGE_NUMBER + 1;

            if (validacao.IsMeuPerfil() || validacao.IsAmigo())
            {
                int totalRecords;
                var videos = _videoService.ObterVideos(PerfilView.UsuarioId, PAGE_NUMBER, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                #region CONTAINER VIDEOS

                if (videos.Count > 0)
                {
                    v_html += "<div id='containerVideosView' class='album-video-container'>";

                    foreach (var video in videos)
                    {
                        var usuarioVideo = video.Usuario;

                        #region CONTAINER VIDEO

                        v_html += "<div class='video-container' codvideo='" + video.VideoId.ToString() + "'>";

                        #region TOOLTIP MENU DE OPCOES

                        if (validacao.IsMeuPerfil())
                        {
                            v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codvideo='" + video.VideoId.ToString() + "' opcao='EV'>Excluir vídeo</div></a>";

                            v_html += @"
                                    <div class='popr tooltip-dropmenu-container' codvideo='" + video.VideoId.ToString() + @"'>
                                        <div class='button-group minor-group'>

                                            <div class='tooltip-dropmenu'> 
                                                <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                                <div class='tooltip-dropmenu-html-container' codvideo='" + video.VideoId.ToString() + @"'>
                                                    <div class='tooltip-dropmenu-menu' codvideo='" + video.VideoId.ToString() + @"'>
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
                                <a href='javascript:void(0);' class='image-foto'>
                                    <img class='gl-zoom-img' src='http://img.youtube.com/vi/" + video.Url + "/0.jpg' codvideo='" + video.VideoId.ToString() + "' videoid='" + video.Url + "' videotitle='" + video.NomeHtml + @"' />
                                </a>
                                <div class='video-caption'>
                                    <h3>" + video.NomeHtml + @"</h3>
                                </div>
                            ";

                        #region COMENTARIOS DO VIDEO

                        var comentarios = _videoComentarioService.ObterComentarios(video.VideoId, PAGE_NUMBER, FuncaoSite.TOTAL_COMENT_PAGE).ToList();

                        v_html += "<div class='ed-cnt-cmt-coments-temp'>";

                        foreach (var comentario in comentarios)
                        {
                            var usuarioComentario = comentario.Usuario;

                            urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                            v_html += @"
                                    <div class='cmt-cnt' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
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
                                v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.VideoComentarioId.ToString() + "' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>";
                            }
                            else if (validacao.IsMeuPerfil())
                            {
                                v_html_menuitens = @"
                                    <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.VideoComentarioId.ToString() + @"' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>
                                    <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.VideoComentarioId.ToString() + "' codperfil='" + usuarioComentario.UsuarioId.ToString() + @"' opcao='B'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
                                ";
                            }

                            if (v_html_menuitens.ToString().Length > 0)
                            {
                                #region TOOLTIP MENU CONTAINER

                                v_html += @"
                                        <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
                                            <div class='button-group minor-group'>

                                            <div class='tooltip-dropmenu'>
                                                <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                                <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
                                                    <div class='tooltip-dropmenu-menu' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
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

                            v_html += @"
                                             <h5><a href='" + urlEncryptadaPerfil + "'>" + usuarioComentario.Perfil.Nome + " " + usuarioComentario.Perfil.Sobrenome + "</a></h5><span data-utime='1371248446' class='com-dt'>" + FuncaoSite.getTempoPost(comentario.DataComentario) + @"</span>
                                             <div class='com-text'>" + comentario.ComentarioHtml + @"</div>
                                         </div>
                                    </div><!-- end 'cmt-cnt' -->
                                ";
                        }

                        var proximosComentarios = _videoComentarioService.ObterComentarios(video.VideoId, nextPage, FuncaoSite.TOTAL_COMENT_PAGE).ToList();
                        if (proximosComentarios.Count > 0)
                        {
                            v_html += @"
                                    <div class='ed-cnt-cmt-more button-group minor-group'>
                                         <a href='javascript:void(0);' class='button minor1 btnVerMais' codvideo='" + video.VideoId.ToString() + "' page='" + nextPage.ToString() + "'>" + BTN_VER_MAIS_COMENTS + @"</a>
                                    </div>
                                ";
                        }

                        v_html += "</div>";

                        #endregion

                        v_html += "</div>";

                        #endregion
                    }

                    v_html += "</div>";

                    #region CONTAINER VIDEO VAZIO

                    //Apenas para mater o ultimo item alinhado a esquerda caso o numero de videos seja impar
                    v_html += @"
                            <div class='video-container video-container-invisible'>
                                <a href='javascript:void(0);' class='image-foto'>
                                    <img class='gl-zoom-img' src='' />
                                </a>
                                <div class='video-caption'>
                                    <h3>&nbsp;</h3>
                                </div>
                            </div>
                        ";

                    #endregion

                }

                #endregion

                #region TITULO

                v_html_titulo = @"
                        <div class='ms-cnt-tit-blc'>
                            <h1 class='ms-tit-blc-txt icon-video'>" + UIConfig.MeusVideos + " <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                #endregion

                var perfilView = _perfilService.Obter(PerfilView.UsuarioId);

                #region PAGINACAO

                v_html_paginacao = FuncaoSite.getPaginacaoPerfil(PAGE_NUMBER, totalRecords, perfilView, FuncaoSite.NomePagina.VIDEO);

                #endregion

                #region FORM

                if (validacao.IsMeuPerfil())
                {
                    v_html_form = @"
                            <div id='containerForm' class='fm-cnt-default'>

                                <div class='box-form'>
                                    <fieldset>
                                        <div class='half-width' style='width:100%;'>
                                            <label for='txtUrlVideo'>"+ UIConfig.UrlDoVideo + @"</label>
                                            <input type='text' class='txtUrlVideo obrigatorio' name='txtUrlVideo' maxlength='100' />
                                        </div>
                                    </fieldset>
                                </div>

                                <div class='ed-cnt-cmt-btn'>
                                    <a href='javascript:void(0);' class='button big icon approve primary btnSalvarVideo'>" + UIConfig.SalvarVideo + @"</a>
                                </div>

                            </div>
                        ";
                }

                #endregion
            }
            else
            {
                #region TITULO

                v_html_titulo = @"
                        <div class='ms-cnt-tit-blc'>
                            <h1 class='ms-tit-blc-txt icon-video'>Vídeos</h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                #endregion
            }

            object[] arrRetorno = new object[4];
            arrRetorno[0] = v_html_titulo;
            arrRetorno[1] = v_html;
            arrRetorno[2] = v_html_paginacao;
            arrRetorno[3] = v_html_form;

            return Json(arrRetorno, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ajax/video/comentario/template/view")]
        public ActionResult GetListVideoComentarios(Guid videoid, int page)
        {
            var v_html = string.Empty;
            var v_html_menuitens = string.Empty;

            var nextPage = page + 1;

            #region COMENTARIOS DO POST

            var comentarios = _videoComentarioService.ObterComentarios(videoid, page, FuncaoSite.TOTAL_COMENT_PAGE).ToList();

            foreach (var comentario in comentarios)
            {
                var usuarioComentario = comentario.Usuario;

                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                v_html += @"
                        <div class='cmt-cnt' codcoment='" + comentario.VideoComentarioId.ToString() + @"' style='display:none;'>
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
                    v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.VideoComentarioId.ToString() + "' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>";
                }
                else
                {
                    v_html_menuitens = @"
                        <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.VideoComentarioId.ToString() + @"' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>
                        <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.VideoComentarioId.ToString() + @"' codperfil='" + usuarioComentario.UsuarioId.ToString() + @"' opcao='B'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
                    ";
                }

                v_html += @"
                        <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
                            <div class='button-group minor-group'>

                            <div class='tooltip-dropmenu'>
                                <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
                                    <div class='tooltip-dropmenu-menu' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
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

            var proximosComentarios = _videoComentarioService.ObterComentarios(videoid, nextPage, FuncaoSite.TOTAL_COMENT_PAGE).ToList();
            if (proximosComentarios.Count > 0)
            {
                v_html += @"
                        <div class='ed-cnt-cmt-more button-group minor-group'>
                            <a href='javascript:void(0);' class='button minor1 btnVerMais' codvideo='" + videoid.ToString() + "' page='" + nextPage.ToString() + "'>" + BTN_VER_MAIS_COMENTS + @"</a>
                    ";

                if (page > 1)
                    v_html += @"<a href='javascript:void(0);' class='button minor1 btnVerMenos' codvideo='" + videoid.ToString() + "' >" + BTN_VER_MENOS_COMENTS + "</a>";

                v_html += @"
                        </div>
                    ";
            }
            else if (page > 1)
            {
                v_html += @"
                            <div class='ed-cnt-cmt-more button-group minor-group'>
                                 <a href='javascript:void(0);' class='button minor1 btnVerMenos' codvideo='" + videoid.ToString() + "' >" + BTN_VER_MENOS_COMENTS + @"</a>
                            </div>
                        ";
            }

            #endregion

            return Json(v_html, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _videoService.Dispose();
            _videoComentarioService.Dispose();
        }
    }
}