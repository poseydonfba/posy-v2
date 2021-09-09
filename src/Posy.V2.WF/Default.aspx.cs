using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.WF.Helpers;
using System;
using System.Web.UI;

namespace Posy.V2.WF
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect(Funcao.ResolveServerUrl("~/" + FuncaoSite.ROUTE_URL_INICIO, false), true);
            }
        }
    }
}