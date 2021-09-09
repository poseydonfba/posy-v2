using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Identity.Configuration;
using Posy.V2.Infra.CrossCutting.Identity.Model;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Posy.V2.WF.Account
{
    public partial class TwoFactorAuthenticationSignIn : System.Web.UI.Page
    {
        [Import] public ApplicationSignInManager signinManager { get; set; }
        [Import] public ApplicationUserManager manager { get; set; }
        //[Import] public IPerfilService _perfilService { get; set; }
        //private ApplicationSignInManager signinManager;
        //private ApplicationUserManager manager;

        //public TwoFactorAuthenticationSignIn()
        //{
        //    manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            var userId = signinManager.GetVerifiedUserId<ApplicationUser, Guid>();
            if (userId == null)
            {
                Response.Redirect("/Account/Error", true);
            }
            var userFactors = manager.GetValidTwoFactorProviders(userId);
            Providers.DataSource = userFactors.Select(x => x).ToList();
            Providers.DataBind();            
        }

        protected void CodeSubmit_Click(object sender, EventArgs e)
        {
            bool rememberMe = false;
            bool.TryParse(Request.QueryString["RememberMe"], out rememberMe);
            
            var result = signinManager.TwoFactorSignIn<ApplicationUser, Guid>(SelectedProvider.Value, Code.Text, isPersistent: rememberMe, rememberBrowser: RememberBrowser.Checked);
            switch (result)
            {
                case SignInStatus.Success:
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    break;
                case SignInStatus.LockedOut:
                    Response.Redirect("/Account/Lockout");
                    break;
                case SignInStatus.Failure:
                default:
                    FailureText.Text = "Invalid code";
                    ErrorMessage.Visible = true;
                    break;
            }
        }

        protected void ProviderSubmit_Click(object sender, EventArgs e)
        {
            if (!signinManager.SendTwoFactorCode(Providers.SelectedValue))
            {
                Response.Redirect("/Account/Error");
            }

            var user = manager.FindById(signinManager.GetVerifiedUserId<ApplicationUser, Guid>());
            if (user != null)
            {
                var code = manager.GenerateTwoFactorToken(user.Id, Providers.SelectedValue);
            }

            SelectedProvider.Value = Providers.SelectedValue;
            sendcode.Visible = false;
            verifycode.Visible = true;
        }
    }
}