using Posy.V2.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class ConnectionConfig : EntityTypeConfiguration<Connection>
    {
        public ConnectionConfig()
        {
            ToTable("Connection");

            HasKey(k => k.ConnectionId);

            Property(x => x.UserAgent).HasMaxLength(250).IsRequired();

            HasRequired(x => x.Usuario);
        }
    }
}
