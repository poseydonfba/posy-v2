using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class TopicoConfig : EntityTypeConfiguration<Topico>
    {
        public TopicoConfig()
        {
            ToTable("Topico");

            HasKey(k => k.TopicoId);

            Property(x => x.Titulo).IsRequired();
            Property(x => x.Descricao).IsRequired();

            Ignore(x => x.DescricaoHtml);

            HasRequired(x => x.Comunidade).WithMany().HasForeignKey(x => x.ComunidadeId);
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId);
        }
    }
}
