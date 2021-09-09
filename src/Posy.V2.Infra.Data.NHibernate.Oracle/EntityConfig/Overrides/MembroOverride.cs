using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class MembroOverride : IAutoMappingOverride<Membro>
    {
        public void Override(AutoMapping<Membro> mapping)
        {
            mapping.Table("Membro");
            mapping.Id(x => x.Id);

            mapping.HasOne(x => x.Comunidade);

            mapping.HasOne(x => x.UsuarioMembro);
            mapping.HasOne(x => x.UsuarioResposta);
        }
    }
}
