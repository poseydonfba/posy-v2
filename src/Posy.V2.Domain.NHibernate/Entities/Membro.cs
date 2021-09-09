using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Membro : EntityBase
    {
        public virtual int ComunidadeId { get; set; }
        public virtual int UsuarioMembroId { get; set; }
        public virtual DateTime DataSolicitacao { get; set; }
        public virtual DateTime? DataResposta { get; set; }
        public virtual int? UsuarioRespostaId { get; set; }

        public virtual StatusSolicitacaoMembroComunidade StatusSolicitacao { get; set; }

        public virtual int? Uer { get; set; }
        public virtual DateTime? Der { get; set; }

        public virtual Comunidade Comunidade { get; set; }
        public virtual Usuario UsuarioMembro { get; set; }
        public virtual Usuario UsuarioResposta { get; set; }

        protected Membro()
        {
        }

        public Membro(int comunidadeId, int usuarioMembroId)
        {
            ComunidadeId = comunidadeId;
            UsuarioMembroId = usuarioMembroId;
            DataSolicitacao = ConfigurationBase.DataAtual;
            StatusSolicitacao = StatusSolicitacaoMembroComunidade.Pendente;
        }

        public virtual void AdicionarMembro()
        {
            SetResposta(StatusSolicitacaoMembroComunidade.Aprovado, UsuarioMembroId);
        }

        public virtual void SetResposta(StatusSolicitacaoMembroComunidade statusSolicitacao, int usuarioRespostaId)
        {
            DataResposta = ConfigurationBase.DataAtual;
            StatusSolicitacao = statusSolicitacao;
            UsuarioRespostaId = usuarioRespostaId;
        }

        public virtual void Delete(int usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }
    }
}
