using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Identity.Configuration;
using Posy.V2.Infra.CrossCutting.Identity.Extensions;
using Posy.V2.Infra.CrossCutting.Identity.Model;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Posy.V2.MVC.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IPerfilService _perfilService;

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IPerfilService perfilService)
        //public ManageController(ApplicationUserManager userManager, IPerfilService perfilService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _perfilService = perfilService;
        }

        //
        // GET: /Account/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? UIConfigLogin.ASenhaFoiAlterada // "A senha foi alterada."
                : message == ManageMessageId.SetPasswordSuccess ? UIConfigLogin.ASenhaFoiEnviada // "A senha foi enviada."
                : message == ManageMessageId.SetTwoFactorSuccess ? UIConfigLogin.ASegundaValidacaoFoiEnviada // "A segunda validação foi enviada."
                : message == ManageMessageId.Error ? UIConfigLogin.OcorreuUmErro // "Ocorreu um erro."
                : message == ManageMessageId.AddPhoneSuccess ? UIConfigLogin.OTelefoneFoiAdicionado // "O Telefone foi adicionado."
                : message == ManageMessageId.RemovePhoneSuccess ? UIConfigLogin.OTelefoneFoiRemovido // "O Telefone foi removido."
                : "";

            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(Guid.Parse(User.Identity.GetUserId())),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(Guid.Parse(User.Identity.GetUserId())),
                Logins = await _userManager.GetLoginsAsync(Guid.Parse(User.Identity.GetUserId())),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId())
            };
            return View(model);
        }

        //
        // GET: /Account/RemoveLogin
        public ActionResult RemoveLogin()
        {
            var linkedAccounts = _userManager.GetLogins(Guid.Parse(User.Identity.GetUserId()));
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await _userManager.RemoveLoginAsync(Guid.Parse(User.Identity.GetUserId()), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Account/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Account/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Gerar um token e enviar
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(Guid.Parse(User.Identity.GetUserId()), model.Number);
            if (_userManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = UIConfigLogin.OCodigoDeSegurancaE + ": " + code
                };
                await _userManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/RememberBrowser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RememberBrowser()
        {
            var rememberBrowserIdentity = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(User.Identity.GetUserId());
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, rememberBrowserIdentity);
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/ForgetBrowser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetBrowser()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/EnableTFA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTFA()
        {
            await _userManager.SetTwoFactorEnabledAsync(Guid.Parse(User.Identity.GetUserId()), true);
            var user = await _userManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTFA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTFA()
        {
            await _userManager.SetTwoFactorEnabledAsync(Guid.Parse(User.Identity.GetUserId()), false);
            var user = await _userManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Account/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            // This code allows you exercise the flow without actually sending codes
            // For production use please register a SMS provider in IdentityConfig and generate a code here.
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(Guid.Parse(User.Identity.GetUserId()), phoneNumber);

            ViewBag.Status = "DEMO: Caso o código não chegue via SMS o código é: ";
            ViewBag.CodigoAcesso = code;

            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Account/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _userManager.ChangePhoneNumberAsync(Guid.Parse(User.Identity.GetUserId()), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // No caso de falha, reexibir a view. 
            ModelState.AddModelError("", UIConfigLogin.FalhaAoAdicionarTelefone);
            return View(model);
        }

        //
        // GET: /Account/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await _userManager.SetPhoneNumberAsync(Guid.Parse(User.Identity.GetUserId()), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await _userManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _userManager.ChangePasswordAsync(Guid.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.AddPasswordAsync(Guid.Parse(User.Identity.GetUserId()), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // No caso de falha, reexibir a view. 
            return View(model);
        }

        //
        // GET: /Account/Manage
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? UIConfigLogin.OLoginFoiRemovido + "."
                : message == ManageMessageId.Error ?UIConfigLogin.OcorreuUmErro +  "."
                : "";
            var user = await _userManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(Guid.Parse(User.Identity.GetUserId()));
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(Guid.Parse(User.Identity.GetUserId()), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        #region CONFIGURATION CULTURE INFO

        // https://www.jerriepelser.com/blog/allowing-user-to-set-culture-settings-aspnet5-part2/

        [HttpGet]
        public async Task<ActionResult> ConfigureCultureInfo()
        {
            var user = await _userManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));

            var viewModel = new ConfigureCultureInfoViewModel
            {
                Language = user.Language,
                ShortDateFormat = user.ShortDateFormat,
                LongDateFormat = user.LongDateFormat,
                CurrencySymbol = user.CurrencySymbol
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ConfigureCultureInfo(ConfigureCultureInfoViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
            user.Language = viewModel.Language;
            user.ShortDateFormat = viewModel.ShortDateFormat;
            user.LongDateFormat = viewModel.LongDateFormat;
            user.CurrencySymbol = viewModel.CurrencySymbol;

            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, true, false); // Force the CreateUserPrincipalAsync method on our CustomSignInManager to be called again

            return RedirectToAction("Index");
        }

        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            // Claims da model Perfil
            var perfil = _perfilService.Obter(user.Id); // POSEYDON
            var othersClaims = new OthersClaims // POSEYDON
            {
                Nome = perfil.Nome, // POSEYDON
                Sobrenome = perfil.Sobrenome, // POSEYDON
                Language = user.Language, // POSEYDON
                ShortDateFormat = user.ShortDateFormat, // POSEYDON
                LongDateFormat = user.LongDateFormat, // POSEYDON
                CurrencySymbol = user.CurrencySymbol // POSEYDON
            };

            var clientKey = Request.Browser.Type;
            await _userManager.SignInClientAsync(user, clientKey, othersClaims);

            // Zerando contador de logins errados.
            await _userManager.ResetAccessFailedCountAsync(user.Id);

            // Coletando Claims externos (se houver)
            ClaimsIdentity ext = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn
                (
                    new AuthenticationProperties { IsPersistent = isPersistent },
                    // Criação da instancia do Identity e atribuição dos Claims
                    await user.GenerateUserIdentityAsync(_userManager, ext)
                );
        }

        //private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        //{
        //    var clientKey = Request.Browser.Type;
        //    await _userManager.SignInClientAsync(user, clientKey);

        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
        //    AuthenticationManager.SignIn
        //        (
        //            new AuthenticationProperties { IsPersistent = isPersistent }, 
        //            await user.GenerateUserIdentityAsync(_userManager)
        //        );
        //}

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = _userManager.FindById(Guid.Parse(User.Identity.GetUserId()));
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = _userManager.FindById(Guid.Parse(User.Identity.GetUserId()));
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (!requestContext.HttpContext.User.Identity.IsAuthenticated)
                return;

            VerifyCultureInfo(requestContext);
        }

        protected void VerifyCultureInfo(RequestContext requestContext)
        {
            var user = requestContext.HttpContext.User.Identity as ClaimsIdentity;
            var culturePreferences = user.GetCulture();
            if (culturePreferences == null)
                return;

            var uiCulture = new CultureInfo(culturePreferences.Language);
            var culture = new CultureInfo(culturePreferences.Language);

            if (!string.IsNullOrEmpty(culturePreferences.ShortDateFormat))
                culture.DateTimeFormat.ShortDatePattern = culturePreferences.ShortDateFormat;
            if (!string.IsNullOrEmpty(culturePreferences.LongDateFormat))
                culture.DateTimeFormat.LongDatePattern = culturePreferences.LongDateFormat;
            if (!string.IsNullOrEmpty(culturePreferences.CurrencySymbol))
                culture.NumberFormat.CurrencySymbol = culturePreferences.CurrencySymbol;

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = uiCulture;
        }
    }
}