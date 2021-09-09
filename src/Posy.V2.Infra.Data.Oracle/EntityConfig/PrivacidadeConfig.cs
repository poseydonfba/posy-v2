using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class PrivacidadeConfig : EntityTypeConfiguration<Privacidade>
    {
        public PrivacidadeConfig()
        {
            ToTable("Privacidade");

            HasKey(x => x.UsuarioId);
            //HasKey(x => x.PrivacidadeId);

            Property(x => x.VerRecado).IsRequired();
            Property(x => x.EscreverRecado).IsRequired();

            HasRequired(x => x.Usuario);
        }
    }
}
