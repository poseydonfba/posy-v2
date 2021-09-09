using Posy.V2.Domain.Entities;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.MVC.Controllers;
using Posy.V2.MVC.Helpers;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;

namespace Posy.V2.MVC
{
    public class uploadFotoCadCmm : IHttpHandler
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
                    var _comunidadeView = BaseControllerComunidade.ComunidadeView;

                    string arquivoTemp = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + postedFile.FileName;
                    string dirDestino = context.Server.MapPath("~") + "\\Images\\cmm\\" + _comunidadeView.ComunidadeId.ToString();

                    if (!Directory.Exists(dirDestino))
                        Directory.CreateDirectory(dirDestino);

                    postedFile.SaveAs(dirDestino + "\\" + arquivoTemp);

                    retorno = "/Images/cmm/" + _comunidadeView.ComunidadeId.ToString() + "/" + arquivoTemp;
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