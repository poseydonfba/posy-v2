namespace Posy.V2.Infra.Data.PostgreSql.Migrations
{
    using Posy.V2.Domain.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Posy.V2.Infra.Data.Context.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Posy.V2.Infra.Data.Context.DatabaseContext context)
        {
            context.Categorias.AddOrUpdate(x => x.CategoriaId,
                              new Categoria { CategoriaId = 1, Nome = "Amizade" },
                              new Categoria { CategoriaId = 2, Nome = "Relacionamentos" });
        }
    }
}
