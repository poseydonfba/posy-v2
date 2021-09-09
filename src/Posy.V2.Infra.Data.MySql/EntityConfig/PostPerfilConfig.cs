using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class PostPerfilConfig : EntityTypeConfiguration<PostPerfil>
    {
        public PostPerfilConfig()
        {
            ToTable("PostPerfil");

            HasKey(x => x.PostPerfilId);

            Property(x => x.DescricaoPost).IsRequired();

            Ignore(x => x.PostHtml);
            Ignore(x => x.IsOculto);

            HasRequired(x => x.Usuario);
        }
    }
}
