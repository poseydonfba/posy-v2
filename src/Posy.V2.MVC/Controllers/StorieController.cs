using Newtonsoft.Json;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using Posy.V2.Infra.CrossCutting.Common.Extensions;
using Posy.V2.MVC.Attributes;
using Posy.V2.MVC.Controllers.Base;
using Posy.V2.MVC.Models;
using System;
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
            var stories = _storieService.ObterStories(PerfilLogged.UsuarioId, 0, 0).ToList();//.Select(x => new StoriesModel
            //{
            //    UsuarioId = x.Perfil.UsuarioId,
            //    Name = x.Perfil.Nome,
            //    LastUpdated = x.Stories.Max(y => y.DataStorie),
            //    Stories = x.Stories 
            //});

            //Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var results = (from e in stories
                          group new { e } by new { e.Usuario.Perfil.UsuarioId, e.Usuario.Perfil.Nome } into g
                          //orderby g.Key
                          select new StoriesModel
                          {
                              UsuarioId = g.Key.UsuarioId,
                              Name = g.Key.Nome,
                              LastUpdated = g.Max(c => c.e.DataStorie),
                              Stories = g.Select(c => new StorieModel {
                                  StorieId = c.e.StorieId,
                                  Type = c.e.StorieType.GetDescription(),
                                  Length = c.e.Length,
                                  Src = c.e.Src,
                                  Preview = c.e.Preview,
                                  Link = c.e.Link,
                                  LinkText = c.e.LinkText,
                                  Seen = c.e.Seen,
                                  Time = ((DateTimeOffset)c.e.DataStorie).ToUnixTimeSeconds().ToString(), // c.e.Time,
                                  //Time = (TimeZoneInfo.ConvertTimeToUtc(c.e.DataStorie) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds.ToString(), // c.e.Time,
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