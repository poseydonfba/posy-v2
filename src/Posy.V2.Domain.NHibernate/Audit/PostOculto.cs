using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class PostOcultoAudit : Auditable
    {
        public int PostOcultoId { get; private set; }
        public int UsuarioId { get; private set; }
        public int PostPerfilId { get; private set; }
        public DateTime Data { get; private set; }
        public StatusPostOculto StatusPost { get; private set; }
    }
}
