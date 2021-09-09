using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Amizade : EntityBase
    {
        public virtual int SolicitadoPorId { get; set; }
        public virtual int SolicitadoParaId { get; set; }

        public virtual DateTime DataSolicitacao { get; set; }
        public virtual DateTime? DataResposta { get; set; }

        public virtual SolicitacaoAmizade StatusSolicitacao { get; set; }

        public virtual int? Uer { get; set; }
        public virtual DateTime? Der { get; set; }

        //[NotMapped]
        public virtual bool Aprovado => StatusSolicitacao == SolicitacaoAmizade.Aprovado && Der == null;


        public virtual Usuario SolicitadoPor { get; set; }
        public virtual Usuario SolicitadoPara { get; set; }


        protected Amizade()
        {

        }

        public Amizade(int solicitadoPorId, int solicitadoParaId)
        {
            //SolicitadoPorId = solicitadoPorId;
            //SolicitadoParaId = solicitadoParaId;
            DataSolicitacao = ConfigurationBase.DataAtual;
            StatusSolicitacao = SolicitacaoAmizade.Pendente;
        }

        public virtual void SetResposta(SolicitacaoAmizade statusSolicitacao)
        {
            DataResposta = ConfigurationBase.DataAtual;
            StatusSolicitacao = statusSolicitacao;
        }

        public virtual void Delete(int usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }
    }
}
