using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class RecadoComentarioConfig : EntityTypeConfiguration<RecadoComentario>
    {
        public RecadoComentarioConfig()
        {
            ToTable("RecadoComentario");

            HasKey(x => x.RecadoComentarioId);

            Property(x => x.DescricaoComentario).IsRequired();

            Ignore(x => x.ComentarioHtml);

            HasRequired(x => x.Usuario);
            HasRequired(x => x.Recado);
        }
    }
}
