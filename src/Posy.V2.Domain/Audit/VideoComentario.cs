using System;

namespace Posy.V2.Domain.Audit
{
    public class VideoComentarioAudit : Auditable
    {
        public Guid VideoComentarioAuditId { get; private set; } = Guid.NewGuid();

        public Guid VideoComentarioId { get; private set; }
        public Guid VideoId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public byte[] DescricaoComentario { get; private set; }
        public DateTime DataComentario { get; private set; }
        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
