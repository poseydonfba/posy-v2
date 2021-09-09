using Posy.V2.MVC.Helpers;
//using System.Net.Http;
using System.Web.Mvc;

namespace Posy.V2.MVC.Attributes
{
    public class DeflateCompressionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actContext)
        {
            //HttpContent content = (HttpContent)actContext.RequestContext.HttpContext;
            //var bytes = content == null ? null : content.ReadAsByteArrayAsync().Result;
            //var zlibbedContent = bytes == null ? new byte[0] :
            //CompressionHelper.DeflateByte(bytes);
            //actContext.RequestContext.HttpContext.Response.Write = new ByteArrayContent(zlibbedContent);
            //actContext.RequestContext.HttpContext.Response.Headers.Remove("Content-Type");
            //actContext.RequestContext.HttpContext.Response.Headers.Add("Content-encoding", "deflate");
            //actContext.RequestContext.HttpContext.Response.Headers.Add("Content-Type", "application/json");
            base.OnActionExecuted(actContext);
        }
    }
}