using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class PrivacidadeRepository : Repository<Privacidade, int>, IPrivacidadeRepository
    {
        public PrivacidadeRepository(DatabaseContext context) : base(context)
        {

        }

        public Privacidade GetByUsuario(int usuarioId)
        {
            return DbSet.FirstOrDefault(x => x.Id == usuarioId);
        }
    }
}