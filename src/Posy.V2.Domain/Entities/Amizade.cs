using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Amizade : EntityBase
    {
        public Guid AmizadeId { get; private set; }

        public Guid SolicitadoPorId { get; private set; }
        public Guid SolicitadoParaId { get; private set; }

        public DateTime DataSolicitacao { get; private set; }
        public DateTime? DataResposta { get; private set; }

        public SolicitacaoAmizade StatusSolicitacao { get; private set; }

        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }

        //[NotMapped]
        public bool Aprovado => StatusSolicitacao == SolicitacaoAmizade.Aprovado && Der == null;


        public virtual Usuario SolicitadoPor { get; set; }
        public virtual Usuario SolicitadoPara { get; set; }


        protected Amizade()
        {
            AmizadeId = Guid.NewGuid();
        }

        public Amizade(Guid solicitadoPorId, Guid solicitadoParaId)
        {
            AmizadeId = Guid.NewGuid();
            SolicitadoPorId = solicitadoPorId;
            SolicitadoParaId = solicitadoParaId;
            DataSolicitacao = ConfigurationBase.DataAtual;
            StatusSolicitacao = SolicitacaoAmizade.Pendente;
        }

        public void SetResposta(SolicitacaoAmizade statusSolicitacao)
        {
            DataResposta = ConfigurationBase.DataAtual;
            StatusSolicitacao = statusSolicitacao;
        }

        public void Delete(Guid usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }
    }
}
