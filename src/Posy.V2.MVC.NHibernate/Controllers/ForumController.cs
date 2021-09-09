using Posy.V2.Domain.Entities;
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
using System;
using System.Linq;
using System.Web.Mvc;

namespace Posy.V2.MVC.Controllers
{
    [Authorize]
    [ExceptionHandler]
    [UserCurrent]
    public class ForumController : BaseControllerComunidade
    {
        IMembroService _membroService;
        ITopicoService _topicoService;
        ITopicoPostService _topicoPostService;
        IModeradorService _moderadorService;

        public ForumController(IUnitOfWork uow,
                                ICacheService cacheService,
                                IPerfilService perfilService,
                                IComunidadeService comunidadeService,
                                IMembroService membroService,
                                ITopicoService topicoService,
                                ITopicoPostService topicoPostService,
                                IModeradorService moderadorService,
                                IGlobalBaseController globalBaseController) :
            base(globalBaseController, uow, cacheService, perfilService, comunidadeService)
        {
            _membroService = membroService;
            _topicoService = topicoService;
            _topicoPostService = topicoPostService;
            _moderadorService = moderadorService;
        }

        // GET: Forum
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/topico")]
        public ActionResult SalvarTopico(SalvarTopicoModel model)
        {
            using (_uow)
            {
                var topico = _topicoService.SalvarTopico(ComunidadeView.Id, model.Titulo, model.Descricao, model.Fixo);
                _uow.Commit();

                #region HTML DE RETORNO

                var v_html = string.Empty;
                var perfilDonoTpc = PerfilLogged;
                var cmm = ComunidadeView;

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilDonoTpc.Alias);
                urlEncryptadaCmm = FuncaoSite.getUrlNomeIdCmm(cmm.Alias) + "/" + FuncaoSite.ROUTE_URL_TOPICO + "/" + topico.Id.ToString();
                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfilDonoTpc.Id.ToString() + "/1.jpg", false);

                v_html = @"
                    <li class='list-li'>
                        <div class='irultimopost'><a href='" + urlEncryptadaCmm + @"' class='lastpost'>" + UIConfig.VerUltimo + @"</a></div>
                        < div class='ultimopost'><span>"+ UIConfig.UltimoPost + "</span>" + FuncaoSite.getTempoPost(DateTime.Now) + @"</div>
                        <div class='totalpost'><span>" + UIConfig.Posts.ToLower() + @"</span>0</div>
                        < div class='autorpost'><span>" + UIConfig.Autor.ToLower() + @"</span><a href='" + urlEncryptadaPerfil + "' class='lastpost'>" + perfilDonoTpc.Nome + @"</a></div>
                        <img class='list-img' src='" + urlResolve + @"'>
                        <h3 class='list-h3'><a href='" + urlEncryptadaCmm + "'>" + topico.Titulo + @"</a></h3>
                        <p class='list-p'>&nbsp;</p>
                    </li>
                ";

                #endregion

                return Json(v_html);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/topico/post")]
        public ActionResult SalvarTopicoPost(SalvarTopicoPostModel model)
        {
            using (_uow)
            {
                var post = _topicoPostService.SalvarPost(TOPICO_NUMBER, model.Descricao);
                _uow.Commit();

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;
                var autorizacaoComu = string.Empty;

                var perfilDonoTpc = PerfilLogged;
                var cmm = ComunidadeView;

                var validacao = new CmmValidacao(PerfilLogged, ComunidadeView);

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilDonoTpc.Alias);
                urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfilDonoTpc.Id.ToString() + "/1.jpg", false);

                v_html = @"<li class='list-li postado-recente' style='display:none;' codpost='" + post.Id.ToString() + "' cp='" + perfilDonoTpc.Id.ToString() + "'>";

                #region TOOLTIP MENU DE OPCOES

                #region ITENS MENU

                v_html_menuitens = string.Empty;

