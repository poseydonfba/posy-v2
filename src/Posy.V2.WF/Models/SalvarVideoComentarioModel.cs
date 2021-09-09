using System;

namespace Posy.V2.WF.Models
{
    public class SalvarVideoComentarioModel
    {
        public Guid VideoId { get; set; }
        public string Comentario { get; set; }
    }
}