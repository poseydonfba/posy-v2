using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class ModeradorRepository : Repository<Moderador, int>, IModeradorRepository
    {
        public ModeradorRepository(DatabaseContext context) : base(context)
        {

        }

        public Moderador Get(int comunidadeId, int usuarioId)
        {
            var moderador = DbSet
                    .Fetch(x => x.UsuarioModerador)
                    .ThenFetch(x => x.Perfil)
                    .Fetch(x => x.UsuarioOperacao)
                    .ThenFetch(x => x.Perfil)
                    .Where(x => x.ComunidadeId == comunidadeId && x.UsuarioModeradorId == usuarioId && x.Der == null)
                    .FirstOrDefault();

            return moderador;
        }

        public IEnumerable<Moderador> GetComunidades(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            totalRecords = DbSet.Where(x => x.UsuarioModeradorId == usuarioId && x.Der == null).Count();

            var moderadores = DbSet
                            .Fetch(x => x.Comunidade)
                            .Where(x => x.UsuarioModeradorId == usuarioId && x.Der == null)
                            .OrderByDescending(x => x.DataOperacao)
                            .Skip((paginaAtual - 1) * itensPagina)
                            .Take(itensPagina)
                            .ToList();

            return moderadores;
        }

        public IEnumerable<Moderador> GetModeradores(int comunidadeId)
        {
            var moderadores = DbSet
                            .Fetch(x => x.UsuarioModerador)
                            .ThenFetch(x => x.Perfil)
                            .Fetch(x => x.UsuarioOperacao)
                            .ThenFetch(x => x.Perfil)
                            .Where(x => x.ComunidadeId == comunidadeId && x.Der == null)
                            .OrderByDescending(x => x.DataOperacao)
                            .ToList();

            return moderadores;
        }

        public IEnumerable<Moderador> GetModeradores(int comunidadeId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            totalRecords = DbSet.Where(x => x.ComunidadeId == comunidadeId && x.Der == null).Count();

            var moderadores = DbSet
                            .Fetch(x => x.UsuarioModerador)
                            .ThenFetch(x => x.Perfil)
                            .Fetch(x => x.UsuarioOperacao)
                            .ThenFetch(x => x.Perfil)
                            .Where(x => x.ComunidadeId == comunidadeId && x.Der == null)
                            .OrderByDescending(x => x.DataOperacao)
                            .Skip((paginaAtual - 1) * itensPagina)
                            .Take(itensPagina)
                            .ToList();

            return moderadores;
        }
    }
}