using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class ModeradorConfig : EntityTypeConfiguration<Moderador>
    {
        public ModeradorConfig()
        {
            ToTable("Moderador");

            //HasKey(k => new { k.ComunidadeId, k.UsuarioModeradorId, k.DataOperacao });
            HasKey(k => k.ModeradorId);

            HasRequired(x => x.Comunidade);
            HasRequired(x => x.UsuarioModerador).WithMany().HasForeignKey(x => x.UsuarioModeradorId);
            HasRequired(x => x.UsuarioOperacao).WithMany().HasForeignKey(x => x.UsuarioOperacaoId);
        }
    }
}
