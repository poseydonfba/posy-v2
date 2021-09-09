using System;

namespace Posy.V2.Domain.Audit
{
    public class TopicoPostAudit : Auditable
    {
        public int TopicoPostId { get; private set; }
        public int TopicoId { get; private set; }
        public int UsuarioId { get; private set; }
        public DateTime DataPost { get; private set; }
        public byte[] Descricao { get; private set; }
        public int? Uer { get; private set; }
        public DateTime? Der { get; private set; }
        public int? Uerp { get; private set; }
        public DateTime? Derp { get; private set; }
    }
}
