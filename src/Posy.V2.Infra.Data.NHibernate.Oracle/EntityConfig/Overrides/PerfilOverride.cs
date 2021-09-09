using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class PerfilOverride : IAutoMappingOverride<Perfil>
    {
        public void Override(AutoMapping<Perfil> mapping)
        {
            mapping.Table("Perfil");

            /**
            * Quando for one-to-one e a tabela perfil tem uma chave primaria Id e chave estrangeira UsuarioId
            **/
            mapping.Id(x => x.Id);

            mapping.IgnoreProperty(x => x.Foto);
            mapping.IgnoreProperty(x => x.PerfilHtml);
            mapping.IgnoreProperty(x => x.FraseHtml);

            //mapping.References(x => x.Usuario).Unique();
            //mapping.HasOne(x => x.Usuario).Constrained().ForeignKey();
            //mapping.HasOne(x => x.Usuario).Constrained();
            //mapping.HasOne(x => x.Usuario).Cascade.All().PropertyRef("Perfil");
            //mapping.HasOne(x => x.Usuario).Constrained().ForeignKey();

            /**
            * Quando for one-to-one e as duas tabelas com a mesma chave primaria UsuarioId
            **/
            mapping.HasOne(x => x.Usuario).Constrained().ForeignKey();
        }
    }
}
