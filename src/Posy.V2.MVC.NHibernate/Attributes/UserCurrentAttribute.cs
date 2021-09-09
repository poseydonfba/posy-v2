using System;
using System.Web;
using System.Web.Mvc;

namespace Posy.V2.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class UserCurrentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //VerifyUserAuthorized();
        }

        //protected bool UserIsAuthorized()
        //{
        //    return HttpContext.Current.User.Identity.IsAuthenticated;
        //}

        //protected void VerifyUserAuthorized()
        //{
        //    if (UserIsAuthorized())
        //        throw new System.Exception("Não autorizado");
        //    //throw new System.Web.HttpException(401, "Não autorizado");
        //}
    }
}