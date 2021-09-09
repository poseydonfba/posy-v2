using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class PostPerfilComentario : EntityBase
    {
        public virtual int PostPerfilId { get; set; }
        public virtual int UsuarioId { get; set; }
        public virtual byte[] Comentario { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual int? Uer { get; set; }
        public virtual DateTime? Der { get; set; }

        public virtual PostPerfil PostPerfil { get; set; }
        public virtual Usuario Usuario { get; set; }
        
        public virtual string ComentarioHtml
        {
            get
            {
                return Comentario == null ? "" : Conversion.ByteArrayToStr(Comentario);
            }
        }

        protected PostPerfilComentario()
        {
            Data = ConfigurationBase.DataAtual;
        }

        public PostPerfilComentario(int postPerfilId, int usuarioId, string comentario)
        {
            PostPerfilId = postPerfilId;
            UsuarioId = usuarioId;
            Comentario = Conversion.StrToByteArray(comentario);
            Data = ConfigurationBase.DataAtual;

            Validate();
        }

        public virtual void Delete(int usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }

        public virtual void Validate()
        {
            Valid.AssertArgumentNotNull(PostPerfilId, Errors.PostInvalido);
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(Comentario, Errors.NenhumComentarioInformado);
        }
    }
}
