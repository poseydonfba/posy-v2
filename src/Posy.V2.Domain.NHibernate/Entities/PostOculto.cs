using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class PostOculto : EntityBase
    {
        public virtual int UsuarioId { get; set; }
        public virtual int PostPerfilId { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual StatusPostOculto StatusPost { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual PostPerfil PostPerfil { get; set; }

        protected PostOculto()
        {
            Data = ConfigurationBase.DataAtual;
        }

        public PostOculto(int usuarioId, int postPerfilId, StatusPostOculto statusPost)
        {
            UsuarioId = usuarioId;
            PostPerfilId = postPerfilId;
            StatusPost = statusPost;
            Data = ConfigurationBase.DataAtual;

            Validate();
        }

        public virtual void SetOcultar()
        {
            StatusPost = StatusPostOculto.Oculto;
        }

        public virtual void Validate()
        {
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(PostPerfilId, Errors.PostInvalido);
        }
    }
}
