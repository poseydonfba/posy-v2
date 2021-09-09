namespace Posy.V2.Infra.Data.MySql.Migrations
{
    using global::MySql.Data.Entity;
    using Posy.V2.Domain.Entities;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Posy.V2.Infra.Data.Context.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
            SetSqlGenerator(MySqlProviderInvariantName.ProviderName, new MySqlMigrationSqlGenerator());
            SetHistoryContextFactory(MySqlProviderInvariantName.ProviderName, (connection, schema) => new MySqlHistoryContext(connection, schema));
        }

        protected override void Seed(Posy.V2.Infra.Data.Context.DatabaseContext context)
        {
            context.Categorias.AddOrUpdate(x => x.CategoriaId,
                              new Categoria { CategoriaId = 1, Nome = "Amizade" },
                              new Categoria { CategoriaId = 2, Nome = "Relacionamentos" });
        }
    }
}
