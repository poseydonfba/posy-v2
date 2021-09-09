using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Enums;
using Posy.V2.Infra.CrossCutting.Common.Extensions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Identity.Configuration;
using Posy.V2.Infra.CrossCutting.Identity.Model;
using Posy.V2.MVC.Configuration;
using Posy.V2.MVC.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Posy.V2.MVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private IUnitOfWork _uow;
        private IPerfilService _perfilService;
        private IPrivacidadeService _privacidadeService;

        public AccountController(ApplicationUserManager userManager,
                                 ApplicationSignInManager signInManager,
                                 IUnitOfWork uow,
                                 IPerfilService perfilService,
                                 IPrivacidadeService privacidadeService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _uow = uow;
            _perfilService = perfilService;
            _privacidadeService = privacidadeService;
        }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            CarregaListDrops();

            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = await _userManager.FindAsync(model.Email, model.Password);
                    if (!user.EmailConfirmed)
                    {
                        //TempData["AvisoEmail"] = "Usuário não confirmado, verifique seu e-mail.";

                        string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, UIConfigLogin.EmailSubjectResend);
                        ViewBag.Link = callbackUrl;
                        TempData["Link"] = callbackUrl;
                        return RedirectToAction("DisplayEmail"); //View("DisplayEmail");
                    }
                    else
                    {
                        await SignInAsync(user, model.RememberMe);
                        return RedirectToLocal(returnUrl);
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", UIConfigLogin.LoginOuSenhaIncorretos);
                    return View(model);
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

        //
        // GET: /Account/DisplayEmail
        [AllowAnonymous]
        public ActionResult DisplayEmail()
        {
            ViewBag.Link = ViewBag.Link;
            return View();
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, Guid userId)
        {
            // Requer que o usuario já tenha feito um login por senha.
            if (!await _signInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(await _signInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "DEMO: Caso o código não chegue via " + provider + " o código é: ";
                ViewBag.CodigoAcesso = await _userManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, UserId = userId });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = _userManager.FindByIdAsync(model.UserId);
                    await SignInAsync(user.Result, false);
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Código Inválido.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            // Initialization.  
            //LoginRegisterViewModel model = new LoginRegisterViewModel();
            //model.LoginVM = new LoginViewModel();
            //model.RegisterVM = new RegisterViewModel();

            CarregaListDrops();

            return View();
        }

        public void CarregaListDrops()
        {
            ListDia();
            ListMes();
            ListAno();
            ListSexo();
            ListEstadoCivil();
        }

        public void ListAno()
        {
            var lista = new List<Posy.V2.MVC.Models.HtmlSelect>();
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "", Text = "" });
            for (int i = 1900; i <= DateTime.Now.Year; i++)
                lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = i.ToString(), Text = i.ToString() });

            ViewBag.Ano = lista;
        }

        public void ListMes()
        {
            var lista = new List<Posy.V2.MVC.Models.HtmlSelect>();
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "", Text = "" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "1", Text = "Janeiro" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "2", Text = "Fevereiro" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "3", Text = "Março" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "4", Text = "Abril" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "5", Text = "Maio" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "6", Text = "Junho" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "7", Text = "Julho" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "8", Text = "Agosto" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "9", Text = "Setembro" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "10", Text = "Outubro" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "11", Text = "Novembro" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "12", Text = "Dezembro" });

            ViewBag.Mes = lista;
        }

        public void ListDia()
        {
            var lista = new List<Posy.V2.MVC.Models.HtmlSelect>();
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "", Text = "" });
            for (int i = 1; i < 31; i++)
                lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = i.ToString(), Text = i.ToString() });

            ViewBag.Dia = lista;
        }

        public void ListSexo()
        {
            var lista = new List<Posy.V2.MVC.Models.HtmlSelect>();
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "", Text = "" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = (int)Sexo.MASCULINO, Text = Sexo.MASCULINO.GetDescription() });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = (int)Sexo.FEMININO, Text = Sexo.FEMININO.GetDescription() });

            ViewBag.Sexo = lista;
        }

        public void ListEstadoCivil()
        {
            var lista = new List<Posy.V2.MVC.Models.HtmlSelect>();
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = "", Text = "" });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = (int)EstadoCivil.SOLTEIRO, Text = EstadoCivil.SOLTEIRO.GetDescription() });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = (int)EstadoCivil.CASADO, Text = EstadoCivil.CASADO.GetDescription() });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = (int)EstadoCivil.DIVORCIADO, Text = EstadoCivil.DIVORCIADO.GetDescription() });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = (int)EstadoCivil.VIUVO, Text = EstadoCivil.VIUVO.GetDescription() });
            lista.Add(new Posy.V2.MVC.Models.HtmlSelect { Value = (int)EstadoCivil.SEPARADO, Text = EstadoCivil.SEPARADO.GetDescription() });

            ViewBag.EstadoCivil = lista;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ImageCaptcha()
        {
            using (System.Drawing.Bitmap objBMP = new System.Drawing.Bitmap(200, 66))
            {
                using (System.Drawing.Graphics objGraphics = System.Drawing.Graphics.FromImage(objBMP))
                {
                    objGraphics.Clear(System.Drawing.Color.SkyBlue);
                    objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                    // Fonte configurada para ser usada no texto do captcha
                    using (System.Drawing.Font objFont = new System.Drawing.Font("Times New Roman", 30, System.Drawing.FontStyle.Strikeout))
                    {
                        string captchaValue = "";
                        int[] valuesArray = new int[8];
                        int x;

                        //Cria o valor randomicamente e adiciona ao array
                        Random autoRand = new Random();

                        for (x = 0; x < 8; x++)
                        {
                            valuesArray[x] = System.Convert.ToInt32(autoRand.Next(0, 9));
                            captchaValue += (valuesArray[x].ToString());
                        }

                        //Adiciona o valor gerado para o captcha na sessão

                        //para ser validado posteriormente
                        Session.Add("CaptchaValue", captchaValue);
                        //Application.Set("CaptchaValue", captchaValue);

                        //Desenha a imagem com o nosso texto
                        objGraphics.DrawString(captchaValue, objFont, System.Drawing.Brushes.White, 8, 8);

                        //Determina o tipo de conteúdo da imagem do captcha
                        //Response.ContentType = "image/GIF";

                        //Salva em stream
                        //objBMP.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);

                        MemoryStream ms = new MemoryStream();
                        objBMP.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

                        ViewBag.Captcha = captchaValue;

                        return File(ms.ToArray(), "image/GIF");
                    }
                }
            }
            //Libera os objeto da memória pois os mesmos não são mais necessários
            //objFont.Dispose();
            //objGraphics.Dispose();
            //objBMP.Dispose();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!model.Captcha.Equals((string)Session["CaptchaValue"]))
                        throw new Exception(Errors.CaptchaInvalido);
                }
                catch (Exception ex)
                {
                    CarregaListDrops();
                    AddErrors(new IdentityResult(new string[] { ex.Message }));
                    return View(model);
                }

                try
                {
                    var id = Guid.NewGuid();
                    var perfilValidation = new Perfil(id, model.Nome, model.Sobrenome, id.ToString(), new DateTime(model.Ano, model.Mes, model.Dia), (Sexo)model.Sexo, (EstadoCivil)model.EstadoCivil, "pt-BR");
                }
                catch (Exception ex)
                {
                    CarregaListDrops();
                    AddErrors(new IdentityResult(new string[] { ex.Message }));
                    return View(model);
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var perfil = new Perfil(user.Id, model.Nome, model.Sobrenome, user.Id.ToString(), new DateTime(model.Ano, model.Mes, model.Dia), (Sexo)model.Sexo, (EstadoCivil)model.EstadoCivil, "pt-BR");
                    ConfigPerfil(user, perfil);
                    ConfigFotoPerfil(user.Id);

                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, UIConfigLogin.EmailSubject);
                    ViewBag.Link = callbackUrl;
                    TempData["Link"] = callbackUrl;
                    return View("DisplayEmail");
                }

                AddErrors(result);
            }

            // No caso de falha, reexibir a view. 
            CarregaListDrops();
            return View(model);
        }

        private void ConfigPerfil(ApplicationUser user, Perfil perfil)
        {
            _perfilService.Inserir(perfil);
            _privacidadeService.IncluirPrivacidade(user.Id, 1, 1);
            _uow.Commit(new GlobalUser { UsuarioId = user.Id, Nome = $"{perfil.Nome} {perfil.Sobrenome}" });
        }
        private void ConfigFotoPerfil(Guid usuarioId)
        {
            try
            {
                string dirDestinoFotoPerfil = Server.MapPath("~") + "\\Images\\perfil\\" + usuarioId.ToString();
                string fotoPadrao = Server.MapPath("~") + "\\Images\\perfil\\0.jpg";
                string fotoPadraoPerfil = Server.MapPath("~") + "\\Images\\perfil\\" + usuarioId.ToString() + "\\1.jpg";

                if (!Directory.Exists(dirDestinoFotoPerfil))
                    Directory.CreateDirectory(dirDestinoFotoPerfil);

                if (System.IO.File.Exists(fotoPadraoPerfil))
                    System.IO.File.Delete(fotoPadraoPerfil);

                System.IO.File.Copy(fotoPadrao, fotoPadraoPerfil);
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErroConfiguracaoFotoPerfil);
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(Guid userId, string code)
        {
            if (userId == default(Guid) || code == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    /**
                    * Não revelar se o usuario nao existe ou nao esta confirmado
                    **/
                    return View("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await _userManager.SendEmailAsync(user.Id, UIConfigLogin.EsqueciMinhaSenha, string.Format(UIConfigLogin.EmailMessageEsqueciMinhaSenha, callbackUrl));
                //ViewBag.Link = callbackUrl;
                //ViewBag.Status = "DEMO: Caso o link não chegue: ";
                //ViewBag.LinkAcesso = callbackUrl;
                return View("ForgotPasswordConfirmation");
            }

            /**
            * No caso de falha, reexibir a view.
            **/
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Não revelar se o usuario nao existe ou nao esta confirmado
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await _signInManager.GetVerifiedUserIdAsync();
            if (userId == default(Guid))
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, UserId = userId });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Gerar o token e enviar
            if (!await _signInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, userId = model.UserId });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _userManager.FindAsync(loginInfo.Login);

            // Logar caso haja um login externo e já esteja logado neste provedor de login
            var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            CarregaListDrops();

            switch (result)
            {
                case SignInStatus.Success:
                    var userext = _userManager.FindByEmailAsync(user.Email);
                    await SignInAsync(userext.Result, false);
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    // Se ele nao tem uma conta solicite que crie uma

                    if (loginInfo.Login.LoginProvider.ToLower() == "facebook")
                    {
                        var externalIdentity = HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
                        var email = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:email").Value;
                        var firstName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:first_name").Value;
                        var lastName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:last_name").Value;

                        ViewBag.ReturnUrl = returnUrl;
                        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email, Sobrenome = lastName, Nome = firstName });
                    }
                    else if (loginInfo.Login.LoginProvider.ToLower() == "google")
                    {
                        var externalIdentity = HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
                        var email = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                        var firstName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value;
                        var lastName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;

                        ViewBag.ReturnUrl = returnUrl;
                        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email, Sobrenome = lastName, Nome = firstName });
                    }
                    else if (loginInfo.Login.LoginProvider.ToLower() == "twitter")
                    {
                        var externalIdentity = HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
                        var access_token = externalIdentity.Result.Claims.Where(x => x.Type == "urn:twitter:access_token").Select(x => x.Value).FirstOrDefault();
                        var access_secret = externalIdentity.Result.Claims.Where(x => x.Type == "urn:twitter:access_secret").Select(x => x.Value).FirstOrDefault();

                        var response = await TwitterHelper.TwitterLogin(access_token, access_secret, TwitterConfiguration.ConsumerKey, TwitterConfiguration.ConsumerSecret);

                        var email = response.email;
                        var firstName = response.name;
                        var lastName = string.Empty;

                        ViewBag.ReturnUrl = returnUrl;
                        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email, Sobrenome = lastName, Nome = firstName });
                    }
                    else
                    {
                        return View("Error");
                    }
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            CarregaListDrops();

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Pegar a informação do login externo.
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        var perfil = new Perfil(user.Id, model.Nome, model.Sobrenome, user.Id.ToString(), new DateTime(model.Ano, model.Mes, model.Dia), (Sexo)model.Sexo, (EstadoCivil)model.EstadoCivil, "pt-BR");
                        ConfigPerfil(user, perfil);
                        ConfigFotoPerfil(user.Id);

                        await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        var userext = _userManager.FindByEmailAsync(model.Email);
                        await SignInAsync(userext.Result, false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            await SignOutAsync();
            //AuthenticationManager.SignOut();
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }


        #region Reenviar link de confirmação de email

        // https://docs.microsoft.com/pt-br/aspnet/mvc/overview/security/create-an-aspnet-mvc-5-web-app-with-email-confirmation-and-password-reset

        private async Task<string> SendEmailConfirmationTokenAsync(Guid userID, string subject)
        {
            try
            {
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(userID);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code }, protocol: Request.Url.Scheme);
                await _userManager.SendEmailAsync(userID, subject, String.Format(UIConfigLogin.EmailMessage, callbackUrl));
                //await _userManager.SendEmailAsync(userID, subject, "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                //await _userManager.SendEmailAsync(user.Id, "Confirme sua Conta", "Por favor confirme sua conta clicando neste link: <a href='" + callbackUrl + "'></a>");

                return callbackUrl;

            }
            catch (Exception)
            {
                return "";
            }
        }

        #endregion

        public ActionResult SignOut()
        {
            ViewBag.Message = "Sair";

            return View();
        }

        private async Task SignOutAsync()
        {
            // Para verificação de erros
            // https://stackoverflow.com/questions/32352902/asp-net-mvc-usermanager-update?lq=1
            //try
            //{
            //    var clientKey = Request.Browser.Type;
            //    var user = _userManager.FindById(Guid.Parse(User.Identity.GetUserId()));
            //    await _userManager.SignOutClientAsync(user, clientKey);
            //    AuthenticationManager.SignOut();
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            Trace.TraceInformation("Property: {0} Error: {1}",
            //                             validationError.PropertyName,
            //                             validationError.ErrorMessage);
            //        }
            //    }
            //}

            var clientKey = Request.Browser.Type;
            var user = _userManager.FindById(Guid.Parse(User.Identity.GetUserId()));
            await _userManager.SignOutClientAsync(user, clientKey);
            AuthenticationManager.SignOut();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignOutEverywhere()
        {
            _userManager.UpdateSecurityStamp(Guid.Parse(User.Identity.GetUserId()));
            await SignOutAsync();

            return RedirectToAction("Login", "Account");
            //return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOutClient(int clientId)
        {
            var user = _userManager.FindById(Guid.Parse(User.Identity.GetUserId()));
            var client = user.UsuarioClientes.SingleOrDefault(c => c.UsuarioClienteId == clientId);
            if (client != null)
            {
                user.UsuarioClientes.Remove(client);
            }
            _userManager.Update(user);
            return RedirectToAction("Login", "Account");
            //return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }

                if (_perfilService != null)
                {
                    _perfilService.Dispose();
                    _perfilService = null;
                }
            }

            base.Dispose(disposing);
        }

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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Index", "Inicio");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                //// this line fixed the problem with returing null
                //context.RequestContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;

                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}