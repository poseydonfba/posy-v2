using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class PostPerfilOverride : IAutoMappingOverride<PostPerfil>
    {
        public void Override(AutoMapping<PostPerfil> mapping)
        {
            mapping.Table("PostPerfil");
            mapping.Id(x => x.Id);

            mapping.IgnoreProperty(x => x.PostHtml);
            mapping.IgnoreProperty(x => x.IsOculto);

            mapping.HasOne(x => x.Usuario);

            mapping.HasMany(x => x.PostsPerfilComentario).KeyColumn("PostPerfilId").Not.LazyLoad();

            mapping.HasMany(x => x.PostsOculto).KeyColumn("PostPerfilId").Not.LazyLoad();
        }
    }
}