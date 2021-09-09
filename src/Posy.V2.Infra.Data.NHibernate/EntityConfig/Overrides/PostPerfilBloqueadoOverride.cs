using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class PostPerfilBloqueadoOverride : IAutoMappingOverride<PostPerfilBloqueado>
    {
        public void Override(AutoMapping<PostPerfilBloqueado> mapping)
        {
            mapping.Table("PostPerfilBloqueado");
            mapping.Id(x => x.Id);

            mapping.HasOne(x => x.Usuario);
        }
    }
}