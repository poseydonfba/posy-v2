using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class TopicoPostOverride : IAutoMappingOverride<TopicoPost>
    {
        public void Override(AutoMapping<TopicoPost> mapping)
        {
            mapping.Table("TopicoPost");
            mapping.Id(x => x.Id);

            mapping.IgnoreProperty(x => x.DescricaoHtml);

            mapping.HasOne(x => x.Usuario);
            mapping.HasOne(x => x.Topico);
        }
    }
}