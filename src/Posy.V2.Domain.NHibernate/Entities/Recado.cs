using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Entities
{
    public class Recado : EntityBase
    {
        public virtual int EnviadoPorId { get; set; }
        public virtual int EnviadoParaId { get; set; }
        public virtual byte[] DescricaoRecado { get; set; }
        //[NotMapped]
        public virtual string RecadoHtml
        {
            get
            {
                return DescricaoRecado == null ? "" : Conversion.ByteArrayToStr(DescricaoRecado);
            }
        }
        public virtual DateTime DataRecado { get; set; }
        public virtual StatusRecado StatusRecado { get; set; }
        public virtual DateTime? DataLeitura { get; set; }
        public virtual int? Uer { get; set; }
        public virtual DateTime? Der { get; set; }

        public virtual Usuario EnviadoPor { get; set; }
        public virtual Usuario EnviadoPara { get; set; }

        public virtual ICollection<RecadoComentario> RecadosComentario { get; set; }

        protected Recado()
        {
            //RecadoComentarios = new List<RecadoComentario>();
        }

        public Recado(int enviadoPorId, int enviadoParaId, string descricaoRecado)
        {
            EnviadoPorId = enviadoPorId;
            EnviadoParaId = enviadoParaId;
            DescricaoRecado = Conversion.StrToByteArray(descricaoRecado);
            DataRecado = ConfigurationBase.DataAtual;
            StatusRecado = StatusRecado.NaoLido;

            //RecadoComentarios = new List<RecadoComentario>();

            Validate();
        }

        public virtual void SetLeitura(StatusRecado statusRecado)
        {
            StatusRecado = statusRecado;
            DataLeitura = ConfigurationBase.DataAtual;
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
            Valid.AssertArgumentNotNull(DescricaoRecado, Errors.NenhumRecadoInformado);
        }
    }
}