                if (validacao.IsDono || validacao.IsModerador)
                {
                    v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codpost='" + post.Id.ToString() + "' opcao='E'>"+ UIConfig.ExcluirPostagem +@"</div></a>";
                }

                autorizacaoComu = string.Empty;

                if (validacao.IsDono)
                {
                    autorizacaoComu = "<span class='cmm-dono'>"+ UIConfig.DonoDaComunidade +@"</span>";
                }
                else if (validacao.IsModerador)
                {
                    autorizacaoComu = "<span class='cmm-mode'>" + UIConfig.ModeradorDaComunidade + @"</span>";
                }

                #endregion

                if (v_html_menuitens.Length > 0)
                {
                    v_html += @"
                        <div class='popr tooltip-dropmenu-container' codpost='" + post.Id.ToString() + @"'>
                            <div class='button-group minor-group'>

                                <div class='tooltip-dropmenu'>
                                    <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                    <div class='tooltip-dropmenu-html-container' codpost='" + post.Id.ToString() + @"'>
                                        <div class='tooltip-dropmenu-menu' codpost='" + post.Id.ToString() + @"'>
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
                        <img class='list-img' src='" + urlResolve + @"'>
                        <h3 class='list-h3'><a href='" + urlEncryptadaPerfil + "'>" + perfilDonoTpc.Nome + " " + perfilDonoTpc.Sobrenome + "</a><span class='list-span-dt'>" + FuncaoSite.getTempoPost(DateTime.Now) + @"</span></h3>
                        " + autorizacaoComu + @"
                        <div class='list-p'>" + post.DescricaoHtml + @"</div>
                    </li>
                ";

                return Json(v_html);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajax/topico/post/delete")]
        public ActionResult ExcluirTopicoPost(ExcluirTopicoPostModel model)
        {
            using (_uow)
            {
                _topicoPostService.ExcluirTopicoPost(model.PostId);
                _uow.Commit();
            }
            return Json("");
        }

        [HttpGet]
        [Route("ajax/forum/template/view")]
        public ActionResult getCmmTopicos()
        {
            var validacao = new CmmValidacao(PerfilLogged, ComunidadeView);

            var v_html = string.Empty;
            var v_html_form = string.Empty;
            var v_html_titulo = string.Empty;
            var v_html_paginacao = string.Empty;

            int v_acesso = 0;

            if (validacao.IsDono || validacao.IsModerador || validacao.IsMembro || validacao.IsPublica)
            {
                int totalRecords;
                var topicos = _topicoService.ObterTopicos(ComunidadeView.Id, PAGE_NUMBER, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                int totalPosts, ultimaPagina;

                #region TOPICOS

                v_html = "<div class='ls-cnt-default poseydon-forum-list-size'>";

                #region HTML UL

                v_html += "<ul class='list-ul'>";

                foreach (var topico in topicos)
                {
                    var perfilDonoTpc = topico.Usuario.Perfil;

                    TopicoPost ultimoPost;
                    var temp = _topicoPostService.ObterPosts(topico.Id, 1, 0, out totalPosts, out ultimoPost);

                    int totalPaginas = (totalPosts / FuncaoSite.TOTAL_POST_PAGE),
                        restoDiv = (totalPosts % FuncaoSite.TOTAL_POST_PAGE);

                    totalPaginas = (restoDiv > 0) ? (totalPaginas + 1) : totalPaginas;

                    ultimaPagina = (totalPaginas == 0) ? 1 : FuncaoSite.SetInicioFimPagina(1, totalPaginas)[1];

                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilDonoTpc.Alias);
                    urlEncryptadaCmm = FuncaoSite.getUrlNomeIdCmm(ComunidadeView.Alias) + "/" + FuncaoSite.ROUTE_URL_TOPICO + "/" + topico.Id + "/" + ultimaPagina;

                    #region HTML LI

                    v_html += @"
                        <li class='list-li'>
                        <div class='irultimopost'><a href='" + urlEncryptadaCmm + @"' class='lastpost'>" + UIConfig.VerUltimo + @"</a></div>
                    ";

                    if (ultimoPost == null)
                        v_html += "<div class='ultimopost'><span>"+ UIConfig.UltimoPost +"</span>-</div>";
                    else
                        v_html += "<div class='ultimopost'><span>" + UIConfig.UltimoPost + "</span>" + FuncaoSite.getTempoPost(ultimoPost.DataPost) + "</div>";

                    v_html += @"
                        <div class='totalpost'><span>posts</span>" + totalPosts.ToString() + @"</div>
                        <div class='autorpost'><span>" + UIConfig.Autor + "</span><a href='" + urlEncryptadaPerfil + "' class='lastpost'>" + perfilDonoTpc.Nome + @"</a></div>
                    ";

                    if (ultimoPost == null)
                    {
                        urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfilDonoTpc.Id.ToString() + "/1.jpg", false);
                        v_html += "<img class='list-img' src='" + urlResolve + "'><div class='seta'></div>";
                    }
                    else
                    {
                        urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + ultimoPost.UsuarioId.ToString() + "/1.jpg", false);
                        v_html += "<img class='list-img' src='" + urlResolve + "'><div class='seta'></div>";
                    }

                    urlEncryptadaCmm = FuncaoSite.getUrlNomeIdCmm(ComunidadeView.Alias) + "/" + FuncaoSite.ROUTE_URL_TOPICO + "/" + topico.Id.ToString();

                    v_html += @"
                            <h3 class='list-h3'><a href='" + urlEncryptadaCmm + "'>" + topico.Titulo + @"</a></h3>
                            <p class='list-p'>&nbsp;</p>
                        </li>
                    ";

                    #endregion
                }

                v_html += "</ul>";

                #endregion

                v_html += "</div>";

                #endregion

                #region TITULO

                v_html_titulo = @"
                    <div class='ms-cnt-tit-blc'>
                        <h1 class='ms-tit-blc-txt icon-cmm'>" + UIConfig.Forum + @" <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                        <div class='ms-tit-blc-line'></div>
                    </div>
                ";

                #endregion

                #region PAGINACAO

                v_html_paginacao = FuncaoSite.getPaginacaoCmm(PAGE_NUMBER, totalRecords, ComunidadeView, null, FuncaoSite.NomePagina.FORUM);

                #endregion

                if (validacao.IsDono || validacao.IsModerador || validacao.IsMembro)
                {

                    #region HTML FORM

                    v_html_form = @"
                        <div class='cnt-criar-tpc' style='display:none;'>

                            <div id='formCriarTpc' class='fm-cnt-default'>

                                <div class='box-form'>
                                    <fieldset>

                                        <div class='half-width'>
                                            <label for='txtNome'>" + UIConfig.NomeDoTopico + @"</label>
                                            <input type='text' name='txtNome' maxlength='200' class='txtNome obrigatorio' />
                                        </div>

                                        <div class='half-width'>
                                            <div>
                                                <p class='half-width' style='width:100%;'>
                                                    <label>" + UIConfig.Fixo + @"</label>
                                                    <b>
                                                    <span class='cd-select' style='width: 100%;'>
                                                        <select name='dropFixo' class='dropFixo' style='width: 100%;'>
                                                            <option value='2'>" + UIConfig.NaoFixo + @"</option>
                                                            <option value='1'>" + UIConfig.Fixo + @"</option>
                                                        </select>
                                                    </span>
                                                    </b>
                                                </p>
                                            </div>
                                        </div>

                                        <div class='half-width' style='width: 100%;'>
                                            <label for='userPerfilCmm'>" + UIConfig.DescricaoDoTopico + @"</label>

                                            <div class='ed-cnt-cmt' style='margin-top:5px;'></div><!-- end of comments container 'ed-cnt-cmt' -->

                                        </div>

                                    </fieldset>

                                    <div class='ed-cnt-cmt-btn'>
                                        <div class='button-group'>
                                            <a href='javascript:void(0);' class='button big icon remove btnSalvarTpc'>" + UIConfig.SalvarTopico + @"</a>
                                            <a href='javascript:void(0);' class='button big icon remove btnSalvarTpcCanc'>" + UIConfig.Cancelar + @"</a>
                                        </div>
                                    </div>

                                    </div>

                                </div>

                            </div> <!-- .fm-cnt-default -->

                            <div class='ed-cnt-cmt-btn'>
                            <a href='javascript:void(0);' class='button big icon remove btnCriarTpc'>" + UIConfig.CriarTopico + @"</a>
                        </div>
                    ";

                    #endregion

                }

                v_acesso = 1;
            }
            else
            {
                #region TITULO

                v_html_titulo = @"
                    <div class='ms-cnt-tit-blc'>
                        <h1 class='ms-tit-blc-txt icon-cmm'>" + UIConfig.Forum + @"</h1>
                        <div class='ms-tit-blc-line'></div>
                    </div>
                ";

                #endregion
            }

