using System;

namespace Posy.V2.Domain.Audit
{
    public class ModeradorAudit : Auditable
    {
        public int ModeradorId { get; private set; }
        public int ComunidadeId { get; private set; }
        public int UsuarioModeradorId { get; private set; }
        public int UsuarioOperacaoId { get; private set; }
        public DateTime DataOperacao { get; private set; }
        public int? Uer { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
