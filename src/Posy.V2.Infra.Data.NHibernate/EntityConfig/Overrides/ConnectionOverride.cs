using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class ConnectionOverride : IAutoMappingOverride<Connection>
    {
        public void Override(AutoMapping<Connection> mapping)
        {
            mapping.Table("Connection");
            mapping.Id(x => x.Id);

            mapping.HasOne(x => x.Usuario);
        }
    }
}