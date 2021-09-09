using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class StorieOverride : IAutoMappingOverride<Storie>
    {
        public void Override(AutoMapping<Storie> mapping)
        {
            mapping.Table("Storie");
            mapping.Id(x => x.Id);

            mapping.HasOne(x => x.Usuario);
        }
    }
}