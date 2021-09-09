using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class DepoimentoAudit : Auditable
    {
        public int DepoimentoId { get; private set; }
        public int EnviadoPorId { get; private set; }
        public int EnviadoParaId { get; private set; }
        public byte[] DescricaoDepoimento { get; private set; }
        public DateTime DataDepoimento { get; private set; }
        public StatusDepoimento StatusDepoimento { get; private set; }
        public DateTime? DataResposta { get; set; }
        public int? Uer { get; set; }
        public DateTime? Der { get; set; }
    }
}
