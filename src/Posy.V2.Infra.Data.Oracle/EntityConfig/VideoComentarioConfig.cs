using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class VideoComentarioConfig : EntityTypeConfiguration<VideoComentario>
    {
        public VideoComentarioConfig()
        {
            ToTable("VideoComentario");

            HasKey(x => x.VideoComentarioId);

            Property(x => x.DescricaoComentario).IsRequired();

            Ignore(x => x.ComentarioHtml);

            HasRequired(x => x.Usuario);
            HasRequired(x => x.Video);
        }
    }
}
