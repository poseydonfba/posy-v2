using System;

namespace Posy.V2.Domain.Audit
{
    public class VisitantePerfilAudit : Auditable
    {
        public Guid VisitantePerfilAuditId { get; private set; } = Guid.NewGuid();

        public Guid VisitantePerfilId { get; private set; }
        public Guid VisitanteId { get; private set; }
        public Guid VisitadoId { get; private set; }
        public DateTime DataVisita { get; private set; }
    }
}
