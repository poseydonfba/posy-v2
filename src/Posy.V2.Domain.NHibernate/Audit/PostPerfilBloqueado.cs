using System;

namespace Posy.V2.Domain.Audit
{
    public class PostPerfilBloqueadoAudit : Auditable
    {
        public int PostPerfilBloqueadoId { get; private set; }
        public int UsuarioId { get; private set; }
        public int UsuarioIdBloqueado { get; private set; }
        public DateTime DataBloqueio { get; private set; }
        public int? Uer { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
