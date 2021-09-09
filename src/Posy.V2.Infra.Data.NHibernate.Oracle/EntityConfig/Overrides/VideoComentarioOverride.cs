using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class VideoComentarioOverride : IAutoMappingOverride<VideoComentario>
    {
        public void Override(AutoMapping<VideoComentario> mapping)
        {
            mapping.Table("VideoComentario");
            mapping.Id(x => x.Id);

            mapping.IgnoreProperty(x => x.ComentarioHtml);

            mapping.HasOne(x => x.Usuario);
            mapping.HasOne(x => x.Video);
        }
    }
}