using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Posy.V2.Infra.CrossCutting.Common
{
    public class Funcao
    {

        #region RESOLVE URL

        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            serverUrl = serverUrl.Replace("~", string.Empty);

            if (serverUrl.EndsWith(".jpg") ||
                serverUrl.EndsWith(".png") ||
                serverUrl.EndsWith(".ico") ||
                serverUrl.EndsWith(".svg"))
            {
                //return Fingerprint.Tag(serverUrl.Substring(2));
                return StaticFile.Version(serverUrl);
            }else
                return serverUrl;
            //else if (serverUrl.EndsWith(".css") ||
            //         serverUrl.EndsWith(".js"))
            //{
            //    //return Fingerprint.Tag(serverUrl.Substring(2));
            //    return StaticFile.Version(serverUrl);
            //}
            //else
            //{
            //    // *** Is it already an absolute Url?
            //    if (serverUrl.IndexOf("://") > -1)
            //        return serverUrl;

            //    // *** Start by fixing up the Url an Application relative Url
            //    //string newUrl = ResolveUrl(serverUrl);
            //    string newUrl = serverUrl.Replace("~/", ConfigurationManager.AppSettings["siteRootUrl"]);

            //    //Uri originalUri = HttpContext.Current.Request.Url;
            //    //newUrl = (forceHttps ? "https" : originalUri.Scheme) + "://" + originalUri.Authority + newUrl;

            //    Uri originalUri = new Uri(newUrl);
            //    newUrl = (forceHttps ? newUrl.Replace(originalUri.Scheme, "https") : newUrl);

            //    return newUrl;
            //}
        }

        #endregion

        #region OUTRAS FUNCOES

        public static bool IsNumeric(string p_numero)
        {
            float output;
            return float.TryParse(p_numero, out output);
        }

        public static bool IsGuid(string p_guid)
        {
            Guid output;
            return Guid.TryParse(p_guid, out output);
        }

        #endregion

        #region VALIDACOES

        /// <summary>
        /// Verificação de numeros e textos em uma string
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static bool contemLetras(string texto)
        {
            return Regex.IsMatch(texto, @"^[a-zA-Z]+$");
        }

        public static bool contemLetrasENumeros(string texto)
        {
            return Regex.IsMatch(texto, @"^[a-zA-Z0-9]+$");
        }

        public static bool contemLetrasENumerosEUnderline(string texto)
        {
            return Regex.IsMatch(texto, @"^[a-zA-Z0-9_]+$");
        }

        public static bool contemNumeros(string texto)
        {
            return Regex.IsMatch(texto, @"^[0-9]+$");
        }

        #endregion

        public static string RemoveWhiteSpaceFromStylesheets(string body)
        {
            body = Regex.Replace(body, @"[a-zA-Z]+#", "#");
            body = Regex.Replace(body, @"[\n\r]+\s*", string.Empty);
            body = Regex.Replace(body, @"\s+", " ");
            body = Regex.Replace(body, @"\s?([:,;{}])\s?", "$1");
            body = body.Replace(";}", "}");
            body = Regex.Replace(body, @"([\s:]0)(px|pt|%|em)", "$1");
            body = Regex.Replace(body, @"/\*[\d\D]*?\*/", string.Empty); // Remove comments from CSS

            return body;
        }
    }
}
