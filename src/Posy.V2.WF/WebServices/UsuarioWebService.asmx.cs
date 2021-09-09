﻿using Microsoft.AspNet.Identity;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Extensions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.WF.Helpers;
using Posy.V2.WF.Models;
using Posy.V2.WF.Validacao;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Posy.V2.WF.WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class UsuarioWebService : WebService
    {
        //private IUnityContainer _container;
        //private IUsuarioServico _usuarioServico;
        //private IPerfilServico _perfilService;
        //private IAmizadeServico _amizadeServico;
        //private IVisitantePerfilServico _visitantePerfilService;
        //private IPostPerfilServico _postServico;
        //private IPostPerfilBloqueadoServico _perfilBloqueadoServico;
        //private IPostPerfilComentarioServico _postPerfilComentarioServico;
        //private IPostOcultoServico _postOcultoServico;
        //private IPrivacidadeServico _privacidadeService;
        //private IRecadoServico _recadoServico;
        //private IRecadoComentarioServico _recadoComentarioServico;
        //private IVideoServico _videoServico;
        //private IVideoComentarioServico _videoComentarioServico;
        //private IDepoimentoServico _depoimentoServico;
        //private IComunidadeServico _comunidadeServico;
        //private IMembroServico _membroServico;
        //private ITopicoServico _topicoServico;

        [Import] public IUsuarioService _usuarioService { get; set; }
        [Import] public IPerfilService _perfilService { get; set; }
        [Import] public IAmizadeService _amizadeService { get; set; }
        [Import] public IVisitantePerfilService _visitantePerfilService { get; set; }
        [Import] public IPostPerfilService _postService { get; set; }
        [Import] public IPostPerfilBloqueadoService _perfilBloqueadoService { get; set; }
        [Import] public IPostPerfilComentarioService _postPerfilComentarioService { get; set; }
        [Import] public IPostOcultoService _postOcultoService { get; set; }
        [Import] public IPrivacidadeService _privacidadeService { get; set; }
        [Import] public IRecadoService _recadoService { get; set; }
        [Import] public IRecadoComentarioService _recadoComentarioService { get; set; }
        [Import] public IVideoService _videoService { get; set; }
        [Import] public IVideoComentarioService _videoComentarioService { get; set; }
        [Import] public IDepoimentoService _depoimentoService { get; set; }
        [Import] public IComunidadeService _comunidadeService { get; set; }
        [Import] public IMembroService _membroService { get; set; }
        [Import] public ITopicoService _topicoService { get; set; }

        private string BTN_VER_MAIS_COMENTS = UIConfig.VerMaisComentarios;
        private string BTN_VER_MENOS_COMENTS = UIConfig.VerMenosComentarios;
        private Perfil _perfilLogado;
        private Perfil _perfilView;
        private int PAGE_NUMBER;

        private string urlEncryptadaPerfil;
        private string urlEncryptadaCmm;
        private string urlResolve;

        public UsuarioWebService()
        {
            //_container = HttpContext.Current.Application.GetContainer();
            //_usuarioServico = _container.Resolve<IUsuarioServico>();
            //_perfilService = _container.Resolve<IPerfilServico>();
            //_amizadeServico = _container.Resolve<IAmizadeServico>();
            //_postServico = _container.Resolve<IPostPerfilServico>();

            _usuarioService = Global.container.GetInstance<IUsuarioService>();
            _perfilService = Global.container.GetInstance<IPerfilService>();
            _amizadeService = Global.container.GetInstance<IAmizadeService>();
            _visitantePerfilService = Global.container.GetInstance<IVisitantePerfilService>();
            _postService = Global.container.GetInstance<IPostPerfilService>();
            _perfilBloqueadoService = Global.container.GetInstance<IPostPerfilBloqueadoService>();
            _postPerfilComentarioService = Global.container.GetInstance<IPostPerfilComentarioService>();
            _postOcultoService = Global.container.GetInstance<IPostOcultoService>();
            _privacidadeService = Global.container.GetInstance<IPrivacidadeService>();
            _recadoService = Global.container.GetInstance<IRecadoService>();
            _recadoComentarioService = Global.container.GetInstance<IRecadoComentarioService>();
            _videoService = Global.container.GetInstance<IVideoService>();
            _videoComentarioService = Global.container.GetInstance<IVideoComentarioService>();
            _depoimentoService = Global.container.GetInstance<IDepoimentoService>();
            _comunidadeService = Global.container.GetInstance<IComunidadeService>();
            _membroService = Global.container.GetInstance<IMembroService>();
            _topicoService = Global.container.GetInstance<ITopicoService>();

            //FormsIdentity ident = HttpContext.Current.User.Identity as FormsIdentity;

            //var id = User.Identity.Name;

            //if (ident != null)

            VerifyAuthenticated();
        }

        public void VerifyAuthenticated()
        {
            //FormsIdentity ident = HttpContext.Current.User.Identity as FormsIdentity;

            //if (ident != null)
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetUserId();
                //FormsAuthenticationTicket ticket = ident.Ticket;

                // Culture do usuario logado
                //var ci = new System.Globalization.CultureInfo(ticket.UserData);
                //System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                //System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

                // Entidade Perfil do Usuario Logado
                _perfilLogado = PerfilLogado.Perfil();
            }
            else
            {
                _perfilLogado = null;
            }

            _perfilView = (Perfil)Application.Get("perfilView");
            //CODCMM_VIEW = Convert.ToInt32(Application.Get("CodCmmView"));
            PAGE_NUMBER = Convert.ToInt32(Application.Get("PageNumber"));
            //TOPICO_NUMBER = Convert.ToInt32(Application.Get("TopicoNumber"));
        }

        #region PERFIL CADASTRO E EDIÇÃO

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> EditarPerfil(EditarPerfilModel model)
        {
            ResponseService responseService = null;

            try
            {
                var perfil = _perfilService.EditarPerfil(_perfilLogado.UsuarioId, model.Nome, model.Sobrenome, model.Alias, model.DataNascimento, model.Sexo, model.EstadoCivil, model.FrasePerfil, model.DescricaoPerfil, model.PaisId);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), perfil, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> VisitarPerfil()
        {
            ResponseService responseService = null;

            try
            {
                //_visitantePerfilService = _container.Resolve<IVisitantePerfilServico>();
                //_visitantePerfilService.SalvarVisita(_perfilLogado.UsuarioId, _perfilView.UsuarioId);
                _visitantePerfilService.SalvarVisita(_perfilView.UsuarioId);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), null, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        #endregion

        #region PERFIL PRIVACIDADE

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> SalvarPrivacidade(SalvarPrivacidadePerfilModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_privacidadeService = _container.Resolve<IPrivacidadeServico>();
                //_privacidadeService.SalvarPrivacidade(_perfilLogado.UsuarioId, model.VerRecado, model.EscreverRecado);
                _privacidadeService.SalvarPrivacidade(model.VerRecado, model.EscreverRecado);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), null, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        #endregion

        #region PERFIL GET VIEWS

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public Task<ResponseService> getPerfilView()
        {
            ResponseService responseService = null;

            try
            {
                var visitantes = (_perfilView.UsuarioId == _perfilLogado.UsuarioId) ? getListVisitantes() : "-1";
                var response = getListDepoimentos(_perfilView.UsuarioId, 1).Result; // Tem que vim antes da rotina de carregamento do meu vertical

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(response.ReasonPhrase);

                var depoimentos = response.Result;

                object[] arrRetorno = new object[7];
                arrRetorno[0] = _perfilView; //perfil
                arrRetorno[1] = getBlocoAmigos(_perfilView); //amigos 
                arrRetorno[2] = getBlocoCmm(_perfilView); //comunidades     
                arrRetorno[3] = getTemplateMenuVerticalPerfil(_perfilView); //menu vertical
                arrRetorno[4] = visitantes; //visitantes
                arrRetorno[5] = new object[2] { "", _perfilView.PerfilHtml }; //view
                arrRetorno[6] = depoimentos; //view depo

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public Task<ResponseService> getInicioView()
        {
            ResponseService responseService = null;

            try
            {
                var visitantes = getListVisitantes();

                object[] arrRetorno = new object[6];
                arrRetorno[0] = _perfilLogado; //perfil
                arrRetorno[1] = getBlocoAmigos(_perfilLogado); //amigos    
                arrRetorno[2] = getBlocoCmm(_perfilLogado); //comunidades    
                arrRetorno[3] = getTemplateMenuVerticalPerfil(_perfilLogado); //menu vertical
                arrRetorno[4] = visitantes; //visitantes

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public Task<ResponseService> getEditPerfilView()
        {
            ResponseService responseService = null;

            try
            {
                //_privacidadeService = _container.Resolve<IPrivacidadeServico>();
                //var privacidade = _privacidadeService.Obter(_perfilLogado.UsuarioId);
                var privacidade = _privacidadeService.Obter();

                var visitantes = getListVisitantes();

                object[] arrRetorno = new object[6];
                arrRetorno[0] = _perfilLogado; //perfil
                arrRetorno[1] = getBlocoAmigos(_perfilLogado); //amigos     
                arrRetorno[2] = getBlocoCmm(_perfilLogado); //comunidades    
                arrRetorno[3] = getTemplateMenuVerticalPerfil(_perfilLogado); //menu vertical
                arrRetorno[4] = visitantes; //visitantes
                arrRetorno[5] = privacidade; //privDao.getObjeto(_perfilLogado); //view

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public Task<ResponseService> getPerfilAmigosView()
        {
            ResponseService responseService = null;

            try
            {
                var visitantes = (_perfilView.UsuarioId == _perfilLogado.UsuarioId) ? getListVisitantes() : "-1";
                var response = getPerfilAmigos(_perfilView.UsuarioId, PAGE_NUMBER).Result;

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(response.ReasonPhrase);

                var amigos = response.Result;

                object[] arrRetorno = new object[6];
                arrRetorno[0] = _perfilView; //perfil
                arrRetorno[1] = getBlocoAmigos(_perfilView); //amigos     
                arrRetorno[2] = getBlocoCmm(_perfilView); //comunidades    
                arrRetorno[3] = getTemplateMenuVerticalPerfil(_perfilView); //menu vertical
                arrRetorno[4] = visitantes; //visitantes
                arrRetorno[5] = amigos; //view

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public Task<ResponseService> getPerfilAmigosPendenteView()
        {
            ResponseService responseService = null;

            try
            {
                var visitantes = (_perfilView.UsuarioId == _perfilLogado.UsuarioId) ? getListVisitantes() : "-1";
                var response = getPerfilAmigosPendente(PAGE_NUMBER).Result;

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(response.ReasonPhrase);

                var amigosPendentes = response.Result;

                object[] arrRetorno = new object[6];
                arrRetorno[0] = _perfilLogado; //perfil
                arrRetorno[1] = getBlocoAmigos(_perfilView); //amigos     
                arrRetorno[2] = getBlocoCmm(_perfilView); //comunidades    
                arrRetorno[3] = getTemplateMenuVerticalPerfil(_perfilView); //menu vertical
                arrRetorno[4] = visitantes; //visitantes
                arrRetorno[5] = amigosPendentes; //view

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public Task<ResponseService> getPerfilRecadosView()
        {
            ResponseService responseService = null;

            try
            {
                var visitantes = (_perfilView.UsuarioId == _perfilLogado.UsuarioId) ? getListVisitantes() : "-1";
                var response = getListRecados(_perfilView.UsuarioId, PAGE_NUMBER).Result; // Tem que vim antes da rotina de carregamento do meu vertical

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(response.ReasonPhrase);

                var recados = response.Result;

                object[] arrRetorno = new object[6];
                arrRetorno[0] = _perfilView; //perfil
                arrRetorno[1] = getBlocoAmigos(_perfilView); //amigos     
                arrRetorno[2] = getBlocoCmm(_perfilView); //comunidades    
                arrRetorno[3] = getTemplateMenuVerticalPerfil(_perfilView); //menu vertical
                arrRetorno[4] = visitantes; //visitantes
                arrRetorno[5] = recados; //view

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public Task<ResponseService> getPerfilVideosView()
        {
            ResponseService responseService = null;

            try
            {
                var visitantes = (_perfilView.UsuarioId == _perfilLogado.UsuarioId) ? getListVisitantes() : "-1";
                var response = getListVideos(_perfilView.UsuarioId, PAGE_NUMBER).Result;

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(response.ReasonPhrase);

                var videos = response.Result;

                object[] arrRetorno = new object[6];
                arrRetorno[0] = _perfilView; //perfil
                arrRetorno[1] = getBlocoAmigos(_perfilView); //amigos          
                arrRetorno[2] = getBlocoCmm(_perfilView); //comunidades    
                arrRetorno[3] = getTemplateMenuVerticalPerfil(_perfilView); //menu vertical
                arrRetorno[4] = visitantes; //visitantes
                arrRetorno[5] = videos; //view

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public Task<ResponseService> getPerfilDepoimentosView()
        {
            ResponseService responseService = null;

            try
            {
                var visitantes = (_perfilView.UsuarioId == _perfilLogado.UsuarioId) ? getListVisitantes() : "-1";
                var response = getListDepoimentos(_perfilView.UsuarioId, PAGE_NUMBER).Result; // Tem que vim antes da rotina de carregamento do meu vertical

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(response.ReasonPhrase);

                var depoimentos = response.Result;

                object[] arrRetorno = new object[6];
                arrRetorno[0] = _perfilView; //perfil
                arrRetorno[1] = getBlocoAmigos(_perfilView); //amigos      
                arrRetorno[2] = getBlocoCmm(_perfilView); //comunidades    
                arrRetorno[3] = getTemplateMenuVerticalPerfil(_perfilView); //menu vertical
                arrRetorno[4] = visitantes; //visitantes
                arrRetorno[5] = depoimentos; //view

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public Task<ResponseService> getPerfilComunidadesView()
        {
            ResponseService responseService = null;

            try
            {
                var visitantes = (_perfilView.UsuarioId == _perfilLogado.UsuarioId) ? getListVisitantes() : "-1";

                var response = getListComunidades(_perfilView.UsuarioId, PAGE_NUMBER).Result;

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(response.ReasonPhrase);

                var comunidades = response.Result;

                object[] arrRetorno = new object[6];
                arrRetorno[0] = _perfilView; //perfil
                arrRetorno[1] = getBlocoAmigos(_perfilView); //amigos     
                arrRetorno[2] = getBlocoCmm(_perfilView); //comunidades    
                arrRetorno[3] = getTemplateMenuVerticalPerfil(_perfilView); //menu vertical
                arrRetorno[4] = visitantes; //visitantes
                arrRetorno[5] = comunidades; //view

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        public string UsuariosOnLine()
        {
            string usuariosOnline = "OnLine: " + Application.Get("UsuariosOnLine").ToString();

            return usuariosOnline;
        }

        #endregion

        #region PERFIL POST

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> SalvarPost(PostPerfilModel model)
        {
            ResponseService responseService = null;

            try
            {
                var v_html_view = string.Empty;
                var v_html_btntop = string.Empty;
                var v_html_menuitens = string.Empty;

                //var post = _postService.Postar(_perfilLogado.UsuarioId, model.PostHtml);
                var post = _postService.Postar(model.PostHtml);

                #region TEMPLATE POST

                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + _perfilLogado.UsuarioId.ToString() + @"/1.jpg", false);

                v_html_view += @"
                    <div class='tl-cnt-default-block postado-recente' postId='" + post.PostPerfilId.ToString() + @"' codperfil='" + _perfilLogado.UsuarioId.ToString() + @"'>

                        <div class='tl-cnt-default-img cd-picture'>
                            <img src='" + urlResolve + @"' alt='Picture' />
                        </div> <!-- tl-cnt-default-img -->

                        <div class='tl-cnt-default-content'>
                ";

                #region TOOLTIP MENU DE OPCOES

                v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' postId='" + post.PostPerfilId.ToString() + "' opcao='E'>" + UIConfig.ExcluirPostagem + @"</div></a>";

                v_html_view += @"
                    <div class='popr tooltip-dropmenu-container' postId='" + post.PostPerfilId.ToString() + @"'>
                        <div class='button-group minor-group'>

                        <div class='tooltip-dropmenu'>
                            <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                            <div class='tooltip-dropmenu-html-container' postId='" + post.PostPerfilId.ToString() + @"'>
                                <div class='tooltip-dropmenu-menu' postId='" + post.PostPerfilId.ToString() + @"'>
                                    " + v_html_menuitens + @"
                                </div>
                            </div>
                            <div class='clear'></div>
                        </div>

                        </div>
                    </div>
                ";

                #endregion

                v_html_view += @"<input type='checkbox' class='tl-checkbox' value='" + post.PostPerfilId.ToString() + "' />";

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(_perfilLogado.Alias, _perfilLogado.UsuarioId);

                v_html_view += @"
                            <h2 class='tl-h2'><a href='" + urlEncryptadaPerfil + @"'>" + _perfilLogado.Nome + " " + _perfilLogado.Sobrenome + @"</a><span data-utime='1371248446' class='post-dt'>" + FuncaoSite.getTempoPost(DateTime.Now) + @"</span></h2>
                            <h3 class='tl-h3'><span>" + _perfilLogado.FraseHtml + @"</span></h3>
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

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> ExcluirPost(ExcluirPostPerfilModel model)
        {
            ResponseService responseService = null;

            try
            {
                _postService.ExcluirPost(model.PostId);
                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), "", Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> ExcluirMultiplosPost(List<ExcluirPostPerfilModel> model)
        {
            ResponseService responseService = null;

            try
            {
                foreach (var post in model)
                    _postService.ExcluirPost(post.PostId);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), "", Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> OcultarPost(OcultarPostPerfilModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_postOcultoServico = _container.Resolve<IPostOcultoServico>();
                //_postOcultoService.OcultarPost(_perfilLogado.UsuarioId, model.PostId);
                _postOcultoService.OcultarPost(model.PostId);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), "", Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> BloquearPost(BloquearPerfilModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_perfilBloqueadoServico = _container.Resolve<IPostPerfilBloqueadoServico>();
                //_perfilBloqueadoService.BloquearPostPerfil(_perfilLogado.UsuarioId, model.UsuarioIdBloqueado);
                _perfilBloqueadoService.BloquearPostPerfil(model.UsuarioIdBloqueado);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), "", Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> SalvarPostComentario(ComentarioPerfilModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_postPerfilComentarioServico = _container.Resolve<IPostPerfilComentarioServico>();
                //_perfilBloqueadoServico = _container.Resolve<IPostPerfilBloqueadoServico>();

                var post = _postService.ObterPost(model.PostId);
                var usuarioComPostBloqueado = _perfilBloqueadoService.ObterPerfilBloqueado(post.UsuarioId, _perfilLogado.UsuarioId);
                if (usuarioComPostBloqueado != null)
                    throw new Exception(Errors.ErroAoEnviarComentario);

                //var comentario = _postPerfilComentarioService.Comentar(model.PostId, _perfilLogado.UsuarioId, model.Comentario);
                var comentario = _postPerfilComentarioService.Comentar(model.PostId, model.Comentario);

                #region TEMPLATE COMENTARIO

                var v_html_view = string.Empty;
                var v_html_menuitens = string.Empty;

                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + _perfilLogado.UsuarioId.ToString() + @"/1.jpg", false);

                v_html_view += @"
                            <div class='cmt-cnt' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"' style='display:none;'>

                                <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                        ";

                #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(_perfilLogado.Alias, _perfilLogado.UsuarioId);
                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + _perfilLogado.UsuarioId.ToString() + @"/1.jpg", false);

                v_html_view += @"
                            <div class='tooltip-dropmenu-html-container'>
                                <div class='tooltip'>
                                    <span class='tooltip-content'>
                                        <img src='" + urlResolve + @"' alt='' />
                                        <span class='tooltip-text'>
                                            <a class='tooltip-text-a' href='" + urlEncryptadaPerfil + @"'>" + _perfilLogado.Nome + " " + _perfilLogado.Sobrenome + "</a><hr><span class='tooltip-text-span'>" + _perfilLogado.FraseHtml + @"</span> 
                                        </span>
                                    </span>
                                </div>
                            </div>
                        ";

                #endregion

                v_html_view += @"<div class='thecom'>";

                #region TOOLTIP MENU DE OPCOES

                v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.PostPerfilComentarioId.ToString() + "' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>";

                v_html_view += @"
                            <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"'>
                                <div class='button-group minor-group'>

                                    <div class='tooltip-dropmenu'>
                                        <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                        <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"'>
                                            <div class='tooltip-dropmenu-menu' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"'>
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
                                    <h5><a href='" + urlEncryptadaPerfil + "'>" + _perfilLogado.Nome + " " + _perfilLogado.Sobrenome + @"</a></h5><span data-utime='1371248446' class='com-dt'>" + FuncaoSite.getTempoPost(DateTime.Now) + @"</span>
                                    <div class='com-text'>" + comentario.ComentarioHtml + @"</div>
                                </div>
                            </div><!-- end 'cmt-cnt' -->
                        ";

                #endregion

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), v_html_view, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> ExcluirPostComentario(ExcluirComentarioPerfilModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_postPerfilComentarioServico = _container.Resolve<IPostPerfilComentarioServico>();
                //_postPerfilComentarioService.ExcluirComentario(model.ComentarioId, _perfilLogado.UsuarioId);
                _postPerfilComentarioService.ExcluirComentario(model.ComentarioId);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), "", Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> getListInicioAtualizacao(int page)
        {
            ResponseService responseService = null;

            try
            {
                //_postPerfilComentarioServico = _container.Resolve<IPostPerfilComentarioServico>();
                //_perfilBloqueadoServico = _container.Resolve<IPostPerfilBloqueadoServico>();

                var posts = _postService.ObterPosts(_perfilLogado.UsuarioId, page, FuncaoSite.TOTAL_POST_PAGE).ToList();

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
                        var postId = post.PostPerfilId.ToString();

                        urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioPost.UsuarioId.ToString() + @"/1.jpg", false);

                        #region HTML

                        v_html += @"
                                <div class='tl-cnt-default-block' postId='" + postId + @"' codperfil='" + usuarioPost.UsuarioId.ToString() + @"' style='display:none;'>

                                    <div class='tl-cnt-default-img cd-picture'>
                                        <img src='" + urlResolve + @"' alt='Picture' />
                                    </div> <!-- tl-cnt-default-img -->

                                    <div class='tl-cnt-default-content'>
                            ";

                        #endregion

                        #region TOOLTIP MENU DE OPCOES

                        if (post.UsuarioId == _perfilLogado.UsuarioId)
                        {
                            v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' postId='" + postId + "' opcao='E'>" + UIConfig.ExcluirPostagem + @"</div></a>";
                        }
                        else
                        {
                            v_html_menuitens = @"
                                <a href='javascript:void(0);'><div class='menuitem btnMenuItem' postId='" + postId + @"' opcao='O'>" + UIConfig.OcultarPostagem + @"</div></a>
                                <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + usuarioPost.UsuarioId.ToString() + @"' opcao='B'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
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

                        if (usuarioPost.UsuarioId == _perfilLogado.UsuarioId)
                            v_html += @"<input type='checkbox' class='tl-checkbox' value='" + postId + "' />";

                        //v_html.Append("     <iframe class='tl-text' src='data:text/html;charset=utf-8," + objeto.Post + "'></iframe>");

                        urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioPost.Perfil.Alias, usuarioPost.UsuarioId);

                        #region HTML

                        v_html += @"
                            <h2 class='tl-h2'><a href='" + urlEncryptadaPerfil + @"'>" + usuarioPost.Perfil.Nome + " " + usuarioPost.Perfil.Sobrenome + @"</a><span data-utime='1371248446' class='post-dt'>" + FuncaoSite.getTempoPost(post.DataPost) + @"</span></h2>
                            <h3 class='tl-h3'><span>" + usuarioPost.Perfil.FraseHtml + @"</span></h3>
                            <div class='tl-text' >" + post.PostHtml + @"</div>
                        ";

                        #endregion

                        #region CONTAINER EDITOR

                        var usuarioLogadoBloqueadoParaPostarNoUsuarioPost =
                            _perfilBloqueadoService.ObterPerfilBloqueado(usuarioPost.UsuarioId, _perfilLogado.UsuarioId);

                        // Verifica se o usuario esta bloqueado para postar bloqueado 0(Pode postar) e 1
                        // Sempre faz a verificação pois estão misturados posts de outros usuarios com os meus, logo os meus não precisam esta bloqueados
                        if (usuarioLogadoBloqueadoParaPostarNoUsuarioPost != null)
                            v_html += @"<div class='ed-cnt-cmt' bloqueado='1' >";
                        else
                            v_html += @"<div class='ed-cnt-cmt' bloqueado='0' >";

                        #region COMENTARIOS DO POST

                        var comentarios = _postPerfilComentarioService.ObterComentarios(post.PostPerfilId, 1, FuncaoSite.TOTAL_COMENT_PAGE);

                        v_html += @"<div class='ed-cnt-cmt-coments'>";

                        var ultimo_codcoment = Guid.Empty;

                        foreach (var comentario in comentarios)
                        {
                            var usuarioComentario = comentario.Usuario;

                            urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioComentario.UsuarioId.ToString() + @"/1.jpg", false);

                            v_html += @"<div class='cmt-cnt' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"'>

                                                    <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />";

                            #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                            urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioComentario.Perfil.Alias, usuarioComentario.UsuarioId);
                            urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioComentario.UsuarioId.ToString() + @"/1.jpg", false);

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


                            if (usuarioComentario.UsuarioId == _perfilLogado.UsuarioId)
                            {
                                v_html_menuitens = @"<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.PostPerfilComentarioId.ToString() + "' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>";
                            }
                            else
                            {
                                v_html_menuitens = @"
                                        <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>
                                        <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + usuarioComentario.UsuarioId.ToString() + @"' opcao='B'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
                                    ";
                            }

                            v_html += @"
                                    <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"'>
                                        <div class='button-group minor-group'>

                                            <div class='tooltip-dropmenu'> 
                                                <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                                <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"'>
                                                    <div class='tooltip-dropmenu-menu' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"'>
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

                        var proximosComentarios = _postPerfilComentarioService.ObterComentarios(post.PostPerfilId, 2, FuncaoSite.TOTAL_COMENT_PAGE).ToList();
                        if (proximosComentarios.Count > 0)
                        {
                            v_html += @"
                                <div class='ed-cnt-cmt-more button-group minor-group'>
                                    <a href='javascript:void(0);' class='button minor1 btnVerMais' postId='" + post.PostPerfilId.ToString() + @"' page='" + nextPage.ToString() + @"'>" + BTN_VER_MAIS_COMENTS + @"</a>
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

                    var proximosPosts = _postService.ObterPosts(_perfilLogado.UsuarioId, nextPage, FuncaoSite.TOTAL_POST_PAGE).ToList();

                    totalPostsParaVer = proximosPosts.Count;

                    #region BOTAO VER MAIS

                    if (totalPostsParaVer > 0)
                    {
                        urlResolve = Funcao.ResolveServerUrl("~/img/cd-icon-location.svg", false);
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

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> getListPostComentarios(Guid postId, int page)
        {
            ResponseService responseService = null;

            try
            {
                //_postPerfilComentarioServico = _container.Resolve<IPostPerfilComentarioServico>();

                string v_html = string.Empty;
                string v_html_menuitens = string.Empty;

                var nextPage = page + 1;

                #region COMENTARIOS DO POST

                var comentarios = _postPerfilComentarioService.ObterComentarios(postId, page, FuncaoSite.TOTAL_COMENT_PAGE);

                foreach (var comentario in comentarios)
                {
                    var usuarioComentario = comentario.Usuario;

                    urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                    v_html += @"
                        <div class='cmt-cnt' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"' style='display:none;'>
                            <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                    ";

                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioComentario.Perfil.Alias, usuarioComentario.UsuarioId);

                    #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                    urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

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

                    if (usuarioComentario.UsuarioId == _perfilLogado.UsuarioId)
                    {
                        v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.PostPerfilComentarioId.ToString() + "' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>";
                    }
                    else
                    {
                        v_html_menuitens = @"
                            <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"' opcao='EC'>" + UIConfig.ExcluirComentário + @"</div></a>
                            <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + usuarioComentario.UsuarioId.ToString() + @"' opcao='B'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
                        ";
                    }

                    v_html += @"
                        <div class='popr tooltip-dropmenu-comentario-container' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"'>
                            <div class='button-group minor-group'>

                            <div class='tooltip-dropmenu'> 
                                <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                                <div class='tooltip-dropmenu-html-container' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"'>
                                    <div class='tooltip-dropmenu-menu' codcoment='" + comentario.PostPerfilComentarioId.ToString() + @"'>
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

                var proximosComentarios = _postPerfilComentarioService.ObterComentarios(postId, nextPage, FuncaoSite.TOTAL_COMENT_PAGE).ToList();
                if (proximosComentarios.Count > 0)
                {
                    v_html += @"
                                <div class='ed-cnt-cmt-more button-group minor-group'>
                                        <a href='javascript:void(0);' class='button minor1 btnVerMais' postId='" + postId.ToString() + "' page='" + nextPage.ToString() + "'>" + BTN_VER_MAIS_COMENTS + @"</a>
                            ";

                    if (page > 1)
                        v_html += @"<a href='javascript:void(0);' class='button minor1 btnVerMenos' postId='" + postId.ToString() + "' >" + BTN_VER_MENOS_COMENTS + "</a>";

                    v_html += @"
                            </div>
                        ";
                }
                else if (page > 1)
                {
                    v_html += @"
                                <div class='ed-cnt-cmt-more button-group minor-group'>
                                     <a href='javascript:void(0);' class='button minor1 btnVerMenos' postId='" + postId.ToString() + "' >" + BTN_VER_MENOS_COMENTS + @"</a>
                                </div>
                            ";
                }

                #endregion

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), v_html, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        #endregion

        #region PERFIL MENU VERTICAL

        public string getTemplateMenuVerticalPerfil(Perfil perfilView)
        {
            //_recadoServico = _container.Resolve<IRecadoServico>();

            string v_html = string.Empty;

            var validacao = new PerfilValidacao(_perfilLogado.UsuarioId, perfilView.UsuarioId);

            if (validacao.IsMeuPerfil()) // Menu do meu perfil
            {
                int totalRecords;
                //_amizadeService.SolicitacoesRecebidasPendentes(_perfilLogado.UsuarioId, 0, 0, out totalRecords).ToList();
                _amizadeService.SolicitacoesRecebidasPendentes(0, 0, out totalRecords).ToList();

                int totalRecadosNaoLidos;
                _recadoService.ObterRecadosRecebidosNaoLidos(_perfilLogado.UsuarioId, 0, 0, out totalRecadosNaoLidos);

                string v_notif_ami = (totalRecords > 0) ? " <span>" + totalRecords.ToString() + "</span>" : "";
                string v_recados_naolidos = (totalRecadosNaoLidos > 0) ? " <span>" + totalRecadosNaoLidos.ToString() + "</span>" : "";

                #region HTML

                string urlEdit = Funcao.ResolveServerUrl("~/editperfil", false);
                string urlPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilView.Alias, perfilView.UsuarioId);
                string urlInicio = Funcao.ResolveServerUrl("~/inicio", false);
                string urlAmigos = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_AMIGOS;
                string urlAmigosPend = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_AMIGOSPEND;
                string urlRecados = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_RECADOS;
                string urlVideos = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_VIDEOS;
                string urlDepoimentos = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_DEPOIMENTOS;
                string urlComunidades = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_COMUNIDADES;

                v_html = @"
                        <ul class='mn-vert'>
                            <li class='menu-li'><div class='icon-edit'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlEdit + @"'>" + UIConfig.EditarPerfil + @"</a></li>
                            <li class='menu-li'><div class='icon-membropend'></div><a item='amigo' cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlAmigosPend + @"'>" + UIConfig.AmigosPendentes + @"" + v_notif_ami + @"</a></li>
                            <li class='menu-li'><div class='icon-inicio'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlInicio + @"'>" + UIConfig.Inicio + @"</a></li><hr />
                            <li class='menu-li'><div class='icon-perfil'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlPerfil + @"'>" + UIConfig.Perfil + @"</a></li>                
                            <li class='menu-li'><div class='icon-people'></div><a item='amigo' cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlAmigos + @"'>" + UIConfig.Amigos + @"</a></li>
                            <li class='menu-li'><div class='icon-recado'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlRecados + @"'>" + UIConfig.Recados + @"" + v_recados_naolidos + @"</a></li>
                            <li class='menu-li'><div class='icon-video'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlVideos + @"'>" + UIConfig.Videos + @"</a></li>
                            <li class='menu-li'><div class='icon-depo'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlDepoimentos + @"'>" + UIConfig.Depoimentos + @"</a></li>
                            <li class='menu-li'><div class='icon-cmm'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlComunidades + @"'>" + UIConfig.Comunidades + @"</a></li>
                        </ul>
                    ";

                #endregion
            }
            else // Menu do perfil que estou visualizando
            {
                var amizade = _amizadeService.Obter(_perfilLogado.UsuarioId, perfilView.UsuarioId);

                #region HTML

                string urlPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilView.Alias, perfilView.UsuarioId);
                string urlAmigos = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_AMIGOS;
                string urlRecados = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_RECADOS;
                string urlVideos = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_VIDEOS;
                string urlDepoimentos = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_DEPOIMENTOS;
                string urlComunidades = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_COMUNIDADES;

                v_html = @"<ul class='mn-vert'>";

                if (amizade == null || amizade.StatusSolicitacao == SolicitacaoAmizade.Rejeitado)
                    v_html += @"<li class='menu-li'><div class='icon-addamigo'></div><a cp='" + perfilView.UsuarioId.ToString() + "' class='btnViewPerfil' load='add' href='javascript:void(0);'>" + UIConfig.AdicionarAmigo + @"</a></li>";

                else if (amizade.SolicitadoPorId == _perfilLogado.UsuarioId && amizade.StatusSolicitacao == SolicitacaoAmizade.Pendente)
                    v_html += @"<li class='menu-li'><div class='icon-aguradamigo'></div><a cp='" + perfilView.UsuarioId.ToString() + "' href='javascript:void(0);'>" + UIConfig.Esperando + @" " + perfilView.Nome + " " + UIConfig.aceitar + @"</a></li>";

                else if (amizade.SolicitadoParaId == _perfilLogado.UsuarioId && amizade.StatusSolicitacao == SolicitacaoAmizade.Pendente)
                    v_html += @"<li class='menu-li'><div class='icon-aguradamigo'></div><a cp='" + perfilView.UsuarioId.ToString() + "' href='javascript:void(0);'>" + UIConfig.Esperando + @" " + _perfilLogado.Nome + " " + UIConfig.aceitar + @"</a></li>";

                else
                    v_html += @"<li class='menu-li'><div class='icon-delamigo'></div><a cp='" + perfilView.UsuarioId.ToString() + "' cpl='" + _perfilLogado + "' class='btnViewPerfil' load='del' href='javascript:void(0);'>" + UIConfig.RemoverAmigo + @"</a></li>";

                v_html += "<hr />";

                v_html += @"
                        <li class='menu-li'><div class='icon-perfil'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlPerfil + @"'>" + UIConfig.Perfil + @"</a></li>
                        <li class='menu-li'><div class='icon-people'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlAmigos + @"'>" + UIConfig.Amigos + @"</a></li>
                        <li class='menu-li'><div class='icon-recado'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlRecados + @"'>" + UIConfig.Recados + @"</a></li>
                        <li class='menu-li'><div class='icon-video'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlVideos + @"'>" + UIConfig.Videos + @"</a></li>
                        <li class='menu-li'><div class='icon-depo'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlDepoimentos + @"'>" + UIConfig.Depoimentos + @"</a></li>
                        <li class='menu-li'><div class='icon-cmm'></div><a cp='" + perfilView.UsuarioId.ToString() + @"' href='" + urlComunidades + @"'>" + UIConfig.Comunidades + @"</a></li>
                        </ul>
                    ";

                #endregion
            }

            return v_html.ToString();
        }

        #endregion

        #region PERFIL FOTO

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> Base64ToImage(SalvarFotoPerfilModel model)
        {
            ResponseService responseService = null;

            try
            {
                //base64String = "data:image/png;base64,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";
                //base64String = "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"; 

                byte[] imageBytes = Convert.FromBase64String(model.Base64String.Replace("data:image/png;base64,", string.Empty));
                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    string v_dirdestino = Server.MapPath("~") + "\\img\\perfil\\" + _perfilLogado.UsuarioId.ToString();
                    string v_imgtemp = model.ImgTemp.Split('/')[3];

                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                    // Remove todos os arquivos, evitando de ficar aquivo temporario caso o usuario saia sem salvar
                    foreach (string file in Directory.GetFiles(v_dirdestino, "*.*"))
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch { }
                    }

                    // Remove o arquivo temporario, mas se o usuario sair sem salvar o arquivo permanece
                    //if (File.Exists(v_dirdestino + "\\" + v_imgtemp))
                    //    File.Delete(v_dirdestino + "\\" + v_imgtemp);

                    if (File.Exists(v_dirdestino + "\\1.jpg"))
                        File.Delete(v_dirdestino + "\\1.jpg");

                    image.Save(v_dirdestino + "\\1.jpg");
                }

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), null, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        #endregion

        #region PERFIL VISITANTES

        [WebMethod]
        public string getListVisitantes()
        {
            //_visitantePerfilService = _container.Resolve<IVisitantePerfilServico>();
            //var lista = _visitantePerfilService.ObterVisitantes(_perfilLogado.UsuarioId, 5);
            var lista = _visitantePerfilService.ObterVisitantes(5);

            string v_html = string.Empty;

            if (lista.Count() > 0)
            {
                v_html += @"
                                <h2 class='ms-cnt-blc-lnk-tit'><a href='javascript:void(0);'>" + UIConfig.Visitantesdoperfil + @"</a></h2>
                                <div class='gl-cnt-zoom'>
                            ";

                foreach (var objeto in lista)
                {
                    Perfil perfil = objeto;

                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfil.Alias, perfil.UsuarioId);
                    urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + perfil.UsuarioId.ToString() + "/1.jpg", false);

                    #region HTML

                    v_html += @"
                        <div class='gl-zoom gl-zoom-visitante tooltip2' title='" + perfil.Nome + " " + perfil.Sobrenome + @"'>
                            <a href='" + urlEncryptadaPerfil + "' cp='" + perfil.UsuarioId.ToString() + @"'>
                                <img class='gl-zoom-img' src='" + urlResolve + @"' />
                            </a>
                        </div>
                    ";

                    #endregion
                }

                v_html += @"
                            </div>
                        ";
            }

            return v_html;
        }

        #endregion

        #region PERFIL GET BLOCOS

        [WebMethod]
        public object[] getBlocoCmm(Perfil perfilView)
        {
            var v_html = string.Empty;

            var validacao = new PerfilValidacao(_perfilLogado.UsuarioId, perfilView.UsuarioId);

            if (validacao.IsMeuPerfil() || validacao.IsAmigo())
            {
                //_membroServico = _container.Resolve<IMembroServico>();

                int totalRecords;
                var membros = _membroService.ObterComunidades(perfilView.UsuarioId, 1, FuncaoSite.TOTAL_ITEM_BLOCO, out totalRecords).ToList();

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilView.Alias, perfilView.UsuarioId) + "/comunidades";

                #region HTML

                v_html = @"
                    <h2 class='ms-cnt-blc-lnk-tit'><a href='" + urlEncryptadaPerfil + "'>Comunidades (" + totalRecords.ToString() + @")</a></h2>
                    <div class='gl-cnt-zoom'>
                ";

                #endregion

                foreach (var membro in membros)
                {
                    var comunidade = membro.Comunidade;

                    urlEncryptadaCmm = FuncaoSite.getUrlNomeIdCmm(comunidade.Alias);
                    urlResolve = Funcao.ResolveServerUrl("~/img/cmm/" + comunidade.ComunidadeId.ToString() + "/1.jpg", false);

                    #region HTML

                    v_html += @"
                        <div class='gl-zoom tooltip2' codcmm='" + comunidade.ComunidadeId.ToString() + "' title='" + comunidade.Nome + @"'>
                         <a href='" + urlEncryptadaCmm + "' cc='" + comunidade.ComunidadeId.ToString() + @"'>
                             <span class='gl-zoom-caption'>
                                 <h3>" + comunidade.Nome + @"</h3>
                             </span>
                             <img class='gl-zoom-img' src='" + urlResolve + @"' />
                         </a>
                        </div>
                    ";

                    #endregion
                }

                v_html += "</div>";
            }

            object[] arrRetorno = new object[1];
            arrRetorno[0] = v_html;

            return arrRetorno;
        }

        [WebMethod]
        public object[] getBlocoAmigos(Perfil perfilView)
        {
            string v_html = string.Empty;

            var validacao = new PerfilValidacao(_perfilLogado.UsuarioId, perfilView.UsuarioId);

            if (validacao.IsMeuPerfil() || validacao.IsAmigo())
            {
                int totalRecords;
                var amigos = _amizadeService.ObterAmigos(perfilView.UsuarioId, 1, FuncaoSite.TOTAL_ITEM_BLOCO, out totalRecords).ToList();

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilView.Alias, perfilView.UsuarioId) + "/amigos";

                #region HTML

                v_html = @"
                    <h2 class='ms-cnt-blc-lnk-tit'><a href='" + urlEncryptadaPerfil + "'>" + UIConfig.Amigos + " (" + totalRecords.ToString() + @")</a></h2>
                    <div class='gl-cnt-zoom'>
                ";

                #endregion

                foreach (var amigo in amigos)
                {
                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(amigo.Perfil.Alias, amigo.UsuarioId);
                    urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + amigo.UsuarioId.ToString() + "/1.jpg", false);

                    #region HTML

                    v_html += @"
                        <div class='gl-zoom tooltip2' codperfil='" + amigo.UsuarioId.ToString() + "' title='" + amigo.Perfil.Nome + " " + amigo.Perfil.Sobrenome + @"'>
                            <a href='" + urlEncryptadaPerfil + "' cp='" + amigo.UsuarioId.ToString() + @"'>
                                <span class='gl-zoom-caption'>
                                    <h3>" + amigo.Perfil.Nome + " " + amigo.Perfil.Sobrenome + @"</h3>
                                </span>
                                <img class='gl-zoom-img' src='" + urlResolve + @"' />
                            </a>
                        </div>
                    ";

                    #endregion
                }

                v_html += "</div>";
            }

            object[] arrRetorno = new object[1];
            arrRetorno[0] = v_html;

            return arrRetorno;
        }

        #endregion

        #region PERFIL AMIGOS

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> AddAmigo(AdicionarAmigoModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_amizadeService.AdicionarSolicitacaoAmizade(_perfilLogado.UsuarioId, model.UsuarioIdSolicitado);
                _amizadeService.AdicionarSolicitacaoAmizade(model.UsuarioIdSolicitado);

                var perfilView = _perfilService.Obter(model.UsuarioIdSolicitado);
                var retorno = @"<li class='menu-li'><div class='icon-aguradamigo'></div><a cp='" + model.UsuarioIdSolicitado.ToString() + "' href='javascript:void(0);'>Esperando " + perfilView.Nome + " aceitar</a></li>";

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), retorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> ExcluirAmigo(ExcluirAmigoModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_amizadeService.ExcluirAmigo(_perfilLogado.UsuarioId, model.UsuarioIdParaExcluir);
                _amizadeService.ExcluirAmigo(model.UsuarioIdParaExcluir);
                var retorno = "<li class='menu-li'><div class='icon-addamigo'></div><a cp='" + model.UsuarioIdParaExcluir.ToString() + "' class='btnViewPerfil' load='add' href='javascript:void(0);'>Adicionar Amigo</a></li>";
                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), retorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> getPerfilAmigos(Guid p_perfilView, int page)
        {
            ResponseService responseService = null;

            try
            {
                var validacao = new PerfilValidacao(_perfilLogado.UsuarioId, p_perfilView);

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
                    var amigos = _amizadeService.ObterAmigos(p_perfilView, page, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                    v_html = @"
                        <div id='containerAmigosAdicionados' class='ls-cnt-default poseydon-amigos-list-size'>
                            <ul class='list-ul'>
                    ";

                    foreach (var amigo in amigos)
                    {
                        v_codperfil = amigo.UsuarioId.ToString();

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

                        urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(amigo.Perfil.Alias, amigo.UsuarioId);

                        urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + v_codperfil + "/1.jpg", false);

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
                            <h1 class='ms-tit-blc-txt icon-people'>Amigos <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                    #endregion

                    var perfilView = _perfilService.Obter(p_perfilView);

                    #region PAGINACAO

                    v_html_paginacao = FuncaoSite.getPaginacaoPerfil(page, totalRecords, perfilView, FuncaoSite.NomePagina.AMIGO);

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

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> getPerfilAmigosPendente(int page)
        {
            ResponseService responseService = null;

            try
            {
                Guid v_codperfil;
                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;
                var v_html_tooltip = string.Empty;
                var v_html_titulo = string.Empty;
                var v_html_paginacao = string.Empty;

                int totalRecords;
                //var solicitacoesRecebidasPendentes =
                //    _amizadeService.SolicitacoesRecebidasPendentes(_perfilLogado.UsuarioId, page, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();
                var solicitacoesRecebidasPendentes =
                    _amizadeService.SolicitacoesRecebidasPendentes(page, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                #region LISTA SOLICITACOES DE AMIZADE

                v_html = @"
                            <div class='ls-cnt-default poseydon-amigos-list-size amigos-pendente'>
                                <ul class='list-ul'>
                        ";

                foreach (var solicitacao in solicitacoesRecebidasPendentes)
                {
                    var usuarioSolicitacao = solicitacao.SolicitadoPor;
                    v_codperfil = usuarioSolicitacao.UsuarioId;

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

                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioSolicitacao.Perfil.Alias, usuarioSolicitacao.UsuarioId);
                    urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioSolicitacao.UsuarioId.ToString() + "/1.jpg", false);

                    v_html += @"
                                <li class='list-li pendente' cppend='" + usuarioSolicitacao.UsuarioId.ToString() + @"'>
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
                            <h1 class='ms-tit-blc-txt icon-people'>Amigos Pendentes <span class='ms-tit-blc-btn'>" + totalRegistros.ToString() + @"</span></h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                #endregion

                var perfilView = _perfilLogado; // _perfilService.Obter(_perfilLogado);

                #region PAGINACAO

                v_html_paginacao = FuncaoSite.getPaginacaoPerfil(page, totalRegistros, perfilView, FuncaoSite.NomePagina.AMIGOPENDENTE);

                #endregion

                object[] arrRetorno = new object[3];
                arrRetorno[0] = v_html_titulo;
                arrRetorno[1] = v_html;
                arrRetorno[2] = v_html_paginacao;

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> AceitarAmigo(AceitarSolicitacaoAmizadeModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_amizadeService.AceitarSolicitacaoAmizade(_perfilLogado.UsuarioId, model.UsuarioIdAceitar);
                _amizadeService.AceitarSolicitacaoAmizade(model.UsuarioIdAceitar);
                var perfilAprovado = _perfilService.Obter(model.UsuarioIdAceitar);

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;
                var v_html_tooltip = string.Empty;
                var v_html_blocoamigos = string.Empty;

                #region TOOLTIP MENU DE OPCOES

                v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + perfilAprovado.UsuarioId.ToString() + "' opcao='AE'>Excluir amigo</div></a>";

                v_html_tooltip = @"
                    <div class='popr tooltip-dropmenu-container' codperfil='" + perfilAprovado.UsuarioId.ToString() + @"'>
                        <div class='button-group minor-group'>

                        <div class='tooltip-dropmenu'>
                            <a href='javascript:void(0);' class='button icon settings semtexto' title=''></a>
                            <div class='tooltip-dropmenu-html-container' codperfil='" + perfilAprovado.UsuarioId.ToString() + @"'>
                                <div class='tooltip-dropmenu-menu' codperfil='" + perfilAprovado.UsuarioId.ToString() + @"'>
                                    " + v_html_menuitens + @"
                                </div>
                            </div>
                            <div class='clear'></div>
                        </div>

                        </div>
                    </div>
                ";

                #endregion

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilAprovado.Alias, perfilAprovado.UsuarioId);
                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + perfilAprovado.UsuarioId.ToString() + "/1.jpg", false);

                v_html = @"
                    <li class='list-li' codperfil='" + perfilAprovado.UsuarioId.ToString() + @"'>
                        <img class='list-img' src='" + urlResolve + @"'>
                        <h3 class='list-h3'><a href='" + urlEncryptadaPerfil + "'>" + perfilAprovado.Nome + " " + perfilAprovado.Sobrenome + @"</a></h3>
                        <p class='list-p'>" + perfilAprovado.Sexo.GetDescription() + @"<span></span></p>
                        " + v_html_tooltip + @"
                    </li>
                ";

                #region ITEM DO BLOCO DE AMIGOS

                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + perfilAprovado.UsuarioId.ToString() + "/1.jpg", false);

                v_html_blocoamigos = @"
                    <div class='gl-zoom tooltip2'  title='" + perfilAprovado.Nome + " " + perfilAprovado.Sobrenome + @"'>
                        <a href='" + urlEncryptadaPerfil + "' cp='" + perfilAprovado.UsuarioId.ToString() + @"'>
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

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> RecusarAmigo(UsuarioIdRecusarModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_amizadeService.RecusarSolicitacaoAmizade(_perfilLogado.UsuarioId, model.UsuarioIdRecusar);
                _amizadeService.RecusarSolicitacaoAmizade(model.UsuarioIdRecusar);
                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), null, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        #endregion

        #region PERFIL RECADOS

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> SalvarRecado(SalvarRecadoModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_recadoServico = _container.Resolve<IRecadoServico>();
                //var recado = _recadoService.EnviarRecado(_perfilLogado.UsuarioId, _perfilView.UsuarioId, model.RecadoHtml);
                var recado = _recadoService.EnviarRecado(_perfilView.UsuarioId, model.RecadoHtml);

                var v_codrecado = recado.RecadoId.ToString();

                var perfilRecebeuRecado = _perfilView; // _perfilService.Obter(perfilView);

                #region TEMPLATE POST

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;
                var v_html_paginacao = string.Empty;

                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + _perfilLogado.UsuarioId.ToString() + "/1.jpg", false);

                v_html = @"
                        <div class='tl-cnt-default-block postado-recente' codrecado='" + recado.RecadoId.ToString() + "' codperfil='" + _perfilLogado.UsuarioId.ToString() + @"'>
                            <div class='tl-cnt-default-img cd-picture'>
                                    <img src='" + urlResolve + @"' alt='Picture' />
                            </div> <!-- tl-cnt-default-img -->

                            <div class='tl-cnt-default-content'>
                    ";

                #region TOOLTIP MENU DE OPCOES

                if (_perfilView.UsuarioId == _perfilLogado.UsuarioId)
                {
                    v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' codrecado='" + v_codrecado.ToString() + "' opcao='RE'>" + UIConfig.Excluirrecado + @"</div></a>";
                }
                else
                {
                    v_html_menuitens = @"
                        <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codrecado='" + v_codrecado.ToString() + @"' opcao='RE'>" + UIConfig.Excluirrecado + @"</div></a>
                        <a href='javascript:void(0);'><div class='menuitem btnMenuItem' codperfil='" + _perfilLogado.UsuarioId.ToString() + @"' opcao='RB'>" + UIConfig.BloquearPostagensDessePerfil + @"</div></a>
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

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilRecebeuRecado.Alias, perfilRecebeuRecado.UsuarioId);
                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + perfilRecebeuRecado.UsuarioId.ToString() + "/1.jpg", false);

                v_html += "<h2 class='tl-h2'><a href='" + urlEncryptadaPerfil + "'>" + _perfilLogado.Nome + " " + _perfilLogado.Sobrenome + "</a><span data-utime='1371248446' class='post-dt'>" + FuncaoSite.getTempoPost(recado.DataRecado) + "</span></h2>";
                v_html += @"
                                <h3 class='tl-h3'><span>&nbsp;</span></h3>

                            <div class='tl-cnt-default-img-para'>
                            <span>Para: </span>
                            <img src='" + urlResolve + @"' class='cmt-cnt-img-perfil-tooltip-big' alt='Picture' />
                        ";

                //urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioRecebeuRecado.Perfil.Alias, usuarioRecebeuRecado.UsuarioId);

                #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + perfilRecebeuRecado.UsuarioId.ToString() + "/1.jpg", false);

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

                // CONTAINER EDITOR

                v_html += @"
                                    <a href='javascript:void(0);' class='tl-read-more'>Read more</a>
                                    <span class='tl-date'>há 29 segundos</span>
                                </div><!-- tl-cnt-default-content -->
                            </div><!-- tl-cnt-default-block -->
                        ";

                #endregion

                #region PAGINACAO

                v_html_paginacao = FuncaoSite.getPaginacaoPerfil(1, 1, _perfilLogado, FuncaoSite.NomePagina.RECADO);

                #endregion

                object[] arrRetorno = new object[2];
                arrRetorno[0] = v_html;
                arrRetorno[1] = v_html_paginacao;

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> ExcluirRecado(ExcluirRecadoModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_recadoServico = _container.Resolve<IRecadoServico>();
                //_recadoService.ExcluirRecado(model.RecadoIdParaExcluir, _perfilLogado.UsuarioId);
                _recadoService.ExcluirRecado(model.RecadoIdParaExcluir);
                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), null, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> ExcluirMultiplosRecados(List<ExcluirRecadoModel> model)
        {
            ResponseService responseService = null;

            try
            {
                //_recadoServico = _container.Resolve<IRecadoServico>();
                foreach (var recado in model)
                    _recadoService.ExcluirRecado(recado.RecadoIdParaExcluir);
                //_recadoService.ExcluirRecado(recado.RecadoIdParaExcluir, _perfilLogado.UsuarioId);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), null, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> SalvarRecadoComentario(SalvarRecadoComentarioModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_recadoServico = _container.Resolve<IRecadoServico>();
                //_recadoComentarioServico = _container.Resolve<IRecadoComentarioServico>();
                //_perfilBloqueadoServico = _container.Resolve<IPostPerfilBloqueadoServico>();

                var recado = _recadoService.ObterRecado(model.RecadoId);
                var usuarioComPostBloqueado = _perfilBloqueadoService.ObterPerfilBloqueado(recado.EnviadoPorId, _perfilLogado.UsuarioId);
                if (usuarioComPostBloqueado != null)
                    throw new Exception(Errors.ErroAoEnviarComentario);

                //var comentario = _recadoComentarioService.Comentar(model.RecadoId, _perfilLogado.UsuarioId, model.Comentario);
                var comentario = _recadoComentarioService.Comentar(model.RecadoId, model.Comentario);

                #region TEMPLATE COMENTARIO

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;

                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + _perfilLogado.UsuarioId.ToString() + "/1.jpg", false);

                v_html += @"
                        <div class='cmt-cnt' codcoment='" + comentario.RecadoComentarioId.ToString() + @"' style='display:none;'>
                            <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                    ";

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(_perfilLogado.Alias, _perfilLogado.UsuarioId);

                #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + _perfilLogado.UsuarioId.ToString() + "/1.jpg", false);

                v_html += @"
                    <div class='tooltip-dropmenu-html-container'>
                        <div class='tooltip'>
                            <span class='tooltip-content'>
                                <img src='" + urlResolve + @"' alt='' />
                                <span class='tooltip-text'>
                                    <a class='tooltip-text-a' href='" + urlEncryptadaPerfil + "'>" + _perfilLogado.Nome + " " + _perfilLogado.Sobrenome + "</a><hr><span class='tooltip-text-span'>" + _perfilLogado.FraseHtml + @"</span> 
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
                                 <h5><a href='" + urlEncryptadaPerfil + "'>" + _perfilLogado.Nome + " " + _perfilLogado.Sobrenome + "</a></h5><span data-utime='1371248446' class='com-dt'>" + FuncaoSite.getTempoPost(DateTime.Now) + @"</span>
                                 <div class='com-text'>" + comentario.ComentarioHtml + @"</div>
                             </div>
                        </div><!-- end 'cmt-cnt' -->
                    ";

                #endregion

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), v_html, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> ExcluirRecadoComentario(ExcluirRecadoComentarioModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_recadoComentarioServico = _container.Resolve<IRecadoComentarioServico>();
                //_recadoComentarioService.ExcluirComentario(model.RecadoComentarioId, _perfilLogado.UsuarioId);
                _recadoComentarioService.ExcluirComentario(model.RecadoComentarioId);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), null, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> getListRecados(Guid p_perfilView, int page)
        {
            ResponseService responseService = null;

            try
            {
                //_privacidadeService = _container.Resolve<IPrivacidadeServico>();
                //_perfilBloqueadoServico = _container.Resolve<IPostPerfilBloqueadoServico>();
                //_recadoServico = _container.Resolve<IRecadoServico>();
                //_recadoComentarioServico = _container.Resolve<IRecadoComentarioServico>();

                var validacao = new PerfilValidacao(_perfilLogado.UsuarioId, p_perfilView);
                var isAmigo = validacao.IsAmigo();

                string v_html = string.Empty;
                var v_html_menuitens = string.Empty;
                var v_html_titulo = string.Empty;
                var v_html_paginacao = string.Empty;
                var v_html_editor = string.Empty;
                var v_html_btntop = string.Empty;
                var v_html_btnvermais = string.Empty;

                int totalRecords = 0;
                var nextPage = page + 1;

                //var priv = _privacidadeService.Obter(p_perfilView);
                var priv = _privacidadeService.Obter();

                if (validacao.IsMeuPerfil() || isAmigo)
                {
                    var recados = _recadoService.ObterRecadosEnviadosERecebidos(p_perfilView, page, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

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

                            urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioRecado.UsuarioId.ToString() + "/1.jpg", false);

                            v_html += @"
                                        <div class='tl-cnt-default-block' codrecado='" + recado.RecadoId.ToString() + @"' style='display:none;'>
                                            <div class='tl-cnt-default-img cd-picture'>
                                                    <img src='" + urlResolve + @"' alt='Picture' />
                                            </div> <!-- tl-cnt-default-img -->

                                            <div class='tl-cnt-default-content'>
                                    ";

                            #region TOOLTIP MENU DE OPCOES

                            if (usuarioRecado.UsuarioId == _perfilLogado.UsuarioId)
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

                            urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioRecado.Perfil.Alias, usuarioRecado.UsuarioId);
                            urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioRecebeuRecado.UsuarioId.ToString() + "/1.jpg", false);

                            v_html += "<h2 class='tl-h2'><a href='" + urlEncryptadaPerfil + "'>" + usuarioRecado.Perfil.Nome + " " + usuarioRecado.Perfil.Sobrenome + "</a><span data-utime='1371248446' class='post-dt'>" + FuncaoSite.getTempoPost(recado.DataRecado) + "</span></h2>";
                            v_html += @"
                                             <h3 class='tl-h3'><span>&nbsp;</span></h3>

                                        <div class='tl-cnt-default-img-para'>
                                        <span>Para: </span>
                                        <img src='" + urlResolve + @"' class='cmt-cnt-img-perfil-tooltip-big' alt='Picture' />
                                    ";

                            urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioRecebeuRecado.Perfil.Alias, usuarioRecebeuRecado.UsuarioId);

                            #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                            urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioRecebeuRecado.UsuarioId.ToString() + "/1.jpg", false);

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

                            #region CONTAINER EDITOR

                            var usuarioDoRecadoBloqueadoParaPostarNoUsuarioPost =
                                _perfilBloqueadoService.ObterPerfilBloqueado(usuarioRecado.UsuarioId, _perfilLogado.UsuarioId);

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

                                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                                v_html += @"
                                                <div class='cmt-cnt' codcoment='" + comentario.RecadoComentarioId.ToString() + @"'>
                                                    <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                                            ";

                                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioComentario.Perfil.Alias, usuarioComentario.UsuarioId);

                                #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

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

                                if (usuarioComentario.UsuarioId == _perfilLogado.UsuarioId)
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

                            v_html += @"
                                    		    <a href='javascript:void(0);' class='tl-read-more'>Read more</a>
                                             <span class='tl-date'>há 29 segundos</span>
                                         </div><!-- tl-cnt-default-content -->
                                        </div><!-- tl-cnt-default-block -->
                                    ";

                            #endregion

                            if (usuarioRecebeuRecado.UsuarioId == _perfilLogado.UsuarioId)
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

                        var proximosPosts = _recadoService.ObterRecadosEnviadosERecebidos(p_perfilView, nextPage, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                        #region BOTAO VER MAIS

                        if (proximosPosts.Count > 0)
                        {
                            urlResolve = Funcao.ResolveServerUrl("~/img/cd-icon-location.svg", false);
                            v_html_btnvermais = @"<div class='tl-cnt-default-img-vermais cd-location btnVerMaisPost' page='" + nextPage.ToString() + @"'>
                                            <img src='" + urlResolve + @"' alt='Location'>
                                         </div>";
                        }

                        #endregion
                    }

                    #region TITULO

                    v_html_titulo = @"
                        <div class='ms-cnt-tit-blc'>
                            <h1 class='ms-tit-blc-txt icon-recado'>Meus recados <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                    #endregion

                    var perfilView = _perfilService.Obter(p_perfilView);

                    #region PAGINACAO

                    v_html_paginacao = FuncaoSite.getPaginacaoPerfil(page, totalRecords, perfilView, FuncaoSite.NomePagina.RECADO);

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

                object[] arrRetorno = new object[7];
                arrRetorno[0] = v_html_titulo;
                arrRetorno[1] = v_html;
                arrRetorno[2] = v_html_paginacao;
                arrRetorno[3] = v_html_editor;
                arrRetorno[4] = v_html_btntop;
                arrRetorno[5] = v_html_btnvermais;
                arrRetorno[6] = totalRecords;

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> getListRecadoComentarios(Guid recadoId, int page)
        {
            ResponseService responseService = null;

            try
            {
                //_recadoComentarioServico = _container.Resolve<IRecadoComentarioServico>();

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;

                var nextPage = page + 1;

                #region COMENTARIOS DO POST

                var comentarios = _recadoComentarioService.ObterComentarios(recadoId, page, FuncaoSite.TOTAL_COMENT_PAGE);

                foreach (var comentario in comentarios)
                {
                    var usuarioComentario = comentario.Usuario;

                    urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                    v_html += @"
                        <div class='cmt-cnt' codcoment='" + comentario.RecadoComentarioId.ToString() + @"' style='display:none;'>
                            <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                    ";

                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioComentario.Perfil.Alias, usuarioComentario.UsuarioId);

                    #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                    urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

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

                    if (usuarioComentario.UsuarioId == _perfilLogado.UsuarioId)
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

                var proximosComentarios = _recadoComentarioService.ObterComentarios(recadoId, nextPage, FuncaoSite.TOTAL_COMENT_PAGE).ToList();
                if (proximosComentarios.Count > 0)
                {
                    v_html += @"
                                <div class='ed-cnt-cmt-more button-group minor-group'>
                                        <a href='javascript:void(0);' class='button minor1 btnVerMais' codrecado='" + recadoId.ToString() + "' page='" + nextPage.ToString() + "'>" + BTN_VER_MAIS_COMENTS + @"</a>
                            ";

                    if (page > 1)
                        v_html += @"<a href='javascript:void(0);' class='button minor1 btnVerMenos' codrecado='" + recadoId.ToString() + "' >" + BTN_VER_MENOS_COMENTS + "</a>";

                    v_html += @"
                            </div>
                        ";
                }
                else if (page > 1)
                {
                    v_html += @"
                                <div class='ed-cnt-cmt-more button-group minor-group'>
                                     <a href='javascript:void(0);' class='button minor1 btnVerMenos' codrecado='" + recadoId.ToString() + "' >" + BTN_VER_MENOS_COMENTS + @"</a>
                                </div>
                            ";
                }

                #endregion

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), v_html.ToString(), Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        #endregion

        #region PERFIL DEPOIMENTOS

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> SalvarDepoimento(SalvarDepoimentoModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_depoimentoServico = _container.Resolve<IDepoimentoServico>();
                //var depoimento = _depoimentoService.EnviarDepoimento(_perfilLogado.UsuarioId, _perfilView.UsuarioId, model.Depoimento);
                var depoimento = _depoimentoService.EnviarDepoimento(_perfilView.UsuarioId, model.Depoimento);

                #region TEMPLATE POST

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;

                var perfilDepoimento = _perfilLogado; // _perfilService.Obter(_perfilLogado);

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

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilDepoimento.Alias, perfilDepoimento.UsuarioId);

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

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), v_html, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> ExcluirDepoimento(ExcluirDepoimentoModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_depoimentoServico = _container.Resolve<IDepoimentoServico>();
                //_depoimentoService.ExcluirDepoimento(model.DepoimentoId, _perfilLogado.UsuarioId);
                _depoimentoService.ExcluirDepoimento(model.DepoimentoId);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), null, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> AceitarDepoimento(AceitarRecusarDepoimentoModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_depoimentoServico = _container.Resolve<IDepoimentoServico>();
                _depoimentoService.AceitarDepoimento(model.DepoimentoId);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), null, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> RecusarDepoimento(AceitarRecusarDepoimentoModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_depoimentoServico = _container.Resolve<IDepoimentoServico>();
                _depoimentoService.RecusarDepoimento(model.DepoimentoId);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), null, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> getListDepoimentos(Guid p_perfilView, int page)
        {
            ResponseService responseService = null;

            try
            {
                //_depoimentoServico = _container.Resolve<IDepoimentoServico>();

                var validacao = new PerfilValidacao(_perfilLogado.UsuarioId, p_perfilView);

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;
                var v_html_titulo = string.Empty;
                var v_html_paginacao = string.Empty;
                var v_html_editor = string.Empty;

                var status_depo = "";

                if (validacao.IsMeuPerfil() || validacao.IsAmigo())
                {
                    int totalRecords;
                    var depoimentos = _depoimentoService.ObterDepoimentosRecebidos(p_perfilView, page, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                    #region CONTAINER TIMELINE

                    v_html += "<section class='tl-cnt-default'>";

                    if (depoimentos.Count > 0)
                    {
                        foreach (var depoimento in depoimentos)
                        {
                            var usuarioDepoimento = depoimento.EnviadoPor;

                            #region CONTAINER TIMELINE ITEM

                            urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioDepoimento.UsuarioId.ToString() + "/1.jpg", false);

                            v_html += @"
                                <div class='tl-cnt-default-block' cd='" + depoimento.DepoimentoId.ToString() + @"'>
                                    <div class='tl-cnt-default-img cd-picture'>
                                        <img src='" + urlResolve + @"' alt='Picture' />
                                    </div> <!-- tl-cnt-default-img -->

                                    <div class='tl-cnt-default-content'>
                            ";

                            if (validacao.IsMeuPerfil())
                            {
                                if (usuarioDepoimento.UsuarioId != _perfilLogado.UsuarioId)
                                {
                                    #region TOOLTIP MENU DE OPCOES ITENS

                                    if (depoimento.StatusDepoimento == StatusDepoimento.Pendente)
                                    {
                                        v_html_menuitens = @"
                                            <a href='javascript:void(0);'><div class='menuitem btnMenuItem' cd='" + depoimento.DepoimentoId.ToString() + @"' opcao='A'>Aceitar</div></a>
                                            <a href='javascript:void(0);'><div class='menuitem btnMenuItem' cd='" + depoimento.DepoimentoId.ToString() + @"' opcao='R'>Recusar</div></a>
                                        ";

                                        status_depo = "<i>Você ainda não aceitou este depoimento<i>";
                                    }
                                    else
                                    {
                                        v_html_menuitens = "<a href='javascript:void(0);'><div class='menuitem btnMenuItem' cd='" + depoimento.DepoimentoId.ToString() + "' opcao='E'>Excluir</div></a>";

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
                                if (usuarioDepoimento.UsuarioId == _perfilLogado.UsuarioId)
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

                            urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioDepoimento.Perfil.Alias, usuarioDepoimento.UsuarioId);

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
                            <h1 class='ms-tit-blc-txt icon-depo'>Meus depoimentos <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                    #endregion

                    var perfilView = _perfilService.Obter(p_perfilView);

                    #region PAGINACAO

                    v_html_paginacao = FuncaoSite.getPaginacaoPerfil(page, totalRecords, perfilView, FuncaoSite.NomePagina.DEPOIMENTO);

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
                            <h1 class='ms-tit-blc-txt icon-depo'>Depoimentos</h1>
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

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        #endregion

        #region PERFIL VIDEOS

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> SalvarVideo(SalvarVideoModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_videoServico = _container.Resolve<IVideoServico>();
                //var video = _videoService.SalvarVideo(_perfilLogado.UsuarioId, model.Url, model.Titulo);
                var video = _videoService.SalvarVideo(model.Url, model.Titulo);

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

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), v_html, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> ExcluirVideo(ExcluirVideoModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_videoServico = _container.Resolve<IVideoServico>();
                _videoService.ExcluirVideo(model.VideoId);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), null, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> SalvarVideoComentario(SalvarVideoComentarioModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_videoComentarioServico = _container.Resolve<IVideoComentarioServico>();
                //var comentario = _videoComentarioService.Comentar(model.VideoId, _perfilLogado.UsuarioId, model.Comentario);
                var comentario = _videoComentarioService.Comentar(model.VideoId, model.Comentario);

                #region COMENTARIOS DO VIDEO

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;

                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + _perfilLogado.UsuarioId.ToString() + "/1.jpg", false);

                v_html += @"
                    <div class='cmt-cnt postado-recente' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
                        <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                ";

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(_perfilLogado.Alias, _perfilLogado.UsuarioId);

                #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + _perfilLogado.UsuarioId.ToString() + "/1.jpg", false);

                v_html += @"
                    <div class='tooltip-dropmenu-html-container'>
                        <div class='tooltip'>
                            <span class='tooltip-content'>
                                <img src='" + urlResolve + @"' alt='' />
                                <span class='tooltip-text'>
                                    <a class='tooltip-text-a' href='" + urlEncryptadaPerfil + "'>" + _perfilLogado.Nome + " " + _perfilLogado.Sobrenome + "</a><hr><span class='tooltip-text-span'>" + _perfilLogado.FraseHtml + @"</span> 
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
                             <h5><a href='" + urlEncryptadaPerfil + "'>" + _perfilLogado.Nome + " " + _perfilLogado.Sobrenome + "</a></h5><span data-utime='1371248446' class='com-dt'>" + FuncaoSite.getTempoPost(DateTime.Now) + @"</span>
                             <div class='com-text'>" + comentario.ComentarioHtml + @"</div>
                         </div>
                    </div><!-- end 'cmt-cnt' -->
                ";

                #endregion

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), v_html, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> ExcluirVideoComentario(ExcluirVideoComentarioModel model)
        {
            ResponseService responseService = null;

            try
            {
                //_videoComentarioServico = _container.Resolve<IVideoComentarioServico>();
                //_videoComentarioService.ExcluirComentario(model.VideoComentarioId, _perfilLogado.UsuarioId);
                _videoComentarioService.ExcluirComentario(model.VideoComentarioId);

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), null, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> getListVideos(Guid p_perfilView, int page)
        {
            ResponseService responseService = null;

            try
            {
                //_videoServico = _container.Resolve<IVideoServico>();
                //_videoComentarioServico = _container.Resolve<IVideoComentarioServico>();

                var validacao = new PerfilValidacao(_perfilLogado.UsuarioId, p_perfilView);

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;
                var v_html_titulo = string.Empty;
                var v_html_paginacao = string.Empty;
                var v_html_form = string.Empty;

                var nextPage = page + 1;

                if (validacao.IsMeuPerfil() || validacao.IsAmigo())
                {
                    int totalRecords;
                    var videos = _videoService.ObterVideos(p_perfilView, page, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

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

                            var comentarios = _videoComentarioService.ObterComentarios(video.VideoId, page, FuncaoSite.TOTAL_COMENT_PAGE).ToList();

                            v_html += "<div class='ed-cnt-cmt-coments-temp'>";

                            foreach (var comentario in comentarios)
                            {
                                var usuarioComentario = comentario.Usuario;

                                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                                v_html += @"
                                    <div class='cmt-cnt' codcoment='" + comentario.VideoComentarioId.ToString() + @"'>
                                        <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                                ";

                                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioComentario.Perfil.Alias, usuarioComentario.UsuarioId);

                                #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                                urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

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

                                if (usuarioComentario.UsuarioId == _perfilLogado.UsuarioId)
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
                            <h1 class='ms-tit-blc-txt icon-video'>Meus vídeos <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                            <div class='ms-tit-blc-line'></div>
                        </div>
                    ";

                    #endregion

                    var perfilView = _perfilService.Obter(p_perfilView);

                    #region PAGINACAO

                    v_html_paginacao = FuncaoSite.getPaginacaoPerfil(page, totalRecords, perfilView, FuncaoSite.NomePagina.VIDEO);

                    #endregion

                    #region FORM

                    if (validacao.IsMeuPerfil())
                    {
                        v_html_form = @"
                            <div id='containerForm' class='fm-cnt-default'>

                                <div class='box-form'>
                                    <fieldset>
                                        <div class='half-width' style='width:100%;'>
                                            <label for='txtUrlVideo'>URL do vídeo</label>
                                            <input type='text' class='txtUrlVideo obrigatorio' name='txtUrlVideo' maxlength='100' />
                                        </div>
                                    </fieldset>
                                </div>

                                <div class='ed-cnt-cmt-btn'>
                                    <a href='javascript:void(0);' class='button big icon approve primary btnSalvarVideo'>Salvar vídeo</a>
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

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> getListVideoComentarios(Guid videoId, int page)
        {
            ResponseService responseService = null;

            try
            {
                //_videoComentarioServico = _container.Resolve<IVideoComentarioServico>();

                var v_html = string.Empty;
                var v_html_menuitens = string.Empty;

                var nextPage = page + 1;

                #region COMENTARIOS DO POST

                var comentarios = _videoComentarioService.ObterComentarios(videoId, page, FuncaoSite.TOTAL_COMENT_PAGE).ToList();

                foreach (var comentario in comentarios)
                {
                    var usuarioComentario = comentario.Usuario;

                    urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

                    v_html += @"
                        <div class='cmt-cnt' codcoment='" + comentario.VideoComentarioId.ToString() + @"' style='display:none;'>
                            <img class='cmt-cnt-img-perfil cmt-cnt-img-perfil-tooltip-big' src='" + urlResolve + @"' />
                    ";

                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(usuarioComentario.Perfil.Alias, usuarioComentario.UsuarioId);

                    #region TOOLTIP MAIOR COM FOTO E INFORMACOES DO PERFIL

                    urlResolve = Funcao.ResolveServerUrl("~/img/perfil/" + usuarioComentario.UsuarioId.ToString() + "/1.jpg", false);

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

                    if (usuarioComentario.UsuarioId == _perfilLogado.UsuarioId)
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

                var proximosComentarios = _videoComentarioService.ObterComentarios(videoId, nextPage, FuncaoSite.TOTAL_COMENT_PAGE).ToList();
                if (proximosComentarios.Count > 0)
                {
                    v_html += @"
                        <div class='ed-cnt-cmt-more button-group minor-group'>
                            <a href='javascript:void(0);' class='button minor1 btnVerMais' codvideo='" + videoId.ToString() + "' page='" + nextPage.ToString() + "'>" + BTN_VER_MAIS_COMENTS + @"</a>
                    ";

                    if (page > 1)
                        v_html += @"<a href='javascript:void(0);' class='button minor1 btnVerMenos' codvideo='" + videoId.ToString() + "' >" + BTN_VER_MENOS_COMENTS + "</a>";

                    v_html += @"
                        </div>
                    ";
                }
                else if (page > 1)
                {
                    v_html += @"
                            <div class='ed-cnt-cmt-more button-group minor-group'>
                                 <a href='javascript:void(0);' class='button minor1 btnVerMenos' codvideo='" + videoId.ToString() + "' >" + BTN_VER_MENOS_COMENTS + @"</a>
                            </div>
                        ";
                }

                #endregion

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), v_html, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        #endregion

        #region PERFIL COMUNIDADES

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> getListComunidades(Guid p_perfilView, int page)
        {
            ResponseService responseService = null;

            try
            {
                var validacao = new PerfilValidacao(_perfilLogado.UsuarioId, p_perfilView);

                var v_html = string.Empty;
                var v_html_titulo = string.Empty;
                var v_html_paginacao = string.Empty;

                if (validacao.IsMeuPerfil() || validacao.IsAmigo())
                {
                    //_membroServico = _container.Resolve<IMembroServico>();
                    //_topicoServico = _container.Resolve<ITopicoServico>();
                    //_comunidadeServico = _container.Resolve<IComunidadeServico>();

                    int totalRecords;
                    var membros = _membroService.ObterComunidades(p_perfilView, page, FuncaoSite.TOTAL_POST_PAGE, out totalRecords).ToList();

                    if (membros.Count > 0)
                    {
                        #region CONTAINER

                        v_html = @"
                                    <div class='ls-cnt-default poseydon-cmm-pesquisa-list-size'>
                                    <ul class='list-ul'>
                                ";

                        foreach (var membro in membros)
                        {
                            var comunidade = membro.Comunidade;
                            var perfilDonoCmm = comunidade.Usuario.Perfil;

                            var ultimoPost = _comunidadeService.ObterUltimoPost(comunidade.ComunidadeId);

                            int totalTpc;
                            _topicoService.ObterTopicos(comunidade.ComunidadeId, 1, 0, out totalTpc);

                            urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilDonoCmm.Alias, perfilDonoCmm.UsuarioId);
                            urlEncryptadaCmm = FuncaoSite.getUrlNomeIdCmm(comunidade.Alias);
                            urlResolve = Funcao.ResolveServerUrl("~/img/cmm/" + comunidade.ComunidadeId.ToString() + "/1.jpg", false);

                            v_html += "<li class='list-li'>";

                            if (ultimoPost == null)
                                v_html += "<div class='ultimopost'><span>Último post</span>-</div>";
                            else
                                v_html += "<div class='ultimopost'><span>Último post</span>" + FuncaoSite.getTempoPost(ultimoPost.DataPost) + "</div>";

                            v_html += @"
                                    <div class='totalpost'><span>topicos</span>" + totalTpc.ToString() + @"</div>
                                        <div class='autorpost'><span>dono</span><a href='" + urlEncryptadaPerfil + "' class='lastpost'>" + perfilDonoCmm.Nome + " " + perfilDonoCmm.Sobrenome + @"</a></div>
                                        <img class='list-img' src='" + urlResolve + @"'>
                                        <h3 class='list-h3'><a href='" + urlEncryptadaCmm + "'>" + comunidade.Nome + @"</a></h3>
                                        <p class='list-p' style='display:none;'>Último post<span> - </span><!-- - <a href='#0' class='lastpost'>Pф§eiÐфи ψ E§pilДcфpД</a>--></p>
                                    </li>
                                ";
                        }

                        v_html += @"
                            </ul>
                            </div>
                        ";

                        #endregion
                    }

                    #region TITULO

                    v_html_titulo = @"
                                <div class='ms-cnt-tit-blc'>
                                    <h1 class='ms-tit-blc-txt icon-cmm'>Minhas Comunidades <span class='ms-tit-blc-btn'>" + totalRecords.ToString() + @"</span></h1>
                                    <div class='ms-tit-blc-line'></div>
                                </div>
                            ";

                    #endregion

                    #region PAGINACAO

                    v_html_paginacao = FuncaoSite.getPaginacaoPerfil(page, totalRecords, _perfilView, FuncaoSite.NomePagina.COMUNIDADES);

                    #endregion
                }
                else
                {
                    #region TITULO

                    v_html_titulo = @"
                                <div class='ms-cnt-tit-blc'>
                                    <h1 class='ms-tit-blc-txt icon-cmm'>Minhas Comunidades</h1>
                                    <div class='ms-tit-blc-line'></div>
                                </div>
                            ";

                    #endregion
                }

                object[] arrRetorno = new object[3];
                arrRetorno[0] = v_html_titulo;
                arrRetorno[1] = v_html;
                arrRetorno[2] = v_html_paginacao;

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), arrRetorno, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        #endregion

        #region PERFIL CHAT

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Task<ResponseService> getChatMembers()
        {
            ResponseService responseService = null;

            try
            {
                string v_html = string.Empty;

                int totalRecords;
                var amigos = _amizadeService.ObterAmigos(_perfilLogado.UsuarioId, 1, FuncaoSite.TOTAL_ITEM_BLOCO, out totalRecords).ToList();

                foreach (var amigo in amigos)
                {
                    var pathImg = Server.MapPath("~/img/perfil/" + amigo.Perfil.UsuarioId.ToString() + "/1.jpg");

                    string base64Image = Conversion.ImageToBase64(pathImg);

                    #region HTML

                    v_html += @"
                        <div class='mb-attribution animated slideInUp' cp='" + amigo.Perfil.UsuarioId.ToString() + @"'>
                            <p class='mb-author'>" + amigo.Perfil.Nome + " " + amigo.Perfil.Sobrenome + @"</p>
                            <cite>" + amigo.Perfil.FraseHtml + @"</cite>
                            <div class='mb-thumb' style='background-image: url(data:image/jpeg;base64," + base64Image + @");'></div>
                            <div class='mb-notification'><span></span>12:00</div>
                        </div>
                        ";

                    #endregion
                }

                responseService = new ResponseService(HttpStatusCode.OK, HttpStatusCode.OK.ToString(), v_html, Context);
            }
            catch (Exception ex)
            {
                responseService = new ResponseService(HttpStatusCode.InternalServerError, ex.Message, null, Context);
            }

            var tsc = new TaskCompletionSource<ResponseService>();
            tsc.SetResult(responseService);
            return tsc.Task;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            //_container.Dispose();
            base.Dispose(disposing);
        }
    }
}
