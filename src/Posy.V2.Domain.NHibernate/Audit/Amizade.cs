using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class AmizadeAudit : Auditable
    {
        public int AmizadeId { get; private set; }

        public int SolicitadoPorId { get; private set; }
        public int SolicitadoParaId { get; private set; }

        public DateTime DataSolicitacao { get; private set; }
        public DateTime? DataResposta { get; private set; }

        public SolicitacaoAmizade StatusSolicitacao { get; private set; }

        public int? Uer { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
