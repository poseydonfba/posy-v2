using System;

namespace Posy.V2.Domain.Audit
{
    public class VisitantePerfilAudit : Auditable
    {
        public int VisitantePerfilId { get; private set; }
        public int VisitanteId { get; private set; }
        public int VisitadoId { get; private set; }
        public DateTime DataVisita { get; private set; }
    }
}
