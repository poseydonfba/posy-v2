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
        public Guid DepoimentoId { get; private set; }
        public Guid EnviadoPorId { get; private set; }
        public Guid EnviadoParaId { get; private set; }
        public byte[] DescricaoDepoimento { get; private set; }
        //[NotMapped]
        public string DepoimentoHtml
        {
            get
            {
                return DescricaoDepoimento == null ? "" : Conversion.ByteArrayToStr(DescricaoDepoimento);
            }
        }
        public DateTime DataDepoimento { get; private set; }
        public StatusDepoimento StatusDepoimento { get; private set; }
        public DateTime? DataResposta { get; set; }
        public Guid? Uer { get; set; }
        public DateTime? Der { get; set; }

        public virtual Usuario EnviadoPor { get; set; }
        public virtual Usuario EnviadoPara { get; set; }

        protected Depoimento() { }

        public Depoimento(Guid enviadoPorId, Guid enviadoParaId, string descricaoDepoimento)
        {
            DepoimentoId = Guid.NewGuid();
            EnviadoPorId = enviadoPorId;
            EnviadoParaId = enviadoParaId;
            DescricaoDepoimento = Conversion.StrToByteArray(descricaoDepoimento);
            DataDepoimento = ConfigurationBase.DataAtual;
            StatusDepoimento = StatusDepoimento.Pendente;

            Validate();
        }

        public void SetResposta(StatusDepoimento flag)
        {
            StatusDepoimento = flag;
            DataResposta = ConfigurationBase.DataAtual;
        }

        public void Delete(Guid usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }

        public void Validate()
        {
            Valid.AssertArgumentNotNull(EnviadoPorId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(EnviadoParaId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(DescricaoDepoimento, Errors.NenhumDepoimentoInformado);
        }
    }
}
