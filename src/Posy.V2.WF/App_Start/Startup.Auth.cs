using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Twitter;
using Owin;
using Posy.V2.Infra.CrossCutting.Identity.Configuration;
using Posy.V2.Infra.CrossCutting.Identity.Context;
using Posy.V2.WF.Configuration;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Posy.V2.WF
{
    public partial class Startup
    {
        public string XmlSchemaString { get; private set; }

        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301883
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    //    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                    //        validateInterval: TimeSpan.FromMinutes(30),
                    //        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))

                    //OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, Guid>(
                    //        validateInterval: TimeSpan.FromMinutes(30),
                    //        regenerateIdentityCallback: (manager, user) 
                    //            => user.GenerateUserIdentityAsync(manager),
                    //            getUserIdCallback: (claim) => Guid.Parse(claim.GetUserId()))

                    OnValidateIdentity = ApplicationCookieIdentityValidator.OnValidateIdentity(
                       validateInterval: TimeSpan.FromMinutes(0),
                       regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
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
                //fao.Fields.Add("email");
                //fao.Fields.Add("name");
                //fao.Fields.Add("first_name");
                //fao.Fields.Add("last_name");

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
    }
}
