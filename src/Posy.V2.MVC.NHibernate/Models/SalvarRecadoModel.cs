using System.Web.Mvc;

namespace Posy.V2.MVC.Models
{
    public class SalvarRecadoModel
    {
        [AllowHtml]
        public string RecadoHtml { get; set; }
    }
}