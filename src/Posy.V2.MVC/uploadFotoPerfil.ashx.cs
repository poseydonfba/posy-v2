using Microsoft.AspNet.Identity;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.MVC.Helpers;
using System;
using System.IO;
using System.Web;

namespace Posy.V2.MVC
{
    public class uploadFotoPerfil : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            try
            {
                HttpPostedFile postedFile = context.Request.Files["file"];

                var user = HttpContext.Current.User.Identity;

                string retorno;

                if (user.IsAuthenticated)
                {
                    string arquivoTemp = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + postedFile.FileName;
                    string dirDestino = context.Server.MapPath("~") + "\\Images\\perfil\\" + user.GetUserId();

                    if (!Directory.Exists(dirDestino))
                        Directory.CreateDirectory(dirDestino);

                    postedFile.SaveAs(dirDestino + "\\" + arquivoTemp);

                    retorno = "/Images/perfil/" + user.GetUserId() + "/" + arquivoTemp;
                }
                else
                {
                    retorno = "-1";
                }

                context.Response.Write(retorno);
            }
            catch (Exception ex)
            {
                //context.Response.Write("Error: " + ex.Message);
                context.Response.Redirect(Funcao.ResolveServerUrl("~/" + FuncaoSite.ROUTE_URL_ERRO + "?", false));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}