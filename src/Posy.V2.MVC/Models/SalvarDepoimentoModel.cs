using System.Web.Mvc;

namespace Posy.V2.MVC.Models
{
    public class SalvarDepoimentoModel
    {
        [AllowHtml]
        public string Depoimento { get; set; }
    }
}