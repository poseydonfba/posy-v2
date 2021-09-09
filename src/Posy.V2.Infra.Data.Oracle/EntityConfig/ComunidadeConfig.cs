using Posy.V2.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class ComunidadeConfig : EntityTypeConfiguration<Comunidade>
    {
        public ComunidadeConfig()
        {
            ToTable("Comunidade");

            HasKey(x => x.ComunidadeId);

            Property(x => x.Alias)
                .HasMaxLength(36)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_ALIAS_CMM", 1) { IsUnique = true }))
                .IsRequired();

            Property(x => x.Nome).IsRequired();
            Property(x => x.DescricaoPerfil);

            Ignore(x => x.PerfilHtml);
            Ignore(x => x.Foto);

            HasRequired(x => x.Usuario);
            HasRequired(x => x.Categoria);
        }
    }
}
