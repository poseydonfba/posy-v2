using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class AmizadeAudit : Auditable
    {
        public Guid AmizadeAuditId { get; private set; } = Guid.NewGuid();

        public Guid AmizadeId { get; private set; }

        public Guid SolicitadoPorId { get; private set; }
        public Guid SolicitadoParaId { get; private set; }

        public DateTime DataSolicitacao { get; private set; }
        public DateTime? DataResposta { get; private set; }

        public SolicitacaoAmizade StatusSolicitacao { get; private set; }

        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
