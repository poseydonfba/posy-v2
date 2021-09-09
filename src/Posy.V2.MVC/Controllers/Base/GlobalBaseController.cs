using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.MVC.Helpers;
using Posy.V2.MVC.Validacao;
using System;
using System.Linq;

namespace Posy.V2.MVC.Controllers.Base
{
    public class GlobalBaseController : IGlobalBaseController
    {
        protected readonly IAmizadeService _amizadeService;
        protected readonly IMembroService _membroService;
        protected readonly IVisitantePerfilService _visitantePerfilService;
        protected readonly IRecadoService _recadoService;

        protected string urlEncryptadaPerfil;
        protected string urlEncryptadaCmm;
        protected string urlResolve;

        public GlobalBaseController(IAmizadeService amizadeService,
                                    IMembroService membroService,
                                    IVisitantePerfilService visitantePerfilService,
                                    IRecadoService recadoService)
        {
            _amizadeService = amizadeService;
            _membroService = membroService;
            _visitantePerfilService = visitantePerfilService;
            _recadoService = recadoService;
        }


        public string GetTemplateMenuVerticalPerfilHtml(Perfil PerfilLogged, Perfil PerfilView)
        {
            string v_html = string.Empty;

            var validacao = new PerfilValidacao(PerfilLogged.UsuarioId, PerfilView.UsuarioId);

            if (validacao.IsMeuPerfil()) /// Menu do meu perfil
            {
                _amizadeService.SolicitacoesRecebidasPendentes(0, 0, out int totalRecords).ToList();

                _recadoService.ObterRecadosRecebidosNaoLidos(PerfilLogged.UsuarioId, 0, 0, out int totalRecadosNaoLidos);

                string v_notif_ami = (totalRecords > 0) ? " <span>" + totalRecords.ToString() + "</span>" : "";
                string v_recados_naolidos = (totalRecadosNaoLidos > 0) ? " <span>" + totalRecadosNaoLidos.ToString() + "</span>" : "";

                string urlEdit = FuncaoSite.ROUTE_URL_EDITARPERFIL;
                string urlPerfil = FuncaoSite.getUrlNomeIdPerfil(PerfilView.Alias);
                string urlInicio = FuncaoSite.ROUTE_URL_INICIO;
                string urlAmigos = urlPerfil + "/" + FuncaoSite.ROUTE_URL_AMIGOS;
                string urlAmigosPend = urlPerfil + "/" + FuncaoSite.ROUTE_URL_AMIGOSPEND;
                string urlRecados = urlPerfil + "/" + FuncaoSite.ROUTE_URL_RECADOS;
                string urlVideos = urlPerfil + "/" + FuncaoSite.ROUTE_URL_VIDEOS;
                string urlDepoimentos = urlPerfil + "/" + FuncaoSite.ROUTE_URL_DEPOIMENTOS;
                string urlComunidades = urlPerfil + "/" + FuncaoSite.ROUTE_URL_COMUNIDADES;

                #region HTML

                v_html = @"
                        <ul class='mn-vert'>
                            <li class='menu-li'><div class='icon-edit'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlEdit + @"'>" + UIConfig.EditarPerfil + @"</a></li>
                            <li class='menu-li'><div class='icon-membropend'></div><a item='amigo' cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlAmigosPend + @"'>" + UIConfig.AmigosPendentes + @"" + v_notif_ami + @"</a></li>
                            <li class='menu-li'><div class='icon-inicio'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlInicio + @"'>" + UIConfig.Inicio + @"</a></li><hr />
                            <li class='menu-li'><div class='icon-perfil'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlPerfil + @"'>" + UIConfig.Perfil + @"</a></li>                
                            <li class='menu-li'><div class='icon-people'></div><a item='amigo' cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlAmigos + @"'>" + UIConfig.Amigos + @"</a></li>
                            <li class='menu-li'><div class='icon-recado'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlRecados + @"'>" + UIConfig.Recados + @"" + v_recados_naolidos + @"</a></li>
                            <li class='menu-li'><div class='icon-video'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlVideos + @"'>" + UIConfig.Videos + @"</a></li>
                            <li class='menu-li'><div class='icon-depo'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlDepoimentos + @"'>" + UIConfig.Depoimentos + @"</a></li>
                            <li class='menu-li'><div class='icon-cmm'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlComunidades + @"'>" + UIConfig.Comunidades + @"</a></li>
                        </ul>
                    ";

                #endregion
            }
            else /// Menu do perfil que estou visualizando
            {
                var amizade = _amizadeService.Obter(PerfilLogged.UsuarioId, PerfilView.UsuarioId);

                string urlPerfil = FuncaoSite.getUrlNomeIdPerfil(PerfilView.Alias);
                string urlAmigos = urlPerfil + "/" + FuncaoSite.ROUTE_URL_AMIGOS;
                string urlRecados = urlPerfil + "/" + FuncaoSite.ROUTE_URL_RECADOS;
                string urlVideos = urlPerfil + "/" + FuncaoSite.ROUTE_URL_VIDEOS;
                string urlDepoimentos = urlPerfil + "/" + FuncaoSite.ROUTE_URL_DEPOIMENTOS;
                string urlComunidades = urlPerfil + "/" + FuncaoSite.ROUTE_URL_COMUNIDADES;

                #region HTML

                v_html = @"<ul class='mn-vert'>";

                if (amizade == null || amizade.StatusSolicitacao == SolicitacaoAmizade.Rejeitado)
                    v_html += @"<li class='menu-li'><div class='icon-addamigo'></div><a cp='" + PerfilView.UsuarioId.ToString() + "' class='btnViewPerfil' load='add' href='javascript:void(0);'>" + UIConfig.AdicionarAmigo + @"</a></li>";

                else if (amizade.SolicitadoPorId == PerfilLogged.UsuarioId && amizade.StatusSolicitacao == SolicitacaoAmizade.Pendente)
                    v_html += @"<li class='menu-li'><div class='icon-aguradamigo'></div><a cp='" + PerfilView.UsuarioId.ToString() + "' href='javascript:void(0);'>" + UIConfig.Esperando + @" " + PerfilView.Nome + " " + UIConfig.aceitar + @"</a></li>";

                else if (amizade.SolicitadoParaId == PerfilLogged.UsuarioId && amizade.StatusSolicitacao == SolicitacaoAmizade.Pendente)
                    v_html += @"<li class='menu-li'><div class='icon-aguradamigo'></div><a cp='" + PerfilView.UsuarioId.ToString() + "' href='javascript:void(0);'>" + UIConfig.Esperando + @" " + PerfilLogged.Nome + " " + UIConfig.aceitar + @"</a></li>";

                else
                    v_html += @"<li class='menu-li'><div class='icon-delamigo'></div><a cp='" + PerfilView.UsuarioId.ToString() + "' cpl='" + PerfilLogged + "' class='btnViewPerfil' load='del' href='javascript:void(0);'>" + UIConfig.RemoverAmigo + @"</a></li>";

                v_html += "<hr />";

                v_html += @"
                        <li class='menu-li'><div class='icon-perfil'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlPerfil + @"'>" + UIConfig.Perfil + @"</a></li>
                        <li class='menu-li'><div class='icon-people'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlAmigos + @"'>" + UIConfig.Amigos + @"</a></li>
                        <li class='menu-li'><div class='icon-recado'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlRecados + @"'>" + UIConfig.Recados + @"</a></li>
                        <li class='menu-li'><div class='icon-video'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlVideos + @"'>" + UIConfig.Videos + @"</a></li>
                        <li class='menu-li'><div class='icon-depo'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlDepoimentos + @"'>" + UIConfig.Depoimentos + @"</a></li>
                        <li class='menu-li'><div class='icon-cmm'></div><a cp='" + PerfilView.UsuarioId.ToString() + @"' href='" + urlComunidades + @"'>" + UIConfig.Comunidades + @"</a></li>
                        </ul>
                    ";

                #endregion
            }

            return v_html.ToString();
        }
        
        public string GetTemplateVisitantesHtml(Perfil PerfilLogged, Perfil PerfilView)
        {
            string v_html = string.Empty;

            if (PerfilLogged.UsuarioId != PerfilView.UsuarioId)
                return v_html;

            var lista = _visitantePerfilService.ObterVisitantes(5);

            if (lista.Count() > 0)
            {
                v_html += @"
                                <h2 class='ms-cnt-blc-lnk-tit'><a href='javascript:void(0);'>" + UIConfig.Visitantesdoperfil + @"</a></h2>
                                <div class='gl-cnt-zoom'>
                            ";

                foreach (var objeto in lista)
                {
                    Perfil perfil = objeto;

                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfil.Alias);
                    urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfil.UsuarioId.ToString() + "/1.jpg", false);

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

        public string GetTemplateAmigosHtml(Perfil PerfilLogged, Perfil PerfilView)
        {
            string v_html = string.Empty;

            var validacao = new PerfilValidacao(PerfilLogged.UsuarioId, PerfilView.UsuarioId);

            if (validacao.IsMeuPerfil() || validacao.IsAmigo())
            {
                var amigos = _amizadeService.ObterAmigos(PerfilView.UsuarioId, 1, FuncaoSite.TOTAL_ITEM_BLOCO, out int totalRecords).ToList();

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(PerfilView.Alias) + "/amigos";

                #region HTML

                v_html = @"
                    <h2 class='ms-cnt-blc-lnk-tit'><a href='" + urlEncryptadaPerfil + "'>" + UIConfig.Amigos + " (" + totalRecords.ToString() + @")</a></h2>
                    <div class='gl-cnt-zoom'>
                ";

                #endregion

                foreach (var amigo in amigos)
                {
                    urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(amigo.Perfil.Alias);
                    urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + amigo.UsuarioId.ToString() + "/1.jpg", false);

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

            return v_html;
        }

        public string GetTemplateCmmHtml(Perfil PerfilLogged, Perfil PerfilView)
        {
            var v_html = string.Empty;

            var validacao = new PerfilValidacao(PerfilLogged.UsuarioId, PerfilView.UsuarioId);

            if (validacao.IsMeuPerfil() || validacao.IsAmigo())
            {
                int totalRecords;
                var membros = _membroService.ObterComunidades(PerfilView.UsuarioId, 1, FuncaoSite.TOTAL_ITEM_BLOCO, out totalRecords).ToList();

                urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(PerfilView.Alias) + "/comunidades";

                #region HTML

                v_html = @"
                    <h2 class='ms-cnt-blc-lnk-tit'><a href='" + urlEncryptadaPerfil + "'>"+ UIConfig.Comunidades + @" (" + totalRecords.ToString() + @")</a></h2>
                    <div class='gl-cnt-zoom'>
                ";

                #endregion

                foreach (var membro in membros)
                {
                    var comunidade = membro.Comunidade;

                    urlEncryptadaCmm = FuncaoSite.getUrlNomeIdCmm(comunidade.Alias);
                    urlResolve = Funcao.ResolveServerUrl("~/Images/cmm/" + comunidade.ComunidadeId.ToString() + "/1.jpg", false);

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

            return v_html;
        }

        public void Dispose()
        {
            _amizadeService.Dispose();
            _membroService.Dispose();
            _visitantePerfilService.Dispose();
            _recadoService.Dispose();
        }

        public string GetTemplateMenuVerticalComunidadeHtml(Comunidade ComunidadeView, Perfil PerfilLogged)
        {
            var v_html = string.Empty;

            if (ComunidadeView == null)
                return v_html;

            urlEncryptadaCmm = FuncaoSite.EncryptUrl("c=" + ComunidadeView.ComunidadeId.ToString());

            var validacao = new CmmValidacao(PerfilLogged, ComunidadeView);

            int totalMembrosPendente;
            var membrosPendentes = _membroService.ObterMembrosPendentes(ComunidadeView.ComunidadeId, 1, 0, out totalMembrosPendente);

            if (ComunidadeView.IsDono(PerfilLogged.UsuarioId))
            {
                string urlEdit = FuncaoSite.getUrlNomeIdCmm(ComunidadeView.Alias) + "/" + FuncaoSite.ROUTE_URL_EDITCMM;
                string urlPerfil = FuncaoSite.getUrlNomeIdCmm(ComunidadeView.Alias);
                string urlCmp = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_MEMBROSPENDENTE;
                string urlForum = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_FORUM;
                string urlMembros = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_MEMBROS;
                string urlModeradores = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_MODERADORES;

                string v_notif_mem = (totalMembrosPendente > 0) ? " <span>" + totalMembrosPendente.ToString() + "</span>" : "";

                #region HTML

                var cmmId = ComunidadeView.ComunidadeId;

                v_html = @"
                    <ul class='mn-vert'>
                        <li class='menu-li'><div class='icon-cmm'></div><a cc='" + cmmId + "' href='" + urlEdit + @"'>" + UIConfig.EditarComunidade + @"</a></li>
                        <li class='menu-li'><div class='icon-membropend'></div><a item='membro' cc='" + cmmId + "' href='" + urlCmp + "'>" + UIConfig.MembrosPendentes + @" " + v_notif_mem + @"</a></li><hr />
                        <li class='menu-li'><div class='icon-cmm'></div><a cc='" + cmmId + "' href='" + urlPerfil + @"'>" + UIConfig.Comunidade + @"</a></li>
                        <li class='menu-li'><div class='icon-inicio'></div><a cc='" + cmmId + "' href='" + urlForum + @"'>" + UIConfig.Forum + @"</a></li>
                        <li class='menu-li'><div class='icon-people'></div><a cc='" + cmmId + "' href='" + urlMembros + @"'>" + UIConfig.Membros + @"</a></li>
                        <li class='menu-li'><div class='icon-people'></div><a cc='" + cmmId + "' href='" + urlModeradores + @"'>" + UIConfig.Moderadores + @"</a></li>
                    </ul>
                ";

                //<li class='menu-li'><img src='img/mv-recado.png' alt='mv-recado' /><a cc='" + p_codcmm_view + @"' href='javascript:void(0);'>Notificações</a></li>
                //<li class='menu-li'><img src='img/mv-lix.png'    alt='mv-lix'  /><a cc='" + p_codcmm_view + @"' href='#'>Lixeira</a></li>

                #endregion
            }
            else
            {
                var membro = _membroService.Obter(ComunidadeView.ComunidadeId, PerfilLogged.UsuarioId);

                var urlEdit = Funcao.ResolveServerUrl("~/" + FuncaoSite.ROUTE_URL_CADCMM, false);
                var urlPerfil = FuncaoSite.getUrlNomeIdCmm(ComunidadeView.Alias);
                var urlCmp = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_MEMBROSPENDENTE;
                var urlForum = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_FORUM;
                var urlMembros = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_MEMBROS;
                var urlModeradores = urlPerfil + @"/" + FuncaoSite.ROUTE_URL_MODERADORES;

                #region HTML

                var cmmId = ComunidadeView.ComunidadeId;

                v_html = "<ul class='mn-vert'>";

                if (membro == null || membro.StatusSolicitacao == StatusSolicitacaoMembroComunidade.Rejeitado)
                    v_html += "<li class='menu-li'><div class='icon-cmm'></div><a cc='" + cmmId + "' class='btnViewCmm' load='add' href='javascript:void(0);'>" + UIConfig.EntrarNaComunidade + @"</a></li>";

                else if (membro.StatusSolicitacao == StatusSolicitacaoMembroComunidade.Pendente)
                    v_html += "<li class='menu-li'><div class='icon-cmm'></div><a cc='" + cmmId + "' href='javascript:void(0);'>" + UIConfig.AguardandoPedidoDeEntrada + @"</a></li>";

                else
                    v_html += "<li class='menu-li'><div class='icon-cmm'></div><a cc='" + cmmId + "' class='btnViewCmm' load='del' href='javascript:void(0);'>" + UIConfig.SairDaComunidade + @"</a></li>";

                if (validacao.IsModerador)
                {
                    string v_notif_mem = (totalMembrosPendente > 0) ? " <span>" + totalMembrosPendente.ToString() + "</span>" : "";

                    v_html += "<li class='menu-li'><div class='icon-aguradamigo'></div><a item='membro' cc='" + cmmId + "' href='" + urlCmp + "'>" + UIConfig.MembrosPendentes + @" " + v_notif_mem + "</a></li>";
                }

                v_html += @"
                    <hr />
                    <li class='menu-li'><div class='icon-cmm'></div><a cc='" + cmmId + "' href='" + urlPerfil + @"'>" + UIConfig.Comunidade + @"</a></li>
                ";

                if (validacao.IsModerador || validacao.IsMembro || validacao.IsPublica)
                {
                    v_html += @"
                        <li class='menu-li'><div class='icon-inicio'></div><a cc='" + cmmId + "' href='" + urlForum + @"'>" + UIConfig.Forum + @"</a></li>
                        <li class='menu-li'><div class='icon-people'></div><a cc='" + cmmId + "' href='" + urlMembros + @"'>" + UIConfig.Membros + @"</a></li>
                        <li class='menu-li'><div class='icon-people'></div><a cc='" + cmmId + "' href='" + urlModeradores + @"'>" + UIConfig.Moderadores + @"</a></li>
                    ";

                    //v_html.Append("<li class='menu-li'><img src='img/mv-recado.png' alt='mv-recado' /><a cc='" + p_codcmm_view + "' href='javascript:void(0);'>Notificações</a></li>");
                    //v_html.Append("<li class='menu-li'><img src='img/mv-lix.png'    alt='mv-lix'  /><a cc='" + p_codcmm_view + "' href='#'>Lixeira</a></li>");
                }

                v_html += "</ul>";

                #endregion
            }

            return v_html.ToString();
        }

        public string GetTemplateMembrosHtml(Comunidade ComunidadeView, Perfil PerfilLogged)
        {
            var validacao = new CmmValidacao(PerfilLogged, ComunidadeView);

            string v_html = string.Empty;

            if (validacao.IsDono || validacao.IsModerador || validacao.IsMembro || validacao.IsPublica)
            {
                int totalRecords;
                var membros = _membroService.ObterMembros(ComunidadeView.ComunidadeId, 1, FuncaoSite.TOTAL_ITEM_BLOCO, out totalRecords).ToList();

                #region HTML

                v_html = @"
                        <h2 class='ms-cnt-blc-lnk-tit'><a href='" + urlEncryptadaCmm + @"'>" + UIConfig.Membros + @" (" + totalRecords.ToString() + @")</a></h2>
                        <div class='gl-cnt-zoom'>
                    ";

                #endregion

                if (membros.Count > 0)
                {
                    urlEncryptadaCmm = FuncaoSite.getUrlNomeIdCmm(ComunidadeView.Alias) + "/" + FuncaoSite.ROUTE_URL_MEMBROS;

                    foreach (var membro in membros)
                    {
                        var perfilMembro = membro.UsuarioMembro.Perfil;

                        urlEncryptadaPerfil = FuncaoSite.getUrlNomeIdPerfil(perfilMembro.Alias);
                        urlResolve = Funcao.ResolveServerUrl("~/Images/perfil/" + perfilMembro.UsuarioId.ToString() + "/1.jpg", false);

                        #region HTML

                        v_html += @"
                            <div class='gl-zoom tooltip2' codperfil='" + perfilMembro.UsuarioId.ToString() + @"' title='" + perfilMembro.Nome + " " + perfilMembro.Sobrenome + @"'>
                             <a href='" + urlEncryptadaPerfil + "' cp='" + perfilMembro.UsuarioId.ToString() + @"'>
                                 <span class='gl-zoom-caption'>
                                     <h3>" + perfilMembro.Nome + " " + perfilMembro.Sobrenome + @"</h3>
                                 </span>
                                 <img class='gl-zoom-img' src='" + urlResolve + @"' />
                             </a>
                            </div>
                        ";

                        #endregion
                    }
                }

                #region HTML

                v_html += @"</div>";

                #endregion
            }

            return v_html.ToString();
        }
    }
}