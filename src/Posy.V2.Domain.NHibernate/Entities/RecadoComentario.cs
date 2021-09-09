using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class RecadoComentario : EntityBase
    {
        public virtual int RecadoId { get; set; }
        public virtual int UsuarioId { get; set; }
        public virtual byte[] DescricaoComentario { get; set; }
        //[NotMapped]
        public virtual string ComentarioHtml
        {
            get
            {
                return DescricaoComentario == null ? "" : Conversion.ByteArrayToStr(DescricaoComentario);
            }
        }
        public virtual DateTime DataComentario { get; set; }
        public virtual int? Uer { get; set; }
        public virtual DateTime? Der { get; set; }

        public virtual Recado Recado { get; set; }
        public virtual Usuario Usuario { get; set; }

        protected RecadoComentario() { }

        public RecadoComentario(int recadoId, int usuarioId, string descricaoComentario)
        {
            RecadoId = recadoId;
            UsuarioId = usuarioId;
            DescricaoComentario = Conversion.StrToByteArray(descricaoComentario);
            DataComentario = ConfigurationBase.DataAtual;

            Validate();
        }

        public virtual void Delete(int usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }

        public virtual void Validate()
        {
            Valid.AssertArgumentNotNull(RecadoId, Errors.RecadoInvalido);
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(DescricaoComentario, Errors.NenhumComentarioInformado);
        }
    }
}
