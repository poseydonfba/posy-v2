using System;

namespace Posy.V2.Domain.Audit
{
    public class PostPerfilComentarioAudit : Auditable
    {
        public int PostPerfilComentarioId { get; private set; }
        public int PostPerfilId { get; private set; }
        public int UsuarioId { get; private set; }
        public byte[] Comentario { get; private set; }
        public DateTime Data { get; private set; }
        public int? Uer { get; set; }
        public DateTime? Der { get; private set; }
    }
}
