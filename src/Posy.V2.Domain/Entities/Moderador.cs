using Posy.V2.Infra.CrossCutting.Common;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Moderador : EntityBase
    {
        public Guid ModeradorId { get; private set; }
        public Guid ComunidadeId { get; private set; }
        public Guid UsuarioModeradorId { get; private set; }
        public Guid UsuarioOperacaoId { get; private set; }
        public DateTime DataOperacao { get; private set; }
        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }

        public virtual Comunidade Comunidade { get; set; }
        public virtual Usuario UsuarioModerador { get; set; }
        public virtual Usuario UsuarioOperacao { get; set; }

        protected Moderador() { }

        public Moderador(Guid comunidadeId, Guid usuarioModeradorId, Guid usuarioOperacaoId)
        {
            ModeradorId = Guid.NewGuid();
            ComunidadeId = comunidadeId;
            UsuarioModeradorId = usuarioModeradorId;
            UsuarioOperacaoId = usuarioOperacaoId;
            DataOperacao = ConfigurationBase.DataAtual;
        }

        public void Delete(Guid usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }
    }
}
