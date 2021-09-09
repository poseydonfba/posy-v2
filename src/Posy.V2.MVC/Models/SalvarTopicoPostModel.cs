using System.Web.Mvc;

namespace Posy.V2.MVC.Models
{
    public class SalvarTopicoPostModel
    {
        [AllowHtml]
        public string Descricao { get; set; }
    }
}