using System;

namespace Posy.V2.Domain.Audit
{
    public class RecadoComentarioAudit : Auditable
    {
        public int RecadoComentarioId { get; private set; }
        public int RecadoId { get; private set; }
        public int UsuarioId { get; private set; }
        public byte[] DescricaoComentario { get; private set; }
        public DateTime DataComentario { get; private set; }
        public int? Uer { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
