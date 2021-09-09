using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Entities
{
    public class Usuario : EntityBase
    {
        public virtual DateTime Dir { get; set; }

        public virtual DateTime? Der { get; set; }

        public virtual string Email { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual string SecurityStamp { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }

        public virtual DateTime? LockoutEndDateUtc { get; set; }

        public virtual bool LockoutEnabled { get; set; }

        public virtual int AccessFailedCount { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Culture { get; set; }

        public virtual string UICulture { get; set; }

        public virtual string CurrencySymbol { get; set; }

        public virtual string Language { get; set; }

        public virtual string LongDateFormat { get; set; }

        public virtual string ShortDateFormat { get; set; }


        public virtual Perfil Perfil { get; set; }

        public virtual Privacidade Privacidade { get; set; }

        public virtual ICollection<Amizade> SolicitadosPor { get; set; }
        public virtual ICollection<Amizade> SolicitadosPara { get; set; }

        public virtual ICollection<PostPerfil> PostsPerfil { get; set; }

        public virtual ICollection<PostPerfilComentario> PostsPerfilComentario { get; set; }

        public virtual ICollection<PostPerfilBloqueado> PostsPerfilBloqueado { get; set; }

        public virtual ICollection<PostOculto> PostsOculto { get; set; }

        public virtual ICollection<VisitantePerfil> Visitantes { get; set; }
        public virtual ICollection<VisitantePerfil> Visitados { get; set; }

        public virtual ICollection<Recado> RecadosEnviadosPor { get; set; }
        public virtual ICollection<Recado> RecadosEnviadosPara { get; set; }

        public virtual ICollection<RecadoComentario> RecadosComentario { get; set; }

        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<VideoComentario> VideosComentario { get; set; }

        public virtual ICollection<Depoimento> DepoimentosEnviadosPor { get; set; }
        public virtual ICollection<Depoimento> DepoimentosEnviadosPara { get; set; }

        public virtual ICollection<Connection> Connections { get; set; }
        public virtual ICollection<Storie> Stories { get; set; }

        public virtual ICollection<Comunidade> Comunidades { get; set; }

        public virtual ICollection<Topico> Topicos { get; set; }

        public virtual ICollection<TopicoPost> TopicosPost { get; set; }

        public virtual ICollection<Membro> MembrosUsuarioMembro { get; set; }
        public virtual ICollection<Membro> MembrosUsuarioResposta { get; set; }

        public virtual ICollection<Moderador> ModeradorUsuarioModerador { get; set; }
        public virtual ICollection<Moderador> ModeradorUsuarioOperacao { get; set; }
    }
}
