using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class DepoimentoOverride : IAutoMappingOverride<Depoimento>
    {
        public void Override(AutoMapping<Depoimento> mapping)
        {
            mapping.Table("Depoimento");
            mapping.Id(x => x.Id);

            mapping.IgnoreProperty(x => x.DepoimentoHtml);

            mapping.HasOne(x => x.EnviadoPor);
            mapping.HasOne(x => x.EnviadoPara);
        }
    }
}