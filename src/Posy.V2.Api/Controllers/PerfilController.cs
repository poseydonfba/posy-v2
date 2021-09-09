using Microsoft.AspNet.Identity;
using Posy.V2.Domain.Interfaces.Service;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Posy.V2.Api.Controllers
{
    [RoutePrefix("api/v1/perfil")]
    public class PerfilController : ApiController
    {
        private readonly IPostPerfilService _postService;

        public PerfilController(IPostPerfilService postService)
        {
            _postService = postService;
        }

        private Guid GetIdentityUserId()
        {
            return Guid.Parse(User.Identity.GetUserId());
        }

        // GET api/v1/perfil/feed/{page}
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("feed/{page}")]
        //[OutputCache(Duration = 5, VaryByParam = "page")]
        public async Task<IHttpActionResult> GetListInicioAtualizacao(int page)
        {
            var posts = _postService.ObterPosts(GetIdentityUserId(), page, 10).ToList();

            return Ok(posts);
        }
    }
}
