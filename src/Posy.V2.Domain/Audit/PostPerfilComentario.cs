using System;

namespace Posy.V2.Domain.Audit
{
    public class PostPerfilComentarioAudit : Auditable
    {
        public Guid PostPerfilComentarioAuditId { get; private set; } = Guid.NewGuid();

        public Guid PostPerfilComentarioId { get; private set; }
        public Guid PostPerfilId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public byte[] Comentario { get; private set; }
        public DateTime Data { get; private set; }
        public Guid? Uer { get; set; }
        public DateTime? Der { get; private set; }
    }
}
