using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class PostPerfilComentarioConfig : EntityTypeConfiguration<PostPerfilComentario>
    {
        public PostPerfilComentarioConfig()
        {
            ToTable("PostPerfilComentario");

            HasKey(x => x.PostPerfilComentarioId);

            Property(x => x.Comentario).IsRequired();

            Ignore(x => x.ComentarioHtml);
        }
    }
}
