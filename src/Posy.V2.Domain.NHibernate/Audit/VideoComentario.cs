using System;

namespace Posy.V2.Domain.Audit
{
    public class VideoComentarioAudit : Auditable
    {
        public int VideoComentarioId { get; private set; }
        public int VideoId { get; private set; }
        public int UsuarioId { get; private set; }
        public byte[] DescricaoComentario { get; private set; }
        public DateTime DataComentario { get; private set; }
        public int? Uer { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
