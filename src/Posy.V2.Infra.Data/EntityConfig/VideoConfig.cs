using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class VideoConfig : EntityTypeConfiguration<Video>
    {
        public VideoConfig()
        {
            ToTable("Video");

            HasKey(x => x.VideoId);

            Property(x => x.NomeVideo).IsRequired();
            Property(x => x.Url).IsRequired();

            Ignore(x => x.NomeHtml);

            HasRequired(x => x.Usuario);
        }
    }
}
