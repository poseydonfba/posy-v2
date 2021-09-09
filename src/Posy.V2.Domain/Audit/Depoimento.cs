using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class DepoimentoAudit : Auditable
    {
        public Guid DepoimentoAuditId { get; private set; } = Guid.NewGuid();

        public Guid DepoimentoId { get; private set; }
        public Guid EnviadoPorId { get; private set; }
        public Guid EnviadoParaId { get; private set; }
        public byte[] DescricaoDepoimento { get; private set; }
        public DateTime DataDepoimento { get; private set; }
        public StatusDepoimento StatusDepoimento { get; private set; }
        public DateTime? DataResposta { get; set; }
        public Guid? Uer { get; set; }
        public DateTime? Der { get; set; }
    }
}
