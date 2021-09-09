using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class TopicoOverride : IAutoMappingOverride<Topico>
    {
        public void Override(AutoMapping<Topico> mapping)
        {
            mapping.Table("Topico");
            mapping.Id(x => x.Id);

            mapping.IgnoreProperty(x => x.DescricaoHtml);

            mapping.HasOne(x => x.Usuario);
            mapping.HasOne(x => x.Comunidade);

            mapping.HasMany(x => x.TopicosPost).KeyColumn("TopicoId").Not.LazyLoad();
        }
    }
}
