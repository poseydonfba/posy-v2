using System.Web;
using System.Web.Mvc;

namespace Posy.V2.MVC.Controllers
{
    public class ErrorController : Controller
    {
        //public ViewResult Index()
        //{
        //    return View("Error");
        //}
        //public ViewResult NotFound()
        //{
        //    Response.StatusCode = 404;  //you may want to set this to 200
        //    return View("NotFound");
        //}

        public ActionResult Index()
        {
            return RedirectToAction("GenericError", new HandleErrorInfo(new HttpException(403, "Dont allow access the error pages"), "ErrorController", "Index"));
        }

        public ViewResult GenericError(HandleErrorInfo exception)
        {
            return View("Index", exception);
        }

        public ActionResult NotFound(HandleErrorInfo exception)
        {
            ViewBag.Title = "Page Not Found";
            return View("Index", exception);
        }
    }
}