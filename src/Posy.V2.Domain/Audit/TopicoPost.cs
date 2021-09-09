using System;

namespace Posy.V2.Domain.Audit
{
    public class TopicoPostAudit : Auditable
    {
        public Guid TopicoPostAuditId { get; private set; } = Guid.NewGuid();

        public Guid TopicoPostId { get; private set; }
        public Guid TopicoId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public DateTime DataPost { get; private set; }
        public byte[] Descricao { get; private set; }
        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }
        public Guid? Uerp { get; private set; }
        public DateTime? Derp { get; private set; }
    }
}
