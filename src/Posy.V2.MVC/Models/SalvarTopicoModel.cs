using Posy.V2.Domain.Enums;
using System.Web.Mvc;

namespace Posy.V2.MVC.Models
{
    public class SalvarTopicoModel
    {
        public string Titulo { get; set; }
        [AllowHtml]
        public string Descricao { get; set; }
        public TipoTopico Fixo { get; set; }
    }
}