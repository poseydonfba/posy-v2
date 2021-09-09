using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class ComunidadeOverride : IAutoMappingOverride<Comunidade>
    {
        public void Override(AutoMapping<Comunidade> mapping)
        {
            mapping.Table("Comunidade");
            mapping.Id(x => x.Id);

            mapping.IgnoreProperty(x => x.PerfilHtml);
            mapping.IgnoreProperty(x => x.Foto);

            mapping.HasOne(x => x.Usuario);
            mapping.HasOne(x => x.Categoria);

            mapping.HasMany(x => x.Topicos).KeyColumn("ComunidadeId").Not.LazyLoad();

            mapping.HasMany(x => x.Membros).KeyColumn("ComunidadeId").Not.LazyLoad();

            mapping.HasMany(x => x.Moderadores).KeyColumn("ComunidadeId").Not.LazyLoad();
        }
    }
}