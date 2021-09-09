using NHibernate.Linq;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class StorieRepository : Repository<Storie, int>, IStorieRepository
    {
        public StorieRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<Storie> ObterStories(int usuarioId)
        {
            var amigosSolicitados = (from t in Session.Query<Amizade>() /*.AsNoTracking()*/
                                     where t.SolicitadoPorId == usuarioId
                                     && t.StatusSolicitacao == SolicitacaoAmizade.Aprovado
                                     && t.Der == null
                                     select t.SolicitadoPara.Perfil);

            var amigosSolicitantes = (from t in Session.Query<Amizade>() /*.AsNoTracking()*/
                                      where t.SolicitadoParaId == usuarioId
                                      && t.StatusSolicitacao == SolicitacaoAmizade.Aprovado
                                      && t.Der == null
                                      select t.SolicitadoPor.Perfil);

            var amigos = amigosSolicitados.Concat(amigosSolicitantes);

            var stories = DbSet/*.AsNoTracking()*/.Fetch(x => x.Usuario.Perfil).Where(x => amigos.Any(y => y.Id == x.UsuarioId));

            return stories.ToList();
        }
    }
}
