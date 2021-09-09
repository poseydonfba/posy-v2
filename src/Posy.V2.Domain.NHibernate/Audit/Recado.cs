using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class RecadoAudit : Auditable
    {
        public int RecadoId { get; private set; }
        public int EnviadoPorId { get; private set; }
        public int EnviadoParaId { get; private set; }
        public byte[] DescricaoRecado { get; private set; }
        public DateTime DataRecado { get; private set; }
        public StatusRecado StatusRecado { get; private set; }
        public DateTime? DataLeitura { get; set; }
        public int? Uer { get; set; }
        public DateTime? Der { get; set; }
    }
}
