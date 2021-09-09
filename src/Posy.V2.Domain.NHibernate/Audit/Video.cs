using System;

namespace Posy.V2.Domain.Audit
{
    public class VideoAudit : Auditable
    {
        public int VideoId { get; private set; }
        public int UsuarioId { get; private set; }
        public string Url { get; private set; }
        public byte[] NomeVideo { get; private set; }
        public DateTime DataVideo { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
