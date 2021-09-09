using System.Web.Mvc;

namespace Posy.V2.MVC.Attributes
{
    public class ExceptionHandlerAttribute : HandleErrorAttribute
    {
        //private static readonly string[] DefinedException = { "SessionTimeOutException", "UnauthorizedAccessException" };
        //public override void OnException(ExceptionContext filterContext)
        //{
        //    if (filterContext.ExceptionHandled)
        //        return;

        //    var validationContainer = new ValidationContainer();
        //    filterContext.ExceptionHandled = true;
        //    var exceptionName = filterContext.Exception.GetType().Name;
        //    string exceptionMessage;
        //    if (DefinedException.Contains(exceptionName))
        //    {
        //        exceptionMessage = filterContext.Exception.Message;
        //    }
        //    else
        //    {
        //        exceptionMessage = "We intentionally allowed to not catch exception to track the root causes. Please note down the steps and include following details in the bug.</br>"
        //                           + "</br></br>Exception Message: " + filterContext.Exception;
        //    }
        //    validationContainer.AddMessage(MessageType.Error, exceptionMessage);

        //    filterContext.Result = new JsonResult
        //    {
        //        Data = validationContainer,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
        //}


        //https://stackoverflow.com/questions/14822089/asp-net-mvc-jquery-ajax-error-exception-handling
        //http://outbottle.com/net-mvc-3-custom-ajax-error-handling-2/
        //https://stackoverflow.com/questions/8702103/how-to-report-error-to-ajax-without-throwing-exception-in-mvc-controller
        ////throw new HttpException(400, "ModelState Invalid"); //HttpStatusCodeResult HttpException
        ///
        //throw new HttpException(401, "Não autorizado");

        //return new HttpStatusCodeResult(500, "Erro http 500");

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult()
                {
                    ContentType = "application/json",
                    Data = new
                    {
                        name = filterContext.Exception.GetType().Name,
                        message = filterContext.Exception.Message,
                        callstack = filterContext.Exception.StackTrace
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
            else
            {
                base.OnException(filterContext);
            }
        }
    }
}