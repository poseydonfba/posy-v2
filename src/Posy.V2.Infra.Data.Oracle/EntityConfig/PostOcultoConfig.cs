using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class PostOcultoConfig : EntityTypeConfiguration<PostOculto>
    {
        public PostOcultoConfig()
        {
            ToTable("PostOculto");

            //HasKey(k => new { k.UsuarioId, k.PostPerfilId, k.Data });
            HasKey(k => k.PostOcultoId);

            HasRequired(x => x.Usuario);
            HasRequired(x => x.PostPerfil);
        }
    }
}
