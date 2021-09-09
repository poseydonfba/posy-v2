using System.Collections.Generic;
using System.Web.Http;

namespace Posy.V2.Api.Controllers
{
    public class DefaultController : ApiController
    { //
        // GET: api/default
        [AllowAnonymous]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
