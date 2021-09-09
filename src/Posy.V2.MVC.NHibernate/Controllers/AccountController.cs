using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Enums;
using Posy.V2.Infra.CrossCutting.Identity.NHibernate.Configuration;
using Posy.V2.Infra.CrossCutting.Identity.NHibernate.Model;
using System;
using System.Web;
using System.Web.Mvc;

namespace Posy.V2.MVC.NHibernate.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IPerfilService _perfilService;
        private readonly IPrivacidadeService _privacidadeService;

        public AccountController(IUnitOfWork uow,
                                 IPerfilService perfilService,
                                 IPrivacidadeService privacidadeService)
        {
            _uow = uow;
            _perfilService = perfilService;
            _privacidadeService = privacidadeService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = SignInManager.PasswordSignIn(model.UserName, model.Password, false, false);
                if (result == SignInStatus.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var perfilValidation = new Perfil(
                        1, "Poseydon", "Espilacopa", "1", new DateTime(1985, 12, 12), Sexo.MASCULINO, EstadoCivil.SEPARADO, "pt-BR");
                }
                catch (Exception ex)
                {
                    //CarregaListDrops();
                    ModelState.AddModelError("", ex.Message);
                    return View(model);
                }

                var user = new User() { UserName = model.UserName };
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    //var perfil = new Perfil(
                    //    1, "Poseydon", "Espilacopa", "1", new DateTime(1985, 12, 12), Sexo.MASCULINO, EstadoCivil.SEPARADO, "pt-BR");
                    //ConfigPerfil(user, perfil);

                    SignInManager.SignIn(user, false, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }
        private void ConfigPerfil(User user, Perfil perfil)
        {
            _perfilService.Inserir(perfil);
            _privacidadeService.IncluirPrivacidade(user.Id, 1, 1);
            _uow.Commit(new GlobalUser { UsuarioId = user.Id, Nome = $"{perfil.Nome} {perfil.Sobrenome}" });
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            SignInManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public SignInManager SignInManager
        {
            /// GetOwinContext()  Install-Package Microsoft.Owin.Host.SystemWeb
            get { return HttpContext.GetOwinContext().Get<SignInManager>(); }
        }
        public UserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
        }
    }
}