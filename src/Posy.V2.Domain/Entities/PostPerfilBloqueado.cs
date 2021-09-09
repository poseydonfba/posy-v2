using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class PostPerfilBloqueado : EntityBase
    {
        public Guid PostPerfilBloqueadoId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public Guid UsuarioIdBloqueado { get; private set; }
        public DateTime DataBloqueio { get; private set; }
        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }

        public virtual Usuario Usuario { get; set; }

        protected PostPerfilBloqueado()
        {
            PostPerfilBloqueadoId = Guid.NewGuid();
            DataBloqueio = ConfigurationBase.DataAtual;
        }

        public PostPerfilBloqueado(Guid usuarioId, Guid usuarioIdBloqueado)
        {
            PostPerfilBloqueadoId = Guid.NewGuid();
            UsuarioId = usuarioId;
            UsuarioIdBloqueado = usuarioIdBloqueado;

            DataBloqueio = ConfigurationBase.DataAtual;

            Validate();
        }

        public void Validate()
        {
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(UsuarioIdBloqueado, Errors.UsuarioInvalido);
        }
    }
}
