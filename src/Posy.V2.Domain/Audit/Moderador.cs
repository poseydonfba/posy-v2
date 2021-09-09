using System;

namespace Posy.V2.Domain.Audit
{
    public class ModeradorAudit : Auditable
    {
        public Guid ModeradorAuditId { get; private set; } = Guid.NewGuid();

        public Guid ModeradorId { get; private set; }
        public Guid ComunidadeId { get; private set; }
        public Guid UsuarioModeradorId { get; private set; }
        public Guid UsuarioOperacaoId { get; private set; }
        public DateTime DataOperacao { get; private set; }
        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
