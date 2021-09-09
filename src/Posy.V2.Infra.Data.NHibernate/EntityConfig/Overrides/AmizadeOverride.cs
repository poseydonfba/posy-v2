using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class AmizadeOverride : IAutoMappingOverride<Amizade>
    {
        public void Override(AutoMapping<Amizade> mapping)
        {
            mapping.Table("Amizade");
            mapping.Id(x => x.Id).GeneratedBy.Identity();

            mapping.IgnoreProperty(x => x.Aprovado);

            mapping.HasOne(x => x.SolicitadoPor);
            mapping.HasOne(x => x.SolicitadoPara);
        }
    }
}
