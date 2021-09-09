using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class AmizadeConfig : EntityTypeConfiguration<Amizade>
    {
        public AmizadeConfig()
        {
            ToTable("Amizade");

            //HasKey(f => new { f.SolicitadoPorId, f.SolicitadoParaId, f.DataSolicitacao });
            HasKey(f => f.AmizadeId);

            Ignore(x => x.Aprovado);

            HasRequired(a => a.SolicitadoPor).WithMany().HasForeignKey(c => c.SolicitadoPorId);
            HasRequired(a => a.SolicitadoPara).WithMany().HasForeignKey(c => c.SolicitadoParaId);
        }
    }
}
