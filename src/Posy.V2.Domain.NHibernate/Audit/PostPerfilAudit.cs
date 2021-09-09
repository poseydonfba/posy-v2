using System;

namespace Posy.V2.Domain.Audit
{
    public class PostPerfilAudit : Auditable
    {
        public int PostPerfilId { get; private set; }
        public int UsuarioId { get; private set; }
        public byte[] DescricaoPost { get; private set; }
        public DateTime DataPost { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
