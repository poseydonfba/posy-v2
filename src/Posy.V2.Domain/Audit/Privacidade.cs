using System;

namespace Posy.V2.Domain.Audit
{
    public class PrivacidadeAudit : Auditable
    {
        public Guid PrivacidadeAuditId { get; private set; } = Guid.NewGuid();

        public Guid UsuarioId { get; private set; }
        public int VerRecado { get; private set; }
        public int EscreverRecado { get; private set; }
    }
}
