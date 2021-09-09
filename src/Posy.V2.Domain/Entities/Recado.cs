using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Recado : EntityBase
    {
        public Guid RecadoId { get; private set; }
        public Guid EnviadoPorId { get; private set; }
        public Guid EnviadoParaId { get; private set; }
        public byte[] DescricaoRecado { get; private set; }
        //[NotMapped]
        public string RecadoHtml
        {
            get
            {
                return DescricaoRecado == null ? "" : Conversion.ByteArrayToStr(DescricaoRecado);
            }
        }
        public DateTime DataRecado { get; private set; }
        public StatusRecado StatusRecado { get; private set; }
        public DateTime? DataLeitura { get; set; }
        public Guid? Uer { get; set; }
        public DateTime? Der { get; set; }

        public virtual Usuario EnviadoPor { get; set; }
        public virtual Usuario EnviadoPara { get; set; }

        //public virtual ICollection<RecadoComentario> RecadoComentarios { get; set; }

        protected Recado()
        {
            //RecadoComentarios = new List<RecadoComentario>();
        }

        public Recado(Guid enviadoPorId, Guid enviadoParaId, string descricaoRecado)
        {
            RecadoId = Guid.NewGuid();
            EnviadoPorId = enviadoPorId;
            EnviadoParaId = enviadoParaId;
            DescricaoRecado = Conversion.StrToByteArray(descricaoRecado);
            DataRecado = ConfigurationBase.DataAtual;
            StatusRecado = StatusRecado.NaoLido;

            //RecadoComentarios = new List<RecadoComentario>();

            Validate();
        }

        public void SetLeitura(StatusRecado statusRecado)
        {
            StatusRecado = statusRecado;
            DataLeitura = ConfigurationBase.DataAtual;
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
            Valid.AssertArgumentNotNull(DescricaoRecado, Errors.NenhumRecadoInformado);
        }
    }
}
