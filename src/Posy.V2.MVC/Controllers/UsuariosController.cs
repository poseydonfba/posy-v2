using System.Web.Mvc;
using Posy.V2.Domain.Interfaces.Repository;
using System;

namespace Posy.V2.MVC.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuariosController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // GET: Usuarios
        public ActionResult Index()
        {
            return View(_usuarioRepository.GetAll());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(Guid id)
        {
            return View(_usuarioRepository.Get(id));
        }

        public ActionResult DesativarLock(Guid id)
        {
            _usuarioRepository.DesativarLock(id);
            return RedirectToAction("Index");
        }
    }
}