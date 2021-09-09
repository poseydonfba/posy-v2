using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class PostPerfilBloqueado : EntityBase
    {
        public virtual int UsuarioId { get; set; }
        public virtual int UsuarioIdBloqueado { get; set; }
        public virtual DateTime DataBloqueio { get; set; }
        public virtual Guid? Uer { get; set; }
        public virtual DateTime? Der { get; set; }

        public virtual Usuario Usuario { get; set; }

        protected PostPerfilBloqueado()
        {
            DataBloqueio = ConfigurationBase.DataAtual;
        }

        public PostPerfilBloqueado(int usuarioId, int usuarioIdBloqueado)
        {
            UsuarioId = usuarioId;
            UsuarioIdBloqueado = usuarioIdBloqueado;

            DataBloqueio = ConfigurationBase.DataAtual;

            Validate();
        }

        public virtual void Validate()
        {
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(UsuarioIdBloqueado, Errors.UsuarioInvalido);
        }
    }
}
