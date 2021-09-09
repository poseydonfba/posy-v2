using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class RecadoComentarioOverride : IAutoMappingOverride<RecadoComentario>
    {
        public void Override(AutoMapping<RecadoComentario> mapping)
        {
            mapping.Table("RecadoComentario");
            mapping.Id(x => x.Id);

            mapping.IgnoreProperty(x => x.ComentarioHtml);

            mapping.HasOne(x => x.Usuario);
            mapping.HasOne(x => x.Recado);
        }
    }
}