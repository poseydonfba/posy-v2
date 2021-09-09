using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class CategoriaConfig : EntityTypeConfiguration<Categoria>
    {
        public CategoriaConfig()
        {
            ToTable("Categoria");

            HasKey(x => x.CategoriaId);

            Property(x => x.Nome).IsRequired();
        }
    }
}
