using System;
using System.Net;
using System.Reflection;
using System.Web;

namespace Posy.V2.WF.Helpers
{
    public class ResponseService
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string ReasonPhrase { get; private set; }
        public dynamic Result { get; private set; }

        public ResponseService(HttpStatusCode statusCode, string reasonPhrase, dynamic result, HttpContext context, int secondsCache = 60)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Result = result;

            context.Response.StatusCode = (int)StatusCode;

            // 27/03/2017
            // SetResponseHeaders funciona bem, coloca o metodo do webservice em cache
            // mas ao mudar de perfil por exemplo mostra os dados antigos em cache e não do perfil visualizado
            //if (ChangeCache(context))
            //{
            //    // Tem que remover o cache do response
            //    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //    context.Response.Cache.SetExpires(DateTime.Now);
            //    context.Response.Cache.SetNoStore();
            //}
            //else if (secondsCache > -1)
            //    SetResponseHeaders(context, HttpCacheability.Public, new TimeSpan(0, 0, 0, secondsCache));

            context.ApplicationInstance.CompleteRequest();
        }

        public bool ChangeCache(HttpContext context)
        {
            return (bool)context.Application.Get("changeMethodCache");
        }

        public static void SetResponseHeaders(HttpContext context, HttpCacheability cacheability, TimeSpan delta)
        {
            ////Cache the reponse to the client for 60 seconds:
            //HttpContext context = HttpContext.Current;
            //TimeSpan cacheExpires = new TimeSpan(0, 0, 0, 60);
            //SetResponseHeaders(ref context, HttpCacheability.Public, cacheExpires);

            if ((context != null))
            {
                HttpCachePolicy cache = context.ApplicationInstance.Response.Cache;
                cache.SetCacheability(cacheability);

                switch (cacheability)
                {
                    case HttpCacheability.NoCache:
                        break;
                    default:
                        //set cache expiry:
                        var dateExpires = DateTime.UtcNow;
                        dateExpires = dateExpires.AddMinutes(delta.TotalMinutes);

                        //set expiry date:
                        cache.SetExpires(dateExpires);
                        FieldInfo maxAgeField = cache.GetType().GetField("_maxAge", BindingFlags.Instance | BindingFlags.NonPublic);

                        if ((maxAgeField != null))
                            maxAgeField.SetValue(cache, delta);

                        break;
                }
            }
        }
    }
}