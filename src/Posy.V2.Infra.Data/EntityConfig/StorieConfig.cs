using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class StorieConfig : EntityTypeConfiguration<Storie>
    {
        public StorieConfig()
        {
            ToTable("Storie");

            HasKey(x => x.StorieId);

            Property(x => x.UsuarioId).IsRequired();
            Property(x => x.StorieType).IsRequired();
            Property(x => x.StorieType).IsRequired();
            Property(x => x.Length).IsRequired();
            Property(x => x.Src).IsRequired();
            Property(x => x.Preview).IsRequired();
            Property(x => x.Time).IsRequired();
            Property(x => x.DataStorie).IsRequired();
        }
    }
}
