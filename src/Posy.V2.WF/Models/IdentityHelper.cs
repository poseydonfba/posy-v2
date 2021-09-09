using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Posy.V2.Infra.CrossCutting.Identity.Configuration;
using Posy.V2.Infra.CrossCutting.Identity.Model;
using Posy.V2.WF.Helpers;
using System;
using System.Security.Claims;
using System.Web;

namespace Posy.V2.WF
{
    public class IdentityHelper
    {
        //public static async Task SignInAsync(ApplicationUser user, bool isPersistent, HttpRequest request)
        public static void SignIn(ApplicationUser user, bool isPersistent, HttpRequest request)
        {
            var manager = request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var AuthenticationManager = request.GetOwinContext().Authentication;

            var clientKey = request.Browser.Type;
            //await manager.SignInClientAsync(user, clientKey);
            manager.SignInClient(user, clientKey);

            // Zerando contador de logins errados.
            manager.ResetAccessFailedCount(user.Id);

            // Coletando Claims externos (se houver)
            ClaimsIdentity ext = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn
                (
                    new AuthenticationProperties { IsPersistent = isPersistent },
                    // Criação da instancia do Identity e atribuição dos Claims
                    user.GenerateUserIdentity(manager, ext)
                );
        }

        public static void SignOut(HttpRequest request)
        {
            var manager = request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var AuthenticationManager = request.GetOwinContext().Authentication;

            var clientKey = request.Browser.Type;
            var user = manager.FindById(Guid.Parse(HttpContext.Current.User.Identity.GetUserId()));
            manager.SignOutClient(user, clientKey);
            AuthenticationManager.SignOut();
        }

        // Used for XSRF when linking external logins
        public const string XsrfKey = "XsrfId";

        public const string ProviderNameKey = "providerName";
        public static string GetProviderNameFromRequest(HttpRequest request)
        {
            return request.QueryString[ProviderNameKey];
        }

        public const string CodeKey = "code";
        public static string GetCodeFromRequest(HttpRequest request)
        {
            return request.QueryString[CodeKey];
        }

        public const string UserIdKey = "userId";
        public static string GetUserIdFromRequest(HttpRequest request)
        {
            return HttpUtility.UrlDecode(request.QueryString[UserIdKey]);
        }

        public static string GetResetPasswordRedirectUrl(string code, HttpRequest request)
        {
            var absoluteUri = "/Account/ResetPassword?" + CodeKey + "=" + HttpUtility.UrlEncode(code);
            return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
        }

        public static string GetUserConfirmationRedirectUrl(string code, string userId, HttpRequest request)
        {
            var absoluteUri = "/Account/Confirm?" + CodeKey + "=" + HttpUtility.UrlEncode(code) + "&" + UserIdKey + "=" + HttpUtility.UrlEncode(userId);
            return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
        }

        private static bool IsLocalUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }

        public static void RedirectToReturnUrl(string returnUrl, HttpResponse response)
        {
            if (!String.IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl))
            {
                response.Redirect(returnUrl);
            }
            else
            {
                response.Redirect("~/");
            }
        }
    }
}