using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Identity.Configuration;
using Posy.V2.Infra.CrossCutting.Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Posy.V2.Api.Controllers
{
    [RoutePrefix("api/v1/account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";

        private readonly ApplicationUserManager _userManager;
        private readonly ISecureDataFormat<AuthenticationTicket> _accessTokenFormat;
        private readonly IPerfilService _perfilService; 

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat,
            IPerfilService perfilService)
        {
            _userManager = userManager;
            _accessTokenFormat = accessTokenFormat;
            _perfilService = perfilService;
        }

        private Guid GetIdentityUserId()
        {
            return Guid.Parse(User.Identity.GetUserId());
        }

        // GET api/v1/account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public async Task<IHttpActionResult> GetUserInfo()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            var user = _userManager.FindByName(User.Identity.Name);
            //var perfil = _perfilService.Obter(user.Id);

            var vm = new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin?.LoginProvider,

                Id = user.Id,
                Name = user.UserName,
                //UserPhoto = perfil.,
                Dir = user.Dir
            };

            return Ok(vm);
        }

        // POST api/v1/Account/Logout
        //[Route("Logout")]
        //public IHttpActionResult Logout()
        //{
        //    Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);


        //    return Ok();
        //}

        // GET api/v1/Account/ManageInfo?returnUrl=%2F&generateState=true
        //[Route("ManageInfo")]
        //public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        //{
        //    var user = await _userManager.FindByIdAsync(GetIdentityUserId());

        //    if (user == null)
        //    {
        //        return null;
        //    }

        //    var logins = new List<UserLoginInfoViewModel>();

        //    foreach (var linkedAccount in user.Logins)
        //    {
        //        logins.Add(new UserLoginInfoViewModel
        //        {
        //            LoginProvider = linkedAccount.LoginProvider,
        //            ProviderKey = linkedAccount.ProviderKey
        //        });
        //    }

        //    if (user.PasswordHash != null)
        //    {
        //        logins.Add(new UserLoginInfoViewModel
        //        {
        //            LoginProvider = LocalLoginProvider,
        //            ProviderKey = user.UserName,
        //        });
        //    }

        //    return new ManageInfoViewModel
        //    {
        //        LocalLoginProvider = LocalLoginProvider,
        //        Email = user.UserName,
        //        Logins = logins,
        //        ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
        //    };
        //}

        // POST api/v1/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(GetIdentityUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/v1/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.AddPasswordAsync(GetIdentityUserId(), model.NewPassword);

            return !result.Succeeded ? GetErrorResult(result) : Ok();
        }

        // POST api/v1/Account/AddExternalLogin
        //[Route("AddExternalLogin")]
        //public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

        //    var ticket = _accessTokenFormat.Unprotect(model.ExternalAccessToken);

        //    if (ticket == null || ticket.Identity == null || (ticket.Properties != null
        //        && ticket.Properties.ExpiresUtc.HasValue
        //        && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
        //    {
        //        return BadRequest("External login failure.");
        //    }

        //    var externalData = ExternalLoginData.FromIdentity(ticket.Identity);

        //    if (externalData == null)
        //    {
        //        return BadRequest("The external login is already associated with an account.");
        //    }

        //    var result = await _userManager.AddLoginAsync(GetIdentityUserId(),
        //        new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

        //    return !result.Succeeded ? GetErrorResult(result) : Ok();
        //}

        // POST api/v1/Account/RemoveLogin
        //[Route("RemoveLogin")]
        //public async Task<IHttpActionResult> RemoveLogin(RemoveLoginViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    IdentityResult result;

        //    if (model.LoginProvider == LocalLoginProvider)
        //    {
        //        result = await _userManager.RemovePasswordAsync(GetIdentityUserId());
        //    }
        //    else
        //    {
        //        result = await _userManager.RemoveLoginAsync(GetIdentityUserId(),
        //            new UserLoginInfo(model.LoginProvider, model.ProviderKey));
        //    }

        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    return Ok();
        //}

        // GET api/v1/Account/ExternalLogin
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        //[AllowAnonymous]
        //[Route("ExternalLogin", Name = "ExternalLogin")]
        //public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        //{
        //    if (error != null)
        //    {
        //        return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
        //    }

        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return new ChallengeResult(provider, this);
        //    }

        //    var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

        //    if (externalLogin == null)
        //    {
        //        return InternalServerError();
        //    }

        //    if (externalLogin.LoginProvider != provider)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //        return new ChallengeResult(provider, this);
        //    }

        //    var user = await _userManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
        //        externalLogin.ProviderKey));

        //    var hasRegistered = user != null;

        //    if (hasRegistered)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

        //        var oAuthIdentity = await user.GenerateUserIdentityAsync(_userManager, OAuthDefaults.AuthenticationType);
        //        var cookieIdentity = await user.GenerateUserIdentityAsync(_userManager, CookieAuthenticationDefaults.AuthenticationType);

        //        var properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
        //        Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
        //    }
        //    else
        //    {
        //        var claims = externalLogin.GetClaims();
        //        var identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
        //        Authentication.SignIn(identity);
        //    }

        //    return Ok();
        //}

        // GET api/v1/Account/ExternalLogins?returnUrl=%2F&generateState=true
        //[AllowAnonymous]
        //[Route("ExternalLogins")]
        //public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        //{
        //    var descriptions = Authentication.GetExternalAuthenticationTypes();
        //    var logins = new List<ExternalLoginViewModel>();

        //    string state;

        //    if (generateState)
        //    {
        //        const int strengthInBits = 256;
        //        state = RandomOAuthStateGenerator.Generate(strengthInBits);
        //    }
        //    else
        //    {
        //        state = null;
        //    }

        //    foreach (var description in descriptions)
        //    {
        //        var login = new ExternalLoginViewModel
        //        {
        //            Name = description.Caption,
        //            Url = Url.Route("ExternalLogin", new
        //            {
        //                provider = description.AuthenticationType,
        //                response_type = "token",
        //                client_id = Startup.PublicClientId,
        //                redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
        //                state = state
        //            }),
        //            State = state
        //        };
        //        logins.Add(login);
        //    }

        //    return logins;
        //}

        // POST api/v1/Account/Register
        //[AllowAnonymous]
        //[Route("Register")]
        //public async Task<IHttpActionResult> Register(RegisterViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = new ApplicationUser() { UserName = model.Email, Email = model.Email, Name = model.Name, UserPhoto = model.UserPhoto };

        //    var result = await _userManager.CreateAsync(user, model.Password);

        //    return !result.Succeeded ? GetErrorResult(result) : Ok();
        //}

        // POST api/v1/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(ExternalLoginConfirmationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await _userManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (result.Succeeded) return null;

            if (result.Errors != null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            if (ModelState.IsValid)
            {
                // No ModelState errors are available to send, so just return an empty BadRequest.
                return BadRequest();
            }

            return BadRequest(ModelState);
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                var providerKeyClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(providerKeyClaim?.Issuer) || string.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        #endregion
    }
    //[Authorize]
    //[RoutePrefix("api/Account")]
    //public class AccountController : ApiController
    //{
    //    private ApplicationSignInManager _signInManager;
    //    private ApplicationUserManager _userManager;

    //    public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
    //    {
    //        _userManager = userManager;
    //        _signInManager = signInManager;
    //    }


    //    #region OLD MVC

    //    ////
    //    //// GET: /Account/Login
    //    //[AllowAnonymous]
    //    //public IHttpActionResult Login(string returnUrl)
    //    //{
    //    //    ViewBag.ReturnUrl = returnUrl;
    //    //    return View();
    //    //}

    //    ////
    //    //// POST: /Account/Login
    //    //[HttpPost]
    //    //[AllowAnonymous]
    //    ////[ValidateAntiForgeryToken]
    //    //public async Task<IHttpActionResult> Login(LoginViewModel model, string returnUrl)
    //    //{
    //    //    if (!ModelState.IsValid)
    //    //    {
    //    //        return View(model);
    //    //    }

    //    //    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
    //    //    switch (result)
    //    //    {
    //    //        case SignInStatus.Success:
    //    //            var user = await _userManager.FindAsync(model.Email, model.Password);
    //    //            if (!user.EmailConfirmed)
    //    //            {
    //    //                TempData["AvisoEmail"] = "Usuário não confirmado, verifique seu e-mail.";
    //    //            }
    //    //            await SignInAsync(user, model.RememberMe);
    //    //            return RedirectToLocal(returnUrl);
    //    //        case SignInStatus.LockedOut:
    //    //            return View("Lockout");
    //    //        case SignInStatus.RequiresVerification:
    //    //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
    //    //        case SignInStatus.Failure:
    //    //        default:
    //    //            ModelState.AddModelError("", "Login ou Senha incorretos.");
    //    //            return View(model);
    //    //    }
    //    //}
    //    //private async Task SignInAsync(ApplicationUser user, bool isPersistent)
    //    //{
    //    //    var clientKey = Request.Browser.Type;
    //    //    await _userManager.SignInClientAsync(user, clientKey);

    //    //    // Zerando contador de logins errados.
    //    //    await _userManager.ResetAccessFailedCountAsync(user.Id);

    //    //    // Coletando Claims externos (se houver)
    //    //    ClaimsIdentity ext = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

    //    //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie, DefaultAuthenticationTypes.ApplicationCookie);
    //    //    AuthenticationManager.SignIn
    //    //        (
    //    //            new AuthenticationProperties { IsPersistent = isPersistent },
    //    //            // Criação da instancia do Identity e atribuição dos Claims
    //    //            await user.GenerateUserIdentityAsync(_userManager, ext)
    //    //        );
    //    //}

    //    ////
    //    //// GET: /Account/VerifyCode
    //    //[AllowAnonymous]
    //    //public async Task<IHttpActionResult> VerifyCode(string provider, string returnUrl, Guid userId)
    //    //{
    //    //    // Requer que o usuario já tenha feito um login por senha.
    //    //    if (!await _signInManager.HasBeenVerifiedAsync())
    //    //    {
    //    //        return View("Error");
    //    //    }
    //    //    var user = await _userManager.FindByIdAsync(await _signInManager.GetVerifiedUserIdAsync());
    //    //    if (user != null)
    //    //    {
    //    //        ViewBag.Status = "DEMO: Caso o código não chegue via " + provider + " o código é: ";
    //    //        ViewBag.CodigoAcesso = await _userManager.GenerateTwoFactorTokenAsync(user.Id, provider);
    //    //    }
    //    //    return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, UserId = userId });
    //    //}

    //    ////
    //    //// POST: /Account/VerifyCode
    //    //[HttpPost]
    //    //[AllowAnonymous]
    //    ////[ValidateAntiForgeryToken]
    //    //public async Task<IHttpActionResult> VerifyCode(VerifyCodeViewModel model)
    //    //{
    //    //    if (!ModelState.IsValid)
    //    //    {
    //    //        return View(model);
    //    //    }

    //    //    var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
    //    //    switch (result)
    //    //    {
    //    //        case SignInStatus.Success:
    //    //            var user = _userManager.FindByIdAsync(model.UserId);
    //    //            await SignInAsync(user.Result, false);
    //    //            return RedirectToLocal(model.ReturnUrl);
    //    //        case SignInStatus.LockedOut:
    //    //            return View("Lockout");
    //    //        case SignInStatus.Failure:
    //    //        default:
    //    //            ModelState.AddModelError("", "Código Inválido.");
    //    //            return View(model);
    //    //    }
    //    //}

    //    //////
    //    ////// GET: /Account/Register
    //    ////[AllowAnonymous]
    //    ////public IHttpActionResult Register()
    //    ////{
    //    ////    return View();
    //    ////}

    //    // POST api/Account/Register
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [Route("Register")]
    //    public async Task<IHttpActionResult> Register(RegisterViewModel model)
    //    {
    //        if (!ModelState.IsValid)
    //            return BadRequest(ModelState);

    //        var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

    //        IdentityResult result = await _userManager.CreateAsync(user, model.Password);

    //        if (!result.Succeeded)
    //            return GetErrorResult(result);

    //        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
    //        await _userManager.SendEmailAsync(user.Id, "Confirme sua Conta", $"Código de confirmação: {code}");

    //        return Ok();
    //    }

    //    ////
    //    //// POST: /Account/Register
    //    //[HttpPost]
    //    //[AllowAnonymous]
    //    ////[ValidateAntiForgeryToken]
    //    //public async Task<IHttpActionResult> Register(RegisterViewModel model)
    //    //{
    //    //    if (ModelState.IsValid)
    //    //    {
    //    //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
    //    //        var result = await _userManager.CreateAsync(user, model.Password);
    //    //        if (result.Succeeded)
    //    //        {
    //    //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
    //    //            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
    //    //            await _userManager.SendEmailAsync(user.Id, "Confirme sua Conta", "Por favor confirme sua conta clicando neste link: <a href='" + callbackUrl + "'></a>");
    //    //            ViewBag.Link = callbackUrl;
    //    //            return View("DisplayEmail");
    //    //        }
    //    //        AddErrors(result);
    //    //    }

    //    //    // No caso de falha, reexibir a view. 
    //    //    return View(model);
    //    //}

    //    ////
    //    //// GET: /Account/ConfirmEmail
    //    //[AllowAnonymous]
    //    //public async Task<IHttpActionResult> ConfirmEmail(Guid userId, string code)
    //    //{
    //    //    if (userId == default(Guid) || code == null)
    //    //    {
    //    //        return View("Error");
    //    //    }
    //    //    var result = await _userManager.ConfirmEmailAsync(userId, code);
    //    //    return View(result.Succeeded ? "ConfirmEmail" : "Error");
    //    //}

    //    ////
    //    //// GET: /Account/ForgotPassword
    //    //[AllowAnonymous]
    //    //public IHttpActionResult ForgotPassword()
    //    //{
    //    //    return View();
    //    //}

    //    ////
    //    //// POST: /Account/ForgotPassword
    //    //[HttpPost]
    //    //[AllowAnonymous]
    //    ////[ValidateAntiForgeryToken]
    //    //public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordViewModel model)
    //    //{
    //    //    if (ModelState.IsValid)
    //    //    {
    //    //        var user = await _userManager.FindByNameAsync(model.Email);
    //    //        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
    //    //        {
    //    //            // Não revelar se o usuario nao existe ou nao esta confirmado
    //    //            return View("ForgotPasswordConfirmation");
    //    //        }

    //    //        var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
    //    //        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
    //    //        await _userManager.SendEmailAsync(user.Id, "Esqueci minha senha", "Por favor altere sua senha clicando aqui: <a href='" + callbackUrl + "'></a>");
    //    //        ViewBag.Link = callbackUrl;
    //    //        ViewBag.Status = "DEMO: Caso o link não chegue: ";
    //    //        ViewBag.LinkAcesso = callbackUrl;
    //    //        return View("ForgotPasswordConfirmation");
    //    //    }

    //    //    // No caso de falha, reexibir a view. 
    //    //    return View(model);
    //    //}

    //    ////
    //    //// GET: /Account/ForgotPasswordConfirmation
    //    //[AllowAnonymous]
    //    //public IHttpActionResult ForgotPasswordConfirmation()
    //    //{
    //    //    return View();
    //    //}

    //    ////
    //    //// GET: /Account/ResetPassword
    //    //[AllowAnonymous]
    //    //public IHttpActionResult ResetPassword(string code)
    //    //{
    //    //    return code == null ? View("Error") : View();
    //    //}

    //    ////
    //    //// POST: /Account/ResetPassword
    //    //[HttpPost]
    //    //[AllowAnonymous]
    //    ////[ValidateAntiForgeryToken]
    //    //public async Task<IHttpActionResult> ResetPassword(ResetPasswordViewModel model)
    //    //{
    //    //    if (!ModelState.IsValid)
    //    //    {
    //    //        return View(model);
    //    //    }
    //    //    var user = await _userManager.FindByNameAsync(model.Email);
    //    //    if (user == null)
    //    //    {
    //    //        // Não revelar se o usuario nao existe ou nao esta confirmado
    //    //        return RedirectToAction("ResetPasswordConfirmation", "Account");
    //    //    }
    //    //    var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
    //    //    if (result.Succeeded)
    //    //    {
    //    //        return RedirectToAction("ResetPasswordConfirmation", "Account");
    //    //    }
    //    //    AddErrors(result);
    //    //    return View();
    //    //}

    //    ////
    //    //// GET: /Account/ResetPasswordConfirmation
    //    //[AllowAnonymous]
    //    //public IHttpActionResult ResetPasswordConfirmation()
    //    //{
    //    //    return View();
    //    //}

    //    ////
    //    //// POST: /Account/ExternalLogin
    //    //[HttpPost]
    //    //[AllowAnonymous]
    //    ////[ValidateAntiForgeryToken]
    //    //public IHttpActionResult ExternalLogin(string provider, string returnUrl)
    //    //{
    //    //    // Request a redirect to the external login provider
    //    //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
    //    //}

    //    ////
    //    //// GET: /Account/SendCode
    //    //[AllowAnonymous]
    //    //public async Task<IHttpActionResult> SendCode(string returnUrl)
    //    //{
    //    //    var userId = await _signInManager.GetVerifiedUserIdAsync();
    //    //    if (userId == default(Guid))
    //    //    {
    //    //        return View("Error");
    //    //    }
    //    //    var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(userId);
    //    //    var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
    //    //    return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, UserId = userId });
    //    //}

    //    ////
    //    //// POST: /Account/SendCode
    //    //[HttpPost]
    //    //[AllowAnonymous]
    //    ////[ValidateAntiForgeryToken]
    //    //public async Task<IHttpActionResult> SendCode(SendCodeViewModel model)
    //    //{
    //    //    if (!ModelState.IsValid)
    //    //    {
    //    //        return View();
    //    //    }

    //    //    // Gerar o token e enviar
    //    //    if (!await _signInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
    //    //    {
    //    //        return View("Error");
    //    //    }
    //    //    return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, userId = model.UserId });
    //    //}

    //    ////
    //    //// GET: /Account/ExternalLoginCallback
    //    //[AllowAnonymous]
    //    //public async Task<IHttpActionResult> ExternalLoginCallback(string returnUrl)
    //    //{
    //    //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
    //    //    if (loginInfo == null)
    //    //    {
    //    //        return RedirectToAction("Login");
    //    //    }

    //    //    var user = await _userManager.FindAsync(loginInfo.Login);

    //    //    // Logar caso haja um login externo e já esteja logado neste provedor de login
    //    //    var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

    //    //    switch (result)
    //    //    {
    //    //        case SignInStatus.Success:
    //    //            var userext = _userManager.FindByEmailAsync(user.Email);
    //    //            await SignInAsync(userext.Result, false);
    //    //            return RedirectToLocal(returnUrl);
    //    //        case SignInStatus.LockedOut:
    //    //            return View("Lockout");
    //    //        case SignInStatus.RequiresVerification:
    //    //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
    //    //        case SignInStatus.Failure:
    //    //        default:
    //    //            // Se ele nao tem uma conta solicite que crie uma

    //    //            if (loginInfo.Login.LoginProvider.ToLower() == "facebook")
    //    //            {
    //    //                var externalIdentity = HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
    //    //                var email = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:email").Value;
    //    //                var firstName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:first_name").Value;
    //    //                var lastName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:last_name").Value;

    //    //                ViewBag.ReturnUrl = returnUrl;
    //    //                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
    //    //                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email, LastName = lastName, Name = firstName });
    //    //            }
    //    //            else if (loginInfo.Login.LoginProvider.ToLower() == "google")
    //    //            {
    //    //                var externalIdentity = HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
    //    //                var email = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
    //    //                var firstName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value;
    //    //                var lastName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;

    //    //                ViewBag.ReturnUrl = returnUrl;
    //    //                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
    //    //                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email, LastName = lastName, Name = firstName });
    //    //            }
    //    //            else if (loginInfo.Login.LoginProvider.ToLower() == "twitter")
    //    //            {
    //    //                var externalIdentity = HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
    //    //                var access_token = externalIdentity.Result.Claims.Where(x => x.Type == "urn:twitter:access_token").Select(x => x.Value).FirstOrDefault();
    //    //                var access_secret = externalIdentity.Result.Claims.Where(x => x.Type == "urn:twitter:access_secret").Select(x => x.Value).FirstOrDefault();

    //    //                var response = await TwitterHelper.TwitterLogin(access_token, access_secret, TwitterConfiguration.ConsumerKey, TwitterConfiguration.ConsumerSecret);

    //    //                var email = response.email;
    //    //                var firstName = response.name;
    //    //                var lastName = string.Empty;

    //    //                ViewBag.ReturnUrl = returnUrl;
    //    //                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
    //    //                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email, LastName = lastName, Name = firstName });
    //    //            }
    //    //            else
    //    //            {
    //    //                return View("Error");
    //    //            }
    //    //    }
    //    //}

    //    ////
    //    //// POST: /Account/ExternalLoginConfirmation
    //    //[HttpPost]
    //    //[AllowAnonymous]
    //    ////[ValidateAntiForgeryToken]
    //    //public async Task<IHttpActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
    //    //{
    //    //    if (User.Identity.IsAuthenticated)
    //    //    {
    //    //        return RedirectToAction("Index", "Manage");
    //    //    }

    //    //    if (ModelState.IsValid)
    //    //    {
    //    //        // Pegar a informação do login externo.
    //    //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
    //    //        if (info == null)
    //    //        {
    //    //            return View("ExternalLoginFailure");
    //    //        }
    //    //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
    //    //        var result = await _userManager.CreateAsync(user);
    //    //        if (result.Succeeded)
    //    //        {
    //    //            result = await _userManager.AddLoginAsync(user.Id, info.Login);
    //    //            if (result.Succeeded)
    //    //            {
    //    //                await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
    //    //                var userext = _userManager.FindByEmailAsync(model.Email);
    //    //                await SignInAsync(userext.Result, false);
    //    //                return RedirectToLocal(returnUrl);
    //    //            }
    //    //        }
    //    //        AddErrors(result);
    //    //    }

    //    //    ViewBag.ReturnUrl = returnUrl;
    //    //    return View(model);
    //    //}

    //    ////
    //    //// POST: /Account/LogOff
    //    //[HttpPost]
    //    ////[ValidateAntiForgeryToken]
    //    //public async Task<IHttpActionResult> LogOff()
    //    //{
    //    //    await SignOutAsync();
    //    //    //AuthenticationManager.SignOut();
    //    //    return RedirectToAction("Index", "Home");
    //    //}

    //    ////
    //    //// GET: /Account/ExternalLoginFailure
    //    //[AllowAnonymous]
    //    //public IHttpActionResult ExternalLoginFailure()
    //    //{
    //    //    return View();
    //    //}

    //    //private async Task SignOutAsync()
    //    //{
    //    //    var clientKey = Request.Browser.Type;
    //    //    var user = _userManager.FindById(Guid.Parse(User.Identity.GetUserId()));
    //    //    await _userManager.SignOutClientAsync(user, clientKey);
    //    //    AuthenticationManager.SignOut();
    //    //}

    //    //[HttpPost]
    //    ////[ValidateAntiForgeryToken]
    //    //public async Task<IHttpActionResult> SignOutEverywhere()
    //    //{
    //    //    _userManager.UpdateSecurityStamp(Guid.Parse(User.Identity.GetUserId()));
    //    //    await SignOutAsync();
    //    //    return RedirectToAction("Index", "Home");
    //    //}

    //    //[HttpPost]
    //    ////[ValidateAntiForgeryToken]
    //    //public IHttpActionResult SignOutClient(int clientId)
    //    //{
    //    //    var user = _userManager.FindById(Guid.Parse(User.Identity.GetUserId()));
    //    //    var client = user.UsuarioClientes.SingleOrDefault(c => c.UsuarioClienteId == clientId);
    //    //    if (client != null)
    //    //    {
    //    //        user.UsuarioClientes.Remove(client);
    //    //    }
    //    //    _userManager.Update(user);
    //    //    return RedirectToAction("Index", "Home");
    //    //}

    //    #endregion

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            if (_userManager != null)
    //            {
    //                _userManager.Dispose();
    //                _userManager = null;
    //            }

    //            if (_signInManager != null)
    //            {
    //                _signInManager.Dispose();
    //                _signInManager = null;
    //            }
    //        }

    //        base.Dispose(disposing);
    //    }

    //    #region Helpers

    //    private IAuthenticationManager Authentication
    //    {
    //        get { return Request.GetOwinContext().Authentication; }
    //    }

    //    private IHttpActionResult GetErrorResult(IdentityResult result)
    //    {
    //        if (result == null)
    //        {
    //            return InternalServerError();
    //        }

    //        if (!result.Succeeded)
    //        {
    //            if (result.Errors != null)
    //            {
    //                foreach (string error in result.Errors)
    //                {
    //                    ModelState.AddModelError("", error);
    //                }
    //            }

    //            if (ModelState.IsValid)
    //            {
    //                // No ModelState errors are available to send, so just return an empty BadRequest.
    //                return BadRequest();
    //            }

    //            return BadRequest(ModelState);
    //        }

    //        return null;
    //    }

    //    private class ExternalLoginData
    //    {
    //        public string LoginProvider { get; set; }
    //        public string ProviderKey { get; set; }
    //        public string UserName { get; set; }

    //        public IList<Claim> GetClaims()
    //        {
    //            IList<Claim> claims = new List<Claim>();
    //            claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

    //            if (UserName != null)
    //            {
    //                claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
    //            }

    //            return claims;
    //        }

    //        public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
    //        {
    //            if (identity == null)
    //            {
    //                return null;
    //            }

    //            Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

    //            if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
    //                || String.IsNullOrEmpty(providerKeyClaim.Value))
    //            {
    //                return null;
    //            }

    //            if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
    //            {
    //                return null;
    //            }

    //            return new ExternalLoginData
    //            {
    //                LoginProvider = providerKeyClaim.Issuer,
    //                ProviderKey = providerKeyClaim.Value,
    //                UserName = identity.FindFirstValue(ClaimTypes.Name)
    //            };
    //        }
    //    }

    //    private static class RandomOAuthStateGenerator
    //    {
    //        private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

    //        public static string Generate(int strengthInBits)
    //        {
    //            const int bitsPerByte = 8;

    //            if (strengthInBits % bitsPerByte != 0)
    //            {
    //                throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
    //            }

    //            int strengthInBytes = strengthInBits / bitsPerByte;

    //            byte[] data = new byte[strengthInBytes];
    //            _random.GetBytes(data);
    //            return HttpServerUtility.UrlTokenEncode(data);
    //        }
    //    }

    //    #endregion

    //    //#region Helpers
    //    //// Used for XSRF protection when adding external logins
    //    //private const string XsrfKey = "XsrfId";

    //    //private IAuthenticationManager AuthenticationManager
    //    //{
    //    //    get
    //    //    {
    //    //        return HttpContext.GetOwinContext().Authentication;
    //    //    }
    //    //}

    //    //private void AddErrors(IdentityResult result)
    //    //{
    //    //    foreach (var error in result.Errors)
    //    //    {
    //    //        ModelState.AddModelError("", error);
    //    //    }
    //    //}

    //    //private IHttpActionResult RedirectToLocal(string returnUrl)
    //    //{
    //    //    if (Url.IsLocalUrl(returnUrl))
    //    //    {
    //    //        return Redirect(returnUrl);
    //    //    }
    //    //    return RedirectToAction("Index", "Home");
    //    //}

    //    //internal class ChallengeResult : HttpUnauthorizedResult
    //    //{
    //    //    public ChallengeResult(string provider, string redirectUri)
    //    //        : this(provider, redirectUri, null)
    //    //    {
    //    //    }

    //    //    public ChallengeResult(string provider, string redirectUri, string userId)
    //    //    {
    //    //        LoginProvider = provider;
    //    //        RedirectUri = redirectUri;
    //    //        UserId = userId;
    //    //    }

    //    //    public string LoginProvider { get; set; }
    //    //    public string RedirectUri { get; set; }
    //    //    public string UserId { get; set; }

    //    //    public override void ExecuteResult(ControllerContext context)
    //    //    {
    //    //        //// this line fixed the problem with returing null
    //    //        //context.RequestContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;

    //    //        var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
    //    //        if (UserId != null)
    //    //        {
    //    //            properties.Dictionary[XsrfKey] = UserId;
    //    //        }
    //    //        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
    //    //    }
    //    //}
    //    //#endregion
    //}
}
