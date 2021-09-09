namespace Posy.V2.Infra.CrossCutting.Identity.Migrations
{
    using global::MySql.Data.Entity;
    using Posy.V2.Infra.CrossCutting.Common;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Posy.V2.Infra.CrossCutting.Identity.Context.ApplicationDbContext>
    {
        public Configuration()
        {
            ConfigureServerDatabase();
        }

        private void ConfigureServerDatabase()
        {
            if (ConfigurationBase.ServerDatabase == Common.Enums.ServerDatabase.SQLSERVER)
            {
                AutomaticMigrationsEnabled = true;
                MigrationsDirectory = @"Migrations";
            }
            else if (ConfigurationBase.ServerDatabase == Common.Enums.ServerDatabase.MYSQL)
            {
                AutomaticMigrationsEnabled = true;
                //DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
                SetSqlGenerator(MySqlProviderInvariantName.ProviderName, new MySqlMigrationSqlGenerator());
                SetHistoryContextFactory(MySqlProviderInvariantName.ProviderName, (connection, schema) => new MySqlHistoryContext(connection, schema));
            }
            else if (ConfigurationBase.ServerDatabase == Common.Enums.ServerDatabase.POSTGRESQL)
            {
                AutomaticMigrationsEnabled = true;
                MigrationsDirectory = @"Migrations";
            }
            else if (ConfigurationBase.ServerDatabase == Common.Enums.ServerDatabase.ORACLE)
            {
                AutomaticMigrationsEnabled = true;
                MigrationsDirectory = @"Migrations";
            }
        }

        // ERROS
        // Specified key was too long; max key length is 767 bytes
        // https://wildlyinaccurate.com/mysql-specified-key-was-too-long-max-key-length-is-767-bytes/

        protected override void Seed(Posy.V2.Infra.CrossCutting.Identity.Context.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
