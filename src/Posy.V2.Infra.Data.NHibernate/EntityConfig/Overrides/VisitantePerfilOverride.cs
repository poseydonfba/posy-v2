using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class VisitantePerfilOverride : IAutoMappingOverride<VisitantePerfil>
    {
        public void Override(AutoMapping<VisitantePerfil> mapping)
        {
            mapping.Table("VisitantePerfil");
            mapping.Id(x => x.Id);

            mapping.HasOne(x => x.Visitante);
            mapping.HasOne(x => x.Visitado);
        }
    }
}
