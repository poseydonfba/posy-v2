using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class VisitantePerfilConfig : EntityTypeConfiguration<VisitantePerfil>
    {
        public VisitantePerfilConfig()
        {
            ToTable("VisitantePerfil");

            //HasKey(f => new { f.VisitanteId, f.VisitadoId, f.DataVisita });
            HasKey(f => f.VisitantePerfilId);

            HasRequired(a => a.Visitante).WithMany().HasForeignKey(x => x.VisitanteId);
            HasRequired(a => a.Visitado).WithMany().HasForeignKey(x => x.VisitadoId);
        }
    }
}
