using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class VideoOverride : IAutoMappingOverride<Video>
    {
        public void Override(AutoMapping<Video> mapping)
        {
            mapping.Table("Video");
            mapping.Id(x => x.Id);

            mapping.IgnoreProperty(x => x.NomeHtml);

            mapping.HasOne(x => x.Usuario);

            mapping.HasMany(x => x.VideosComentario).KeyColumn("VideoId").Not.LazyLoad();
        }
    }
}
