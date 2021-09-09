using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class PrivacidadeRepository : Repository<Privacidade, Guid>, IPrivacidadeRepository
    {
        public PrivacidadeRepository(DatabaseContext context) : base(context)
        {

        }

        public Privacidade GetByUsuario(Guid usuarioId)
        {
            return _db.Privacidades.FirstOrDefault(x => x.UsuarioId == usuarioId);
        }
    }
}