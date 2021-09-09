using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System.Linq;
using System.Collections.Generic;
using System;
using Posy.V2.Domain.Enums;
using System.Data.Entity;
using Posy.V2.Infra.CrossCutting.Common;

namespace Posy.V2.Infra.Data.Repository
{
    public class ConnectionRepository : Repository<Connection, string>, IConnectionRepository
    {
        public ConnectionRepository(DatabaseContext context) : base(context)
        {

        }

        public void ExcluirOldConnections(Guid usuarioId)
        {
            foreach (var connection in DbSet.Where(x => x.UsuarioId == usuarioId).ToList())
            {
                connection.Connected = false;
                connection.DataDisconnected = ConfigurationBase.DataAtual;
                connection.TipoDesconexao = TipoDesconexao.MANUAL;
            }
        }

        public IEnumerable<Connection> ObterAmigosConectados(Guid usuarioId)
        {
            //var usuarios = _db.Connections.AsNoTracking()

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

            var amigosConectados = DbSet.AsNoTracking().Include(x => x.Usuario.Perfil).Where(x => amigos.Any(y => y.UsuarioId == x.UsuarioId) && x.Connected == true);
            //var amigosConectados = amigos.Where(x => DbSet.AsNoTracking().Any(y => y.UsuarioId == x.UsuarioId && y.Connected == true));

            return amigosConectados.ToList();

            //return DbSet.Include("Usuario.Perfil").AsNoTracking().Where(x => x.UsuarioId == usuarioId && x.Connected == true);
        }
    }
}
