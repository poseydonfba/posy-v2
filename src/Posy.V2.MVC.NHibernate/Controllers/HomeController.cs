using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Enums;
using Posy.V2.Infra.CrossCutting.Identity.NHibernate.Configuration;
using System;
using System.Web;
using System.Web.Mvc;

namespace Posy.V2.MVC.NHibernate.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsuarioService _usuarioService;
        private readonly IPerfilService _perfilService;
        private readonly IPrivacidadeService _privacidadeService;

        public HomeController(IUnitOfWork uow,
                              IUsuarioService usuarioService,
                              IPerfilService perfilService,
                              IPrivacidadeService privacidadeService)
        {
            _uow = uow;
            _usuarioService = usuarioService;
            _perfilService = perfilService;
            _privacidadeService = privacidadeService;
        }

        public ActionResult Index()
        {
            //var user = new User() { UserName = "poseydon" + Guid.NewGuid().ToString().Replace("-", "") };
            //var result = UserManager.Create(user, "1");

            var usuario = _usuarioService.GetUsuario(1);
            ////usuario.Perfil = new Perfil(usuario.Id, "Poseydon", "Espilacopa", usuario.Id.ToString(), new DateTime(1985, 12, 12), Sexo.MASCULINO, EstadoCivil.SEPARADO, "pt-BR");
            ////usuario.Privacidade = new Privacidade(usuario.Id, 1, 1);
            ////_usuarioService.SaveOrUpdate(usuario);

            var perfil = new Perfil(usuario.Id, "Poseydon", "Espilacopa", usuario.Id.ToString(), new DateTime(1985, 12, 12), Sexo.MASCULINO, EstadoCivil.SEPARADO, "pt-BR");
            perfil.Usuario = usuario;
            _perfilService.Inserir(perfil);

            _privacidadeService.IncluirPrivacidade(usuario.Id, 1, 1);

            _uow.Commit(new GlobalUser { UsuarioId = usuario.Id, Nome = $"Poseydon Espilacopa" });

            //var usuario = _usuarioService.GetUsuario(1);

            return View(CurrentUser);
        }

        public UserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
        }

        private User currentUser = null;
        public User CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    string userName = User.Identity.Name;
                    if (userName != null)
                    {
                        //currentUser = new ApplicationDbContext().Users.FindByNameAsync(userName).Result;
                    }
                }
                return currentUser;
            }
        }
    }
}