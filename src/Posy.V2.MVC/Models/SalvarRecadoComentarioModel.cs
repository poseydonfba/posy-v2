using System;
using System.Web.Mvc;

namespace Posy.V2.MVC.Models
{
    public class SalvarRecadoComentarioModel
    {
        public Guid RecadoId { get; set; }
        [AllowHtml]
        public string Comentario { get; set; }
    }
}