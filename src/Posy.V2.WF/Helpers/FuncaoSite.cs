using Posy.V2.Domain.Entities;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Seguranca;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Posy.V2.WF.Helpers
{
    public class FuncaoSite
    {
        public enum NomePagina
        {
            AMIGO,
            AMIGOPENDENTE,
            RECADO,
            VIDEO,
            DEPOIMENTO,
            COMUNIDADES,
            TOPICO,
            FORUM,
            MEMBROS,
            MEMBROSPENDENTE,
            MODERADORES
        }

        #region VARIAVEIS GLOBAIS

        public static int TOTAL_POST_PAGE = 10;
        public static int TOTAL_TOPICO_PAGE = 50;
        public static int TOTAL_COMENT_PAGE = 4;
        //public static int TOTAL_COMENT_PAGE_ALL = 0;
        public static int TOTAL_ITEM_BLOCO = 6; // Mudar também no amigos.js
        public static int TOTAL_CMM_PESQUISA_PAGE = 10;

        public static string PREFIX_URL_P = "p/";
        public static string PREFIX_URL_C = "c/";

        //public static string ROUTE_URL_LOGIN = "login";
        public static string ROUTE_URL_LOGOUT = "logout";
        public static string ROUTE_URL_DEFAULT = "default";
        //public static string ROUTE_URL_CADERFIL = "cadperfil";
        public static string ROUTE_URL_ERRO = "error";
        public static string ROUTE_URL_CAPTCHA = "captcha";

        //public static string ROUTE_FILE_LOGIN = "login.aspx";
        public static string ROUTE_FILE_LOGOUT = "logout.aspx";
        public static string ROUTE_FILE_DEFAULT = "default.aspx";
        //public static string ROUTE_FILE_CADERFIL = "cadperfil.aspx";
        public static string ROUTE_FILE_ERRO = "error.aspx";
        public static string ROUTE_FILE_CAPTCHA = "captcha.aspx";

        // PERFIL
        public static string ROUTE_URL_INICIO = "inicio";
        public static string ROUTE_URL_EDITARPERFIL = "editperfil";
        public static string ROUTE_URL_PERFIL = "perfil";
        public static string ROUTE_URL_AMIGOS = "amigos";
        public static string ROUTE_URL_AMIGOSPEND = "amigosp";
        public static string ROUTE_URL_RECADOS = "recados";
        public static string ROUTE_URL_VIDEOS = "videos";
        public static string ROUTE_URL_DEPOIMENTOS = "depoimentos";
        public static string ROUTE_URL_COMUNIDADES = "comunidades";

        public static string ROUTE_FILE_INICIO = PREFIX_URL_P + "inicio.aspx";
        public static string ROUTE_FILE_EDITARPERFIL = PREFIX_URL_P + "editperfil.aspx";
        public static string ROUTE_FILE_PERFIL = PREFIX_URL_P + "perfil.aspx";
        public static string ROUTE_FILE_AMIGOS = PREFIX_URL_P + "amigos.aspx";
        public static string ROUTE_FILE_AMIGOSPEND = PREFIX_URL_P + "amigospend.aspx";
        public static string ROUTE_FILE_RECADOS = PREFIX_URL_P + "recados.aspx";
        public static string ROUTE_FILE_VIDEOS = PREFIX_URL_P + "videos.aspx";
        public static string ROUTE_FILE_DEPOIMENTOS = PREFIX_URL_P + "depoimentos.aspx";
        public static string ROUTE_FILE_COMUNIDADES = PREFIX_URL_P + "comunidades.aspx";

        // CMM
        public static string ROUTE_URL_PREFIXCMM = "cmm";

        public static string ROUTE_URL_CMM = "cmm";
        public static string ROUTE_URL_PESQUISARCMM = "pesqcmm";
        public static string ROUTE_URL_FORUM = "forum";
        public static string ROUTE_URL_TOPICO = "topico";
        public static string ROUTE_URL_EDITCMM = "editcmm";
        public static string ROUTE_URL_CADCMM = "cadcmm";
        public static string ROUTE_URL_MEMBROSPENDENTE = "membrosp";
        public static string ROUTE_URL_MEMBROS = "membros";
        public static string ROUTE_URL_MODERADORES = "moderadores";

        public static string ROUTE_FILE_CMM = PREFIX_URL_C + "cmm.aspx";
        public static string ROUTE_FILE_PESQUISARCMM = PREFIX_URL_C + "pesqcmm.aspx";
        public static string ROUTE_FILE_FORUM = PREFIX_URL_C + "forum.aspx";
        public static string ROUTE_FILE_TOPICO = PREFIX_URL_C + "topico.aspx";
        public static string ROUTE_FILE_EDITCMM = PREFIX_URL_C + "editcmm.aspx";
        public static string ROUTE_FILE_CADCMM = PREFIX_URL_C + "cadcmm.aspx";
        public static string ROUTE_FILE_MEMBROSPENDENTE = PREFIX_URL_C + "membrospend.aspx";
        public static string ROUTE_FILE_MEMBROS = PREFIX_URL_C + "membros.aspx";
        public static string ROUTE_FILE_MODERADORES = PREFIX_URL_C + "moderadores.aspx";

        #endregion

        #region VERIFICACAO URL NOME ID

        public static string getUrlNomeIdPerfil(string alias, Guid p_codperfil)
        {
            return Funcao.ResolveServerUrl("~/" + alias, false);
        }

        public static string getUrlNomeIdCmm(string alias)
        {
            return Funcao.ResolveServerUrl("~/" + FuncaoSite.ROUTE_URL_PREFIXCMM + "/" + alias, false);
        }

        //public static string EncryptUrl(string p_url)
        //{
        //    return "?" + QueryStringModule.PARAMETER_NAME + Criptografia.Encrypt(p_url);
        //}

        public static string EncryptUrl(string p_url)
        {
            return Criptografia.Encrypt(p_url);
        }

        public static bool validaUrlNomeId(string texto)
        {
            return Regex.IsMatch(texto, @"^[a-zA-Z0-9_-]+$");
        }

        #endregion

        #region PAGINACAO

        /// <summary>
        /// Retorna a paginacao montada em html (PERFIL)
        /// </summary>
        /// <param name="p_paginaatual"></param>
        /// <param name="p_totalregistros"></param>
        /// <param name="p_codperfil"></param>
        /// <param name="nomepagina"></param>
        /// <returns></returns>
        public static string getPaginacaoPerfil(int paginaAtual, int totalregistros, Perfil perfil, NomePagina nomepagina)
        {
            int totalPaginas = (totalregistros / TOTAL_POST_PAGE),
                restoDiv = (totalregistros % TOTAL_POST_PAGE);

            totalPaginas = (restoDiv > 0) ? (totalPaginas + 1) : totalPaginas;

            if (totalPaginas == 0)
                return "";

            int[] arrIniFimPage = SetInicioFimPagina(paginaAtual, totalPaginas);

            string nomePagina = string.Empty;
            string urlEncryptada = string.Empty;

            switch (nomepagina)
            {
                case NomePagina.AMIGO:
                    nomePagina = ROUTE_URL_AMIGOS;
                    break;
                case NomePagina.AMIGOPENDENTE:
                    nomePagina = ROUTE_URL_AMIGOSPEND;
                    break;
                case NomePagina.RECADO:
                    nomePagina = ROUTE_URL_RECADOS;
                    break;
                case NomePagina.VIDEO:
                    nomePagina = ROUTE_URL_VIDEOS;
                    break;
                case NomePagina.DEPOIMENTO:
                    nomePagina = ROUTE_URL_DEPOIMENTOS;
                    break;
                case NomePagina.COMUNIDADES:
                    nomePagina = ROUTE_URL_COMUNIDADES;
                    break;
            }

            //PerfilDao perfilDao = new PerfilDao();
            //Perfil perfil = perfilDao.getObjeto(p_codperfil);

            string V_URL_PERFIL = getUrlNomeIdPerfil(perfil.Alias, perfil.UsuarioId) + "/" + nomePagina;

            var v_html_pager = new StringBuilder();
            v_html_pager.Append("<div class='paginate-content'>");
            v_html_pager.Append("<ul class='paginate'>");
            v_html_pager.Append("<li class='single-info'>" + UIConfig.Pagina + " " + paginaAtual.ToString() + " " + UIConfig.de + " " + totalPaginas.ToString() + "</li>");

            if (paginaAtual == 1)
                v_html_pager.Append("<li class='single'>" + UIConfig.primeiro + "</li>");
            else
            {
                urlEncryptada = V_URL_PERFIL + "/1";

                v_html_pager.Append("<li><a href='" + urlEncryptada + "'>" + UIConfig.primeiro + "</a></li>");
            }

            if (paginaAtual > 1)
            {
                urlEncryptada = V_URL_PERFIL + "/" + (paginaAtual - 1).ToString();

                v_html_pager.Append("<li><a href='" + urlEncryptada + "'>" + UIConfig.anterior + "</a></li>");
            }
            else
                v_html_pager.Append("<li class='single'>" + UIConfig.anterior + "</li>");

            for (int numpage = arrIniFimPage[0]; numpage <= arrIniFimPage[1]; numpage++)
            {
                if (numpage == paginaAtual)
                    v_html_pager.Append("<li class='current'>" + paginaAtual.ToString() + "</li>");
                else
                {
                    urlEncryptada = V_URL_PERFIL + "/" + numpage.ToString();

                    v_html_pager.Append("<li><a href='" + urlEncryptada + "'>" + numpage.ToString() + "</a></li>");
                }
            }

            if (paginaAtual < totalPaginas)
            {
                urlEncryptada = V_URL_PERFIL + "/" + (paginaAtual + 1).ToString();

                v_html_pager.Append("<li><a href='" + urlEncryptada + "'>" + UIConfig.proximo + "</a></li>");
            }
            else
                v_html_pager.Append("<li class='single'>" + UIConfig.proximo + "</li>");

            if (paginaAtual == totalPaginas)
                v_html_pager.Append("<li class='single'>" + UIConfig.ultimo + "</li>");
            else
            {
                urlEncryptada = V_URL_PERFIL + "/" + totalPaginas.ToString();

                v_html_pager.Append("<li><a href='" + urlEncryptada + "'>" + UIConfig.ultimo + "</a></li>");
            }

            v_html_pager.Append("</ul>");
            v_html_pager.Append("<div class='clear'></div>");
            v_html_pager.Append("</div>");

            return v_html_pager.ToString();
        }

        /// <summary>
        /// Retorna a paginacao montada em html (CMM)
        /// </summary>
        /// <param name="p_paginaatual"></param>
        /// <param name="p_totalregistros"></param>
        /// <param name="p_codcmm"></param>
        /// <param name="topicoId"></param>
        /// <param name="nomepagina"></param>
        /// <returns></returns>
        public static string getPaginacaoCmm(int paginaAtual, int totalregistros, Comunidade comunidade, Guid? topicoId, NomePagina nomepagina)
        {
            int totalPaginas, restoDiv;

            if (nomepagina == NomePagina.FORUM)
            {
                totalPaginas = (totalregistros / TOTAL_POST_PAGE);
                restoDiv = (totalregistros % TOTAL_POST_PAGE);
            }
            else
            {
                totalPaginas = (totalregistros / TOTAL_POST_PAGE);
                restoDiv = (totalregistros % TOTAL_POST_PAGE);
            }

            totalPaginas = (restoDiv > 0) ? (totalPaginas + 1) : totalPaginas;

            if (totalPaginas == 0)
                return "";

            int[] arrIniFimPage = SetInicioFimPagina(paginaAtual, totalPaginas);

            string nomePagina = string.Empty;
            string urlEncryptada = string.Empty;

            switch (nomepagina)
            {
                case NomePagina.TOPICO:
                    nomePagina = ROUTE_URL_TOPICO;
                    break;
                case NomePagina.FORUM:
                    nomePagina = ROUTE_URL_FORUM;
                    break;
                case NomePagina.MEMBROS:
                    nomePagina = ROUTE_URL_MEMBROS;
                    break;
                case NomePagina.MEMBROSPENDENTE:
                    nomePagina = ROUTE_URL_MEMBROSPENDENTE;
                    break;
                case NomePagina.MODERADORES:
                    nomePagina = ROUTE_URL_MODERADORES;
                    break;
            }

            string V_URL_PERFIL = getUrlNomeIdCmm(comunidade.Alias) + "/" + nomePagina;

            StringBuilder v_html_pager = new StringBuilder();
            v_html_pager.Append("<div class='paginate-content'>");
            v_html_pager.Append("<ul class='paginate'>");
            v_html_pager.Append("<li class='single-info'>Página " + paginaAtual.ToString() + " de " + totalPaginas.ToString() + "</li>");

            if (paginaAtual == 1)
                v_html_pager.Append("<li class='single'>primeiro</li>");
            else
            {
                urlEncryptada = (topicoId != null) ? V_URL_PERFIL + "/" + topicoId + "/1" : V_URL_PERFIL + "/1";

                v_html_pager.Append("<li><a href='" + urlEncryptada + "'>primeiro</a></li>");
            }

            if (paginaAtual > 1)
            {
                urlEncryptada = (topicoId != null) ? V_URL_PERFIL + "/" + topicoId + "/" + (paginaAtual - 1).ToString() : V_URL_PERFIL + "/" + (paginaAtual - 1).ToString();

                v_html_pager.Append("<li><a href='" + urlEncryptada + "'>anterior</a></li>");
            }
            else
                v_html_pager.Append("<li class='single'>anterior</li>");

            for (int numpage = arrIniFimPage[0]; numpage <= arrIniFimPage[1]; numpage++)
            {
                if (numpage == paginaAtual)
                    v_html_pager.Append("<li class='current'>" + paginaAtual.ToString() + "</li>");
                else
                {
                    urlEncryptada = (topicoId != null) ? V_URL_PERFIL + "/" + topicoId + "/" + numpage.ToString() : V_URL_PERFIL + "/" + numpage.ToString();

                    v_html_pager.Append("<li><a href='" + urlEncryptada + "'>" + numpage.ToString() + "</a></li>");
                }
            }

            if (paginaAtual < totalPaginas)
            {
                urlEncryptada = (topicoId != null) ? V_URL_PERFIL + "/" + topicoId + "/" + (paginaAtual + 1).ToString() : V_URL_PERFIL + "/" + (paginaAtual + 1).ToString();

                v_html_pager.Append("<li><a href='" + urlEncryptada + "'>próximo</a></li>");
            }
            else
                v_html_pager.Append("<li class='single'>próximo</li>");

            if (paginaAtual == totalPaginas)
                v_html_pager.Append("<li class='single'>último</li>");
            else
            {
                urlEncryptada = (topicoId != null) ? V_URL_PERFIL + "/" + topicoId + "/" + totalPaginas.ToString() : V_URL_PERFIL + "/" + totalPaginas.ToString();

                v_html_pager.Append("<li><a href='" + urlEncryptada + "'>último</a></li>");
            }

            v_html_pager.Append("</ul>");
            v_html_pager.Append("<div class='clear'></div>");
            v_html_pager.Append("</div>");

            return v_html_pager.ToString();
        }

        /// <summary>
        /// Retorna numeração de inicio e fim pagina para paginação
        /// </summary>
        /// <param name="PaginaAtual"></param>
        /// <param name="TotalPaginas"></param>
        /// <returns></returns>
        public static int[] SetInicioFimPagina(int PaginaAtual, int TotalPaginas)
        {
            int i = 0;
            int af = 0;
            int f = 0;
            int ai = 0;

            if (PaginaAtual - 4 < 1)
            {
                i = 1;
                af = Math.Abs(PaginaAtual - 4) + 1;
            }
            else
                i = PaginaAtual - 4;

            if (PaginaAtual + 4 >= TotalPaginas)
            {
                f = TotalPaginas;
                ai = Math.Abs(TotalPaginas - PaginaAtual - 4);
            }
            else
                f = PaginaAtual + 4;

            if (af + f >= TotalPaginas)
                f = TotalPaginas;
            else
                f = f + af;

            //this.Final = f;

            if (i - ai <= 0)
                i = 1;
            else
                i = i - ai;

            //this.Inicio = i;

            int[] arrRetorno = new int[2];
            arrRetorno[0] = i;
            arrRetorno[1] = f;

            return arrRetorno;
        }

        #endregion

        #region OUTRAS FUNÇÕES

        public static string getTempoPost(DateTime dtPost)
        {
            DateTime dtAtual = DateTime.Now; // new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            string tempoPost = string.Empty;

            //DateTime dtPostTemp = new DateTime(dtPost.Year, dtPost.Month, dtPost.Day, dtPost.Hour, dtPost.Minute, dtPost.Second);
            DateTime dtPostTemp = dtPost.ToLocalTime();  //DateTime.SpecifyKind(dtPost, DateTimeKind.Utc).ToLocalTime(); 
            TimeSpan diffResult = dtAtual.Subtract(dtPostTemp);

            if (diffResult.Days > 0)
                tempoPost = (diffResult.Days > 1) ? diffResult.Days.ToString() + " " + UIConfig.diasatras : diffResult.Days.ToString() + " " + UIConfig.diaatras;

            else if (diffResult.Hours > 0)
                tempoPost = (diffResult.Hours > 1) ? diffResult.Hours.ToString() + " " + UIConfig.horasatras : diffResult.Hours.ToString() + " " + UIConfig.horaatras;

            else if (diffResult.Minutes > 0)
                tempoPost = (diffResult.Minutes > 1) ? diffResult.Minutes.ToString() + " " + UIConfig.minutosatras : diffResult.Minutes.ToString() + " " + UIConfig.minutoatras;

            else if (diffResult.Seconds > 0)
                tempoPost = (diffResult.Seconds > 1) ? diffResult.Seconds.ToString() + " " + UIConfig.segundosatras : diffResult.Seconds.ToString() + " " + UIConfig.segundoatras;

            else
                tempoPost = UIConfig.Agora;

            return tempoPost;
        }

        #endregion
    }
}