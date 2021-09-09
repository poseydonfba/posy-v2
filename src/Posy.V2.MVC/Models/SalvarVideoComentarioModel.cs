using System;
using System.Web.Mvc;

namespace Posy.V2.MVC.Models
{
    public class SalvarVideoComentarioModel
    {
        public Guid VideoId { get; set; }
        [AllowHtml]
        public string Comentario { get; set; }
    }
}