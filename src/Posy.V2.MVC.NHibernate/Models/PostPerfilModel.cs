using System.Web.Mvc;

namespace Posy.V2.MVC.Models
{
    public class PostPerfilModel
    {
        [AllowHtml]
        public string PostHtml { get; set; }
    }
}