using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class ModeradorRepository : Repository<Moderador, Guid>, IModeradorRepository
    {
        public ModeradorRepository(DatabaseContext context) : base(context)
        {

        }

        public Moderador Get(Guid comunidadeId, Guid usuarioId)
        {
            var moderador = _db.Moderadores
                    .Include("UsuarioModerador.Perfil")
                    .Include("UsuarioOperacao.Perfil")
                    .Where(x => x.ComunidadeId == comunidadeId && x.UsuarioModeradorId == usuarioId && x.Der == null)
                    .FirstOrDefault();

            return moderador;
        }

        public IEnumerable<Moderador> GetComunidades(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            totalRecords = _db.Moderadores.Where(x => x.UsuarioModeradorId == usuarioId && x.Der == null).Count();

            var moderadores = _db.Moderadores
                            .Include("Comunidade")
                            .Where(x => x.UsuarioModeradorId == usuarioId && x.Der == null)
                            .OrderByDescending(x => x.DataOperacao)
                            .Skip((paginaAtual - 1) * itensPagina)
                            .Take(itensPagina)
                            .ToList();

            return moderadores;
        }

        public IEnumerable<Moderador> GetModeradores(Guid comunidadeId)
        {
            var moderadores = _db.Moderadores
                            .Include("UsuarioModerador.Perfil")
                            .Include("UsuarioOperacao.Perfil")
                            .Where(x => x.ComunidadeId == comunidadeId && x.Der == null)
                            .OrderByDescending(x => x.DataOperacao)
                            .ToList();

            return moderadores;
        }

        public IEnumerable<Moderador> GetModeradores(Guid comunidadeId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            totalRecords = _db.Moderadores.Where(x => x.ComunidadeId == comunidadeId && x.Der == null).Count();

            var moderadores = _db.Moderadores
                            .Include("UsuarioModerador.Perfil")
                            .Include("UsuarioOperacao.Perfil")
                            .Where(x => x.ComunidadeId == comunidadeId && x.Der == null)
                            .OrderByDescending(x => x.DataOperacao)
                            .Skip((paginaAtual - 1) * itensPagina)
                            .Take(itensPagina)
                            .ToList();

            return moderadores;
        }
    }
}