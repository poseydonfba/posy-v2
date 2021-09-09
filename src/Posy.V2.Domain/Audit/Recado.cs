using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class RecadoAudit : Auditable
    {
        public Guid RecadoAuditId { get; private set; } = Guid.NewGuid();

        public Guid RecadoId { get; private set; }
        public Guid EnviadoPorId { get; private set; }
        public Guid EnviadoParaId { get; private set; }
        public byte[] DescricaoRecado { get; private set; }
        public DateTime DataRecado { get; private set; }
        public StatusRecado StatusRecado { get; private set; }
        public DateTime? DataLeitura { get; set; }
        public Guid? Uer { get; set; }
        public DateTime? Der { get; set; }
    }
}