            object[] arrRetorno = new object[5];
            arrRetorno[0] = v_html_titulo;
            arrRetorno[1] = v_html;
            arrRetorno[2] = v_html_paginacao;
            arrRetorno[3] = v_html_form;
            arrRetorno[4] = v_acesso;

            return Json(arrRetorno, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ajax/topico/template/view")]
        public ActionResult getCmmTopicosPost()
        {
            var validacao = new CmmValidacao(PerfilLogged, ComunidadeView);

            var v_html = string.Empty;
            var v_html_titulo = string.Empty;
            var v_html_paginacao = string.Empty;
            var v_html_menuitens = string.Empty;
            var v_html_editor = string.Empty;
            var v_html_topico = string.Empty;
            var topicoIdView = TOPICO_NUMBER;

            if (validacao.IsDono || validacao.IsModerador || validacao.IsMembro || validacao.IsPublica)
            {
                var moderadores = _moderadorService.ObterModeradores(ComunidadeView.Id).ToList();

                int totalRecords;
                TopicoPost ultimoPost;
                var posts = _topicoPostService.ObterPosts(topicoIdView, PAGE_NUMBER, FuncaoSite.TOTAL_POST_PAGE, out totalRecords, out ultimoPost).ToList();

                #region POSTS

                v_html = @"
                    <div class='ls-cnt-default poseydon-cmm-post-list-size'>
                    <ul class='list-ul'>
                ";

                string autorizacaoComu;

                foreach (var post in posts)
                {
                    var perfilDonoTpc = post.Usuario.Perfil;

                    v_html += "<li class='list-li' codpost='" + post.Id.ToString() + "' cp='" + perfilDonoTpc.Id.ToString() + "'>";

                    #region TOOLTIP MENU DE OPCOES

                    #region ITENS MENU

                    v_html_menuitens = string.Empty;

                    var perfilDonoTpcModerador = moderadores.Where(x => x.UsuarioModeradorId == perfilDonoTpc.Id).FirstOrDefault();

                    if (validacao.IsDono)
                    {
                        v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codpost='" + post.Id.ToString() + "' opcao='E'>"+ UIConfig.ExcluirPostagem +@"</div></a>";

                        if (perfilDonoTpc.Id != PerfilLogged.Id && perfilDonoTpcModerador == null)
                        {
                            v_html_menuitens += "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codpost='" + post.Id.ToString() + "' cc='" + ComunidadeView.Id.ToString() + "' cp='" + perfilDonoTpc.Id.ToString() + "' opcao='M'>Add como moderador</div></a>";
                        }
                    }
                    else if (validacao.IsModerador)
                    {
                        v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codpost='" + post.Id.ToString() + "' opcao='E'>" + UIConfig.ExcluirPostagem + @"</div></a>";
                    }
                    else if (perfilDonoTpc.Id == PerfilLogged.Id)
                    {
                        v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codpost='" + post.Id.ToString() + "' opcao='E'>" + UIConfig.ExcluirPostagem + @"</div></a>";
                    }

                    autorizacaoComu = string.Empty;

                    if (perfilDonoTpc.Id == ComunidadeView.UsuarioId)
                    {
                        autorizacaoComu = "<span class='cmm-dono'>"+UIConfig.DonoDaComunidade+"</span>";
                    }
                    else if (perfilDonoTpcModerador != null)
                    {
                        autorizacaoComu = "<span class='cmm-mode'>" + UIConfig.ModeradorDaComunidade + "</span>";
                    }

                    #endregion

                    if (v_html_menuitens.Length > 0)
                    {
                        #region HTML

                        v_html += @"
                                <div class='popr tooltip-dropmenu-container' codpost='" + post.Id.ToString() + @"'>
                                    <div class='button-group minor-group'>

                                        <div class='tooltip-dropmenu'>
                                            <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                            <div class='tooltip-dropmenu-html-container' codpost='" + post.Id.ToString() + @"'>
                                                <div class='tooltip-dropmenu-menu' codpost='" + post.Id.ToString() + @"'>
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

                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilDonoTpc.Alias);
                    urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfilDonoTpc.Id.ToString() + @"/1.jpg", false);

                    #region HTML

                    v_html += @"
                                <img class='list-img' src='" + urlResolve + @"'><div class='seta'></div>
                                <h3 class='list-h3'><a href='" + urlEncryptadaPerfil + "'>" + perfilDonoTpc.Nome + " " + perfilDonoTpc.Sobrenome + @"</a><span class='list-span-dt'>" + FuncaoSite.getTempoPost(post.DataPost) + @"</span></h3>
                                " + autorizacaoComu + @"
                                <div class='list-p'>" + post.DescricaoHtml + @"</div>
                            </li>
                        ";

                    #endregion
                }

                v_html += @"
                        </ul>
                    </div>
                ";

                #endregion

                var topico = _topicoService.Obter(topicoIdView);
                v_html_topico = topico.DescricaoHtml;

                #region TITULO

                v_html_titulo = @"
                    <div class='ms-cnt-tit-blc'>
                        <h1 class='ms-tit-blc-txt icon-cmm'>" + topico.Titulo + @" <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                        <div class='ms-tit-blc-line'></div>
                    </div>
                ";

                #endregion

                #region PAGINACAO

                v_html_paginacao = FuncaoSite.getPaginacaoCmm(PAGE_NUMBER, totalRecords, ComunidadeView, topicoIdView, FuncaoSite.NomePagina.TOPICO);

                #endregion

                if (validacao.IsDono || validacao.IsModerador || validacao.IsMembro)
                {

                    #region EDITOR

                    v_html_editor = @"
                        <div class='ms-cnt-blc cmt-cnt-editor'>
                            <div class='ed-cnt-cmt view-editor-post'>
                            </div>
                        </div>
                    ";

                    #endregion

                }
            }
            else
            {
                #region TITULO

                v_html_titulo = @"
                    <div class='ms-cnt-tit-blc'>
                        <h1 class='ms-tit-blc-txt icon-cmm'>Tópico</h1>
                        <div class='ms-tit-blc-line'></div>
                    </div>
                ";

                #endregion
            }

            object[] arrRetorno = new object[5];
            arrRetorno[0] = v_html_titulo;
            arrRetorno[1] = v_html;
            arrRetorno[2] = v_html_paginacao;
            arrRetorno[3] = v_html_topico;
            arrRetorno[4] = v_html_editor;

            return Json(arrRetorno, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _membroService.Dispose();
            _topicoService.Dispose();
            _topicoPostService.Dispose();
            _moderadorService.Dispose();
        }
    }
}