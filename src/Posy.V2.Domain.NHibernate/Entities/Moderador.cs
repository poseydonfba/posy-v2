using Posy.V2.Infra.CrossCutting.Common;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Moderador : EntityBase
    {
        public virtual int ComunidadeId { get; set; }
        public virtual int UsuarioModeradorId { get; set; }
        public virtual int UsuarioOperacaoId { get; set; }
        public virtual DateTime DataOperacao { get; set; }
        public virtual int? Uer { get; set; }
        public virtual DateTime? Der { get; set; }

        public virtual Comunidade Comunidade { get; set; }
        public virtual Usuario UsuarioModerador { get; set; }
        public virtual Usuario UsuarioOperacao { get; set; }

        protected Moderador() { }

        public Moderador(int comunidadeId, int usuarioModeradorId, int usuarioOperacaoId)
        {
            ComunidadeId = comunidadeId;
            UsuarioModeradorId = usuarioModeradorId;
            UsuarioOperacaoId = usuarioOperacaoId;
            DataOperacao = ConfigurationBase.DataAtual;
        }

        public virtual void Delete(int usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }
    }
}
