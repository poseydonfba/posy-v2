using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class PostPerfilComentario : EntityBase
    {
        public Guid PostPerfilComentarioId { get; private set; }
        public Guid PostPerfilId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public byte[] Comentario { get; private set; }
        public DateTime Data { get; private set; }
        public Guid? Uer { get; set; }
        public DateTime? Der { get; private set; }

        public virtual PostPerfil PostPerfil { get; set; }
        public virtual Usuario Usuario { get; set; }

        public string ComentarioHtml
        {
            get
            {
                return Comentario == null ? "" : Conversion.ByteArrayToStr(Comentario);
            }
        }

        protected PostPerfilComentario()
        {
            PostPerfilComentarioId = Guid.NewGuid();
            Data = ConfigurationBase.DataAtual;
        }

        public PostPerfilComentario(Guid postPerfilId, Guid usuarioId, string comentario)
        {
            PostPerfilComentarioId = Guid.NewGuid();
            PostPerfilId = postPerfilId;
            UsuarioId = usuarioId;
            Comentario = Conversion.StrToByteArray(comentario);
            Data = ConfigurationBase.DataAtual;

            Validate();
        }

        public void Delete(Guid usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }

        public void Validate()
        {
            Valid.AssertArgumentNotNull(PostPerfilId, Errors.PostInvalido);
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(Comentario, Errors.NenhumComentarioInformado);
        }
    }
}
