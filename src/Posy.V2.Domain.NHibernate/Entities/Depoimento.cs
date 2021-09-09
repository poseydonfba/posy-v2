using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Depoimento : EntityBase
    {
        public virtual int EnviadoPorId { get; set; }
        public virtual int EnviadoParaId { get; set; }
        public virtual byte[] DescricaoDepoimento { get; set; }
        //[NotMapped]
        public virtual string DepoimentoHtml
        {
            get
            {
                return DescricaoDepoimento == null ? "" : Conversion.ByteArrayToStr(DescricaoDepoimento);
            }
        }
        public virtual DateTime DataDepoimento { get; set; }
        public virtual StatusDepoimento StatusDepoimento { get; set; }
        public virtual DateTime? DataResposta { get; set; }
        public virtual int? Uer { get; set; }
        public virtual DateTime? Der { get; set; }

        public virtual Usuario EnviadoPor { get; set; }
        public virtual Usuario EnviadoPara { get; set; }

        protected Depoimento() { }

        public Depoimento(int enviadoPorId, int enviadoParaId, string descricaoDepoimento)
        {
            EnviadoPorId = enviadoPorId;
            EnviadoParaId = enviadoParaId;
            DescricaoDepoimento = Conversion.StrToByteArray(descricaoDepoimento);
            DataDepoimento = ConfigurationBase.DataAtual;
            StatusDepoimento = StatusDepoimento.Pendente;

            Validate();
        }

        public virtual void SetResposta(StatusDepoimento flag)
        {
            StatusDepoimento = flag;
            DataResposta = ConfigurationBase.DataAtual;
        }

        public virtual void Delete(int usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }

        public virtual void Validate()
        {
            Valid.AssertArgumentNotNull(EnviadoPorId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(EnviadoParaId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(DescricaoDepoimento, Errors.NenhumDepoimentoInformado);
        }
    }
}
