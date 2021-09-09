using Newtonsoft.Json;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using Posy.V2.Infra.CrossCutting.Common.Extensions;
using Posy.V2.MVC.Attributes;
using Posy.V2.MVC.Controllers.Base;
using Posy.V2.MVC.Models;
using System.Linq;
using System.Web.Mvc;

namespace Posy.V2.MVC.Controllers
{
    [Authorize]
    [ExceptionHandler]
    [UserCurrent]
    public class StorieController : BaseControllerPerfil
    {
        private readonly IStorieService _storieService;

        public StorieController(IUnitOfWork uow,
                                ICacheService cacheService,
                                IPerfilService perfilService,
                                IAmizadeService amizadeService,
                                IStorieService storieService,
                                IGlobalBaseController globalBaseController) :
            base(globalBaseController, uow, cacheService, perfilService)
        {
            _storieService = storieService;
        }

        // GET: Storie
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("ajax/stories")]
        public ActionResult GetPerfil()
        {
            var stories = _storieService.ObterStories(PerfilLogged.Id, 0, 0).ToList();//.Select(x => new StoriesModel
            //{
            //    UsuarioId = x.Perfil.UsuarioId,
            //    Name = x.Perfil.Nome,
            //    LastUpdated = x.Stories.Max(y => y.DataStorie),
            //    Stories = x.Stories 
            //});

            var results = (from e in stories
                          group new { e } by new { e.Usuario.Perfil.Id, e.Usuario.Perfil.Nome } into g
                          //orderby g.Key
                          select new StoriesModel
                          {
                              UsuarioId = g.Key.Id,
                              Name = g.Key.Nome,
                              LastUpdated = g.Max(c => c.e.DataStorie),
                              Stories = g.Select(c => new StorieModel {
                                  StorieId = c.e.Id,
                                  StorieType = c.e.StorieType,
                                  Length = c.e.Length,
                                  Src = c.e.Src,
                                  Preview = c.e.Preview,
                                  Link = c.e.Link,
                                  LinkText = c.e.LinkText,
                                  Seen = c.e.Seen,
                                  Time = c.e.Time,
                                  DataStorie = c.e.DataStorie
                              }).ToList() // g.Select(c => c.e).ToList() //.Distinct()
                          }
                          ).ToList();

            results = results.OrderBy(x => x.LastUpdated).ToList();

            var list = JsonConvert.SerializeObject(results,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

            return Content(list, "application/json");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _storieService.Dispose();
        }
    }
}