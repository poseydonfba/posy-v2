using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class DepoimentoConfig : EntityTypeConfiguration<Depoimento>
    {
        public DepoimentoConfig()
        {
            ToTable("Depoimento");

            HasKey(x => x.DepoimentoId);

            Property(x => x.DescricaoDepoimento).IsRequired();

            Ignore(x => x.DepoimentoHtml);

            HasRequired(a => a.EnviadoPor).WithMany().HasForeignKey(x => x.EnviadoPorId);
            HasRequired(a => a.EnviadoPara).WithMany().HasForeignKey(x => x.EnviadoParaId);
        }
    }
}
