using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class TopicoAudit : Auditable
    {
        public Guid TopicoAuditId { get; private set; } = Guid.NewGuid();

        public Guid TopicoId { get; private set; }
        public Guid ComunidadeId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public DateTime DataTopico { get; private set; }
        public string Titulo { get; private set; }
        public byte[] Descricao { get; private set; }
        public TipoTopico TipoTopico { get; private set; }
        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }
        public Guid? Uerp { get; private set; }
        public DateTime? Derp { get; private set; }
    }
}
