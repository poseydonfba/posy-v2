using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class PostPerfilBloqueadoConfig : EntityTypeConfiguration<PostPerfilBloqueado>
    {
        public PostPerfilBloqueadoConfig()
        {
            ToTable("PostPerfilBloqueado");

            //HasKey(f => new { f.UsuarioId, f.UsuarioIdBloqueado });
            HasKey(f => f.PostPerfilBloqueadoId);

            HasRequired(x => x.Usuario);
        }
    }
}
