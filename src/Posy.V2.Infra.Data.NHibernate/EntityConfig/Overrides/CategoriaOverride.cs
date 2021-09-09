using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class CategoriaOverride : IAutoMappingOverride<Categoria>
    {
        public void Override(AutoMapping<Categoria> mapping)
        {
            mapping.Table("Categoria");
            mapping.Id(x => x.Id);

            mapping.HasMany(x => x.Comunidades).KeyColumn("CategoriaId").Not.LazyLoad();
        }
    }
}

