using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class RecadoOverride : IAutoMappingOverride<Recado>
    {
        public void Override(AutoMapping<Recado> mapping)
        {
            mapping.Table("Recado");
            mapping.Id(x => x.Id);

            mapping.IgnoreProperty(x => x.RecadoHtml);

            mapping.HasOne(x => x.EnviadoPor);
            mapping.HasOne(x => x.EnviadoPara);

            mapping.HasMany(x => x.RecadosComentario).KeyColumn("RecadoId").Not.LazyLoad();
        }
    }
}
