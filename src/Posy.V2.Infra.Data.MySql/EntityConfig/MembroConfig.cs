using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class MembroConfig : EntityTypeConfiguration<Membro>
    {
        public MembroConfig()
        {
            ToTable("Membro");

            //HasKey(k => new { k.ComunidadeId, k.UsuarioMembroId, k.DataSolicitacao });
            HasKey(k => k.MembroId);

            HasRequired(x => x.Comunidade);
            HasRequired(x => x.UsuarioMembro).WithMany().HasForeignKey(x => x.UsuarioMembroId);
            HasOptional(x => x.UsuarioResposta).WithMany().HasForeignKey(x => x.UsuarioRespostaId);
        }
    }
}
