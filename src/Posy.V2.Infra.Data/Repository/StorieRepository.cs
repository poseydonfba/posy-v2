using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class StorieRepository : Repository<Storie, Guid>, IStorieRepository
    {
        public StorieRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<Storie> ObterStories(Guid usuarioId)
        {
            var amigosSolicitados = (from t in _db.Amizades.AsNoTracking()
                                     where t.SolicitadoPorId == usuarioId
                                     && t.StatusSolicitacao == SolicitacaoAmizade.Aprovado
                                     && t.Der == null
                                     select t.SolicitadoPara.Perfil);

            var amigosSolicitantes = (from t in _db.Amizades.AsNoTracking()
                                      where t.SolicitadoParaId == usuarioId
                                      && t.StatusSolicitacao == SolicitacaoAmizade.Aprovado
                                      && t.Der == null
                                      select t.SolicitadoPor.Perfil);

            var amigos = amigosSolicitados.Concat(amigosSolicitantes);

            var stories = DbSet.AsNoTracking().Include(x => x.Usuario.Perfil).Where(x => amigos.Any(y => y.UsuarioId == x.UsuarioId));

            return stories.ToList();
        }
    }
}
