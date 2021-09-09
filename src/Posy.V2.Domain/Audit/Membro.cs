using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class MembroAudit : Auditable
    {
        public Guid MembroAuditId { get; private set; } = Guid.NewGuid();

        public Guid MembroId { get; private set; }
        public Guid ComunidadeId { get; private set; }
        public Guid UsuarioMembroId { get; private set; }
        public DateTime DataSolicitacao { get; private set; }
        public DateTime? DataResposta { get; private set; }
        public Guid? UsuarioRespostaId { get; private set; }

        public StatusSolicitacaoMembroComunidade StatusSolicitacao { get; private set; }

        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
