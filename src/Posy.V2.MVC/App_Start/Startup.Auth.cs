using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Twitter;
using Owin;
using Owin.Security.Providers.LinkedIn;
using Posy.V2.Infra.CrossCutting.Identity.Configuration;
using Posy.V2.MVC.App_Start;
using Posy.V2.MVC.Configuration;
using Posy.V2.MVC.Hub.Config;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Posy.V2.MVC
{
    public partial class Startup
    {
        private const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";

        /**
        * For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        **/
        public void ConfigureAuth(IAppBuilder app)
        {
            /**
            * Configure the db context, user manager and signin manager to use a single instance per request
            **/
            app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<ApplicationUserManager>());

            /**
            * Enable the application to use a cookie to store information for the signed in user
            * and to use a cookie to temporarily store information about a user logging in with a third party login provider
            * Configure the sign in cookie
            **/
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                /// ReturnUrlParameter = "xxx", Substitui o ReturnUrl
                Provider = new CookieAuthenticationProvider
                {
                    /**
                    * Enables the application to validate the security stamp when the user logs in.
                    * This is a security feature which is used when you change a password or add an external login to your account.
                    **/

                    /// OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, Guid>(
                    ///    validateInterval: TimeSpan.FromMinutes(30),
                    ///    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))

                    OnValidateIdentity = ApplicationCookieIdentityValidator.OnValidateIdentity(
                       validateInterval: TimeSpan.FromMinutes(0),
                       regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            /**
            * Enables the application to temporarily store user information when they are verifying the second factor 
            * in the two-factor authentication process.
            **/
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            /**
            * Enables the application to remember the second login verification factor such as phone or email.
            * Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            * This is similar to the RememberMe option when you log in.
            **/
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            /**
            * Uncomment the following lines to enable logging in with third party login providers
            **/

            ///app.UseMicrosoftAccountAuthentication(
            ///    clientId: "SEU ID",
            ///    clientSecret: "SEU TOKEN");

            /**
            * Configuração com o LinkedIn
            * http://developer.linkedin.com
            * https://www.jerriepelser.com/blog/introducing-the-yahoo-linkedin-oauth-security-providers-for-owin/#register-the-linkedin-provider
            **/
            #region LINKEDIN

            if (!string.IsNullOrEmpty(LinkedInConfiguration.ClientId))
            {
                var options = new LinkedInAuthenticationOptions
                {
                    ClientId = LinkedInConfiguration.ClientId,
                    ClientSecret = LinkedInConfiguration.ClientSecret
                };

                app.UseLinkedInAuthentication(options);
            }

            #endregion

            #region TWITTER

            if (!string.IsNullOrEmpty(TwitterConfiguration.ConsumerKey))
            {
                var tao = new TwitterAuthenticationOptions
                {
                    ConsumerKey = TwitterConfiguration.ConsumerKey,
                    ConsumerSecret = TwitterConfiguration.ConsumerSecret
                };

                tao.Provider = new TwitterAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new Claim("urn:twitter:access_token", context.AccessToken));
                        context.Identity.AddClaim(new Claim("urn:twitter:access_secret", context.AccessTokenSecret));
                        return Task.FromResult(0);
                    }
                };

                tao.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;
                app.UseTwitterAuthentication(tao);
            }

            #endregion

            /**
            * Configuração com o LinkedIn
            * https://console.developers.google.com
            * https://docs.microsoft.com/pt-br/aspnet/mvc/overview/security/create-an-aspnet-mvc-5-app-with-facebook-and-google-oauth2-and-openid-sign-on
            **/
            /// 
            #region GOOGLE

            if (!string.IsNullOrEmpty(GoogleConfiguration.ClientId))
            {
                var gao = new GoogleOAuth2AuthenticationOptions
                {
                    ClientId = GoogleConfiguration.ClientId,
                    ClientSecret = GoogleConfiguration.ClientSecret,
                };

                gao.Scope.Add("email");

                gao.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;
                app.UseGoogleAuthentication(gao);
            }

            #endregion

            #region FACEBOOK

            if (!string.IsNullOrEmpty(FacebookConfiguration.AppId))
            {
                var fao = new FacebookAuthenticationOptions
                {
                    AppId = FacebookConfiguration.AppId,
                    AppSecret = FacebookConfiguration.AppSecret
                };

                fao.Scope.Add("public_profile");
                fao.Scope.Add("email");
                //fao.Scope.Add("publish_actions");
                //fao.Scope.Add("basic_info");

                //add this for facebook to actually return the email and name
                fao.Fields.Add("email");
                fao.Fields.Add("name");
                fao.Fields.Add("first_name");
                fao.Fields.Add("last_name");

                fao.Provider = new FacebookAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new Claim("urn:facebook:access_token", context.AccessToken, XmlSchemaString, "Facebook"));
                        foreach (var x in context.User)
                        {
                            var claimType = string.Format("urn:facebook:{0}", x.Key);
                            string claimValue = x.Value.ToString();
                            if (!context.Identity.HasClaim(claimType, claimValue))
                                context.Identity.AddClaim(new Claim(claimType, claimValue, XmlSchemaString, "Facebook"));

                        }
                        return Task.FromResult(0);
                    }
                };

                fao.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;
                app.UseFacebookAuthentication(fao);
            }

            #endregion
        }

        public void ConfigureSignalR(IAppBuilder app)
        {
            var config = new HubConfiguration { };
            SimpleInjectorInitializer.container.EnableSignalR(config);
            app.MapSignalR<SimpleInjectorHubDispatcher>("/signalr", config);

            /// app.MapSignalR();
            GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}