using System;

namespace Posy.V2.Domain.Audit
{
    public class RecadoComentarioAudit : Auditable
    {
        public Guid RecadoComentarioAuditId { get; private set; } = Guid.NewGuid();

        public Guid RecadoComentarioId { get; private set; }
        public Guid RecadoId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public byte[] DescricaoComentario { get; private set; }
        public DateTime DataComentario { get; private set; }
        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
