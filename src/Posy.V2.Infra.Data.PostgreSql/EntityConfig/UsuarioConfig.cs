using System.Data.Entity.ModelConfiguration;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class UsuarioConfig : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfig()
        {
            HasKey(u => u.UsuarioId);

            //Property(u => u.UsuarioId)
            //    .IsRequired()
            //    .HasColumnType("nvarchar")
            //    .HasMaxLength(128);

            Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256);

            HasRequired(x => x.Perfil).WithRequiredPrincipal(y => y.Usuario);
            HasRequired(x => x.Privacidade).WithRequiredPrincipal(y => y.Usuario);//.Map(x => x.MapKey("UsuarioId"));

            ToTable("Usuario");
        }
    }
}