using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class StorieAudit : Auditable
    {
        public Guid StorieAuditId { get; private set; } = Guid.NewGuid();

        public Guid StorieId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public StorieType StorieType { get; private set; }
        public int Length { get; private set; }
        public string Src { get; private set; }
        public string Preview { get; private set; }
        public string Link { get; private set; }
        public string LinkText { get; private set; }
        public string Seen { get; private set; }
        public string Time { get; private set; }
        public DateTime DataStorie { get; private set; }
    }
}
