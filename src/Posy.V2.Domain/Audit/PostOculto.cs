using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class PostOcultoAudit : Auditable
    {
        public Guid PostOcultoAuditId { get; private set; } = Guid.NewGuid();

        public Guid PostOcultoId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public Guid PostPerfilId { get; private set; }
        public DateTime Data { get; private set; }
        public StatusPostOculto StatusPost { get; private set; }
    }
}
