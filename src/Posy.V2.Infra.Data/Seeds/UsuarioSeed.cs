using Posy.V2.Domain.Entities;
using Posy.V2.Infra.Data.Context;
using System.Data.Entity.Migrations;

namespace Posy.V2.Infra.Data.Seeds
{
    public class UsuarioSeed
    {
        public static void Seed(DatabaseContext context)
        {
            //context.Usuarios.AddOrUpdate(x => x.UsuarioId,
            //                                  new Usuario("", new Cpf(""), new Email("@gmail.com"), "", "", endereco));
        }
    }
}
