using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class MembroAudit : Auditable
    {
        public int MembroId { get; private set; }
        public int ComunidadeId { get; private set; }
        public int UsuarioMembroId { get; private set; }
        public DateTime DataSolicitacao { get; private set; }
        public DateTime? DataResposta { get; private set; }
        public int? UsuarioRespostaId { get; private set; }

        public StatusSolicitacaoMembroComunidade StatusSolicitacao { get; private set; }

        public int? Uer { get; private set; }
        public DateTime? Der { get; private set; }
    }
}
