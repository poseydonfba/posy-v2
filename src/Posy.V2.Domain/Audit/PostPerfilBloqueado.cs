using System;

namespace Posy.V2.Domain.Audit
{
    public class PostPerfilBloqueadoAudit : Auditable
    {
        public Guid PostPerfilBloqueadoAuditId { get; private set; } = Guid.NewGuid();

        public Guid PostPerfilBloqueadoId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public Guid UsuarioIdBloqueado { get; private set; }
        public DateTime DataBloqueio { get; private set; }
        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
