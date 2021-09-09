using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class RecadoComentario : EntityBase
    {
        public Guid RecadoComentarioId { get; private set; }
        public Guid RecadoId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public byte[] DescricaoComentario { get; private set; }
        //[NotMapped]
        public string ComentarioHtml
        {
            get
            {
                return DescricaoComentario == null ? "" : Conversion.ByteArrayToStr(DescricaoComentario);
            }
        }
        public DateTime DataComentario { get; private set; }
        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }

        public virtual Recado Recado { get; set; }
        public virtual Usuario Usuario { get; set; }

        protected RecadoComentario() { }

        public RecadoComentario(Guid recadoId, Guid usuarioId, string descricaoComentario)
        {
            RecadoComentarioId = Guid.NewGuid();
            RecadoId = recadoId;
            UsuarioId = usuarioId;
            DescricaoComentario = Conversion.StrToByteArray(descricaoComentario);
            DataComentario = ConfigurationBase.DataAtual;

            Validate();
        }

        public void Delete(Guid usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }

        public void Validate()
        {
            Valid.AssertArgumentNotNull(RecadoId, Errors.RecadoInvalido);
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(DescricaoComentario, Errors.NenhumComentarioInformado);
        }
    }
}
