using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class PrivacidadeOverride : IAutoMappingOverride<Privacidade>
    {
        public void Override(AutoMapping<Privacidade> mapping)
        {
            mapping.Table("Privacidade");
            mapping.Id(x => x.Id);

            /**
            * Quando for one-to-one e as duas tabelas com a mesma chave primaria UsuarioId
            **/
            mapping.HasOne(x => x.Usuario).Constrained().ForeignKey();
        }
    }
}