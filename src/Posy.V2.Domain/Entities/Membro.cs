using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Membro : EntityBase
    {
        public Guid MembroId { get; private set; }
        public Guid ComunidadeId { get; private set; }
        public Guid UsuarioMembroId { get; private set; }
        public DateTime DataSolicitacao { get; private set; }
        public DateTime? DataResposta { get; private set; }
        public Guid? UsuarioRespostaId { get; private set; }

        public StatusSolicitacaoMembroComunidade StatusSolicitacao { get; private set; }

        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }


        public virtual Comunidade Comunidade { get; set; }
        public virtual Usuario UsuarioMembro { get; set; }
        public virtual Usuario UsuarioResposta { get; set; }


        protected Membro()
        {
            MembroId = Guid.NewGuid();
        }

        public Membro(Guid comunidadeId, Guid usuarioMembroId)
        {
            MembroId = Guid.NewGuid();
            ComunidadeId = comunidadeId;
            UsuarioMembroId = usuarioMembroId;
            DataSolicitacao = ConfigurationBase.DataAtual;
            StatusSolicitacao = StatusSolicitacaoMembroComunidade.Pendente;
        }

        public void AdicionarMembro()
        {
            SetResposta(StatusSolicitacaoMembroComunidade.Aprovado, UsuarioMembroId);
        }

        public void SetResposta(StatusSolicitacaoMembroComunidade statusSolicitacao, Guid usuarioRespostaId)
        {
            DataResposta = ConfigurationBase.DataAtual;
            StatusSolicitacao = statusSolicitacao;
            UsuarioRespostaId = usuarioRespostaId;
        }

        public void Delete(Guid usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }
    }
}
