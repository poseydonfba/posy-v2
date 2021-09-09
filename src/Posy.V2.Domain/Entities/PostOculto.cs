using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class PostOculto : EntityBase
    {
        public Guid PostOcultoId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public Guid PostPerfilId { get; private set; }
        public DateTime Data { get; private set; }
        public StatusPostOculto StatusPost { get; private set; }

        public virtual Usuario Usuario { get; set; }
        public virtual PostPerfil PostPerfil { get; set; }

        protected PostOculto()
        {
            PostOcultoId = Guid.NewGuid();
            Data = ConfigurationBase.DataAtual;
        }

        public PostOculto(Guid usuarioId, Guid postPerfilId, StatusPostOculto statusPost)
        {
            PostOcultoId = Guid.NewGuid();
            UsuarioId = usuarioId;
            PostPerfilId = postPerfilId;
            StatusPost = statusPost;
            Data = ConfigurationBase.DataAtual;

            Validate();
        }

        public void SetOcultar()
        {
            StatusPost = StatusPostOculto.Oculto;
        }

        public void Validate()
        {
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(PostPerfilId, Errors.PostInvalido);
        }
    }
}
