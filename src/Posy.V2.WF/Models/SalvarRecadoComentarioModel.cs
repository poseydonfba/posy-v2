using System;

namespace Posy.V2.WF.Models
{
    public class SalvarRecadoComentarioModel
    {
        public Guid RecadoId { get; set; }
        public string Comentario { get; set; }
    }
}