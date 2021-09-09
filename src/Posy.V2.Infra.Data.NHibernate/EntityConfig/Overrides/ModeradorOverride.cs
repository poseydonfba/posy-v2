using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class ModeradorOverride : IAutoMappingOverride<Moderador>
    {
        public void Override(AutoMapping<Moderador> mapping)
        {
            mapping.Table("Moderador");
            mapping.Id(x => x.Id);

            mapping.HasOne(x => x.Comunidade);

            mapping.HasOne(x => x.UsuarioModerador);
            mapping.HasOne(x => x.UsuarioOperacao);
        }
    }
}