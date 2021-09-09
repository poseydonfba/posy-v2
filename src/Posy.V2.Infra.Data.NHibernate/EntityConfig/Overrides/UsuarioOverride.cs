using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class UsuarioOverride : IAutoMappingOverride<Usuario>
    {
        public void Override(AutoMapping<Usuario> mapping)
        {
            mapping.Table("Usuario");
            mapping.Id(x => x.Id).GeneratedBy.Identity();

            mapping.Not.LazyLoad();

            /**
            * Quando for one-to-one e a tabela perfil tem uma chave primaria Id e chave estrangeira UsuarioId
            **/
            //mapping.HasOne(x => x.Perfil).PropertyRef(x => x.Usuario);
            mapping.HasOne(x => x.Perfil).PropertyRef(x => x.Usuario);

            mapping.HasOne(x => x.Privacidade).PropertyRef(x => x.Usuario);

            mapping.HasMany(x => x.SolicitadosPor).KeyColumn("SolicitadoPorId").Not.LazyLoad();
            mapping.HasMany(x => x.SolicitadosPara).KeyColumn("SolicitadoParaId").Not.LazyLoad();

            mapping.HasMany(x => x.PostsPerfil).KeyColumn("UsuarioId").Not.LazyLoad();
            mapping.HasMany(x => x.PostsPerfilComentario).KeyColumn("UsuarioId").Not.LazyLoad();
            mapping.HasMany(x => x.PostsPerfilBloqueado).KeyColumn("UsuarioId").Not.LazyLoad();
            mapping.HasMany(x => x.PostsOculto).KeyColumn("UsuarioId").Not.LazyLoad();

            mapping.HasMany(x => x.Visitantes).KeyColumn("VisitanteId").Not.LazyLoad();
            mapping.HasMany(x => x.Visitados).KeyColumn("VisitadoId").Not.LazyLoad();

            mapping.HasMany(x => x.RecadosEnviadosPor).KeyColumn("EnviadoPorId").Not.LazyLoad();
            mapping.HasMany(x => x.RecadosEnviadosPara).KeyColumn("EnviadoParaId").Not.LazyLoad();

            mapping.HasMany(x => x.RecadosComentario).KeyColumn("UsuarioId").Not.LazyLoad();

            mapping.HasMany(x => x.Videos).KeyColumn("UsuarioId").Not.LazyLoad();
            mapping.HasMany(x => x.VideosComentario).KeyColumn("UsuarioId").Not.LazyLoad();

            mapping.HasMany(x => x.DepoimentosEnviadosPor).KeyColumn("EnviadoPorId").Not.LazyLoad();
            mapping.HasMany(x => x.DepoimentosEnviadosPara).KeyColumn("EnviadoParaId").Not.LazyLoad();

            mapping.HasMany(x => x.Stories).KeyColumn("UsuarioId").Not.LazyLoad();
            mapping.HasMany(x => x.Connections).KeyColumn("UsuarioId").Not.LazyLoad();

            mapping.HasMany(x => x.Comunidades).KeyColumn("UsuarioId").Not.LazyLoad();

            mapping.HasMany(x => x.Topicos).KeyColumn("UsuarioId").Not.LazyLoad();
            mapping.HasMany(x => x.TopicosPost).KeyColumn("UsuarioId").Not.LazyLoad();

            mapping.HasMany(x => x.MembrosUsuarioMembro).KeyColumn("UsuarioMembroId").Not.LazyLoad();
            mapping.HasMany(x => x.MembrosUsuarioResposta).KeyColumn("UsuarioRespostaId").Not.LazyLoad();

            mapping.HasMany(x => x.ModeradorUsuarioModerador).KeyColumn("UsuarioModeradorId").Not.LazyLoad();
            mapping.HasMany(x => x.ModeradorUsuarioOperacao).KeyColumn("UsuarioOperacaoId").Not.LazyLoad();

            //mapping.HasOne(x => x.Perfil).PropertyRef(x => x.Usuario);
            //mapping.HasOne(x => x.Privacidade).PropertyRef(x => x.Usuario);

            //mapping.HasOne(x => x.Perfil).PropertyRef("Usuario").Cascade.All();
            //mapping.HasOne(x => x.Privacidade).PropertyRef("Usuario").Cascade.All();

            //mapping.HasOne(x => x.Perfil).Cascade.All();
            //mapping.HasOne(x => x.Privacidade).Cascade.All();

            //mapping.References(x => x.Perfil).Unique().Cascade.All();
            //mapping.References(x => x.Privacidade).Unique().Cascade.All();

            //mapping.HasOne(x => x.Perfil).Constrained().Cascade.All().ForeignKey("UsuarioId");
            //mapping.HasOne(x => x.Privacidade).Constrained().Cascade.All().ForeignKey("UsuarioId");

            //mapping.References(x => x.Perfil).Column("Usuario");//.WithRequiredPrincipal(y => y.Usuario);
            //mapping.References(x => x.Privacidade).Column("Usuario");//.WithRequiredPrincipal(y => y.Usuario);//.Map(x => x.MapKey("UsuarioId"));
        }
    }
}