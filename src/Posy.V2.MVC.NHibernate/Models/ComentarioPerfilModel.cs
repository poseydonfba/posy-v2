using System;
using System.Web.Mvc;

namespace Posy.V2.MVC.Models
{
    public class ComentarioPerfilModel
    {
        public int PostId { get; set; }

        [AllowHtml]
        public string Comentario { get; set; }
    }
}