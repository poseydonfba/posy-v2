using System;

namespace Posy.V2.Domain.Audit
{
    public class PostPerfilAudit : Auditable
    {
        public Guid PostPerfilAuditId { get; private set; } = Guid.NewGuid();

        public Guid PostPerfilId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public byte[] DescricaoPost { get; private set; }
        public DateTime DataPost { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
