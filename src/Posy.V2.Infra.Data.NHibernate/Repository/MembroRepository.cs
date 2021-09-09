using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class MembroRepository : Repository<Membro, int>, IMembroRepository
    {
        public MembroRepository(DatabaseContext context) : base(context)
        {

        }

        public Membro Get(int comunidadeId, int usuarioId)
        {
            var membro = DbSet
                    .Fetch(x => x.UsuarioMembro)
                    .ThenFetch(x => x.Perfil)
                    .Fetch(x => x.UsuarioResposta)
                    .ThenFetch(x => x.Perfil)
                    .Where(x => x.ComunidadeId == comunidadeId && x.UsuarioMembroId == usuarioId && x.Der == null)
                    .OrderByDescending(x => x.DataSolicitacao)
                    .FirstOrDefault();

            return membro;
        }

        public IEnumerable<Membro> GetMembros(int comunidadeId, int paginaAtual, int itensPagina, StatusSolicitacaoMembroComunidade flag, out int totalRecords)
        {
            if (flag == StatusSolicitacaoMembroComunidade.Todos)
            {
                totalRecords = DbSet.Where(x => x.ComunidadeId == comunidadeId && x.Der == null).Count();

                var membros = DbSet
                                .Fetch(x => x.UsuarioMembro)
                                .ThenFetch(x => x.Perfil)
                                .Fetch(x => x.UsuarioResposta)
                                .ThenFetch(x => x.Perfil)
                                .Where(x => x.ComunidadeId == comunidadeId && x.Der == null)
                                .OrderByDescending(x => x.DataSolicitacao)
                                .Skip((paginaAtual - 1) * itensPagina)
                                .Take(itensPagina)
                                .ToList();

                return membros;
            }
            else
            {
                totalRecords = DbSet.Where(x => x.ComunidadeId == comunidadeId && x.StatusSolicitacao == flag && x.Der == null).Count();

                var membros = DbSet
                                .Fetch(x => x.UsuarioMembro)
                                .ThenFetch(x => x.Perfil)
                                .Fetch(x => x.UsuarioResposta)
                                .ThenFetch(x => x.Perfil)
                                .Where(x => x.ComunidadeId == comunidadeId && x.StatusSolicitacao == flag && x.Der == null)
                                .OrderByDescending(x => x.DataSolicitacao)
                                .Skip((paginaAtual - 1) * itensPagina)
                                .Take(itensPagina)
                                .ToList();

                return membros;
            }
        }

        public IEnumerable<Membro> GetComunidades(int usuarioId, int paginaAtual, int itensPagina, StatusSolicitacaoMembroComunidade flag, out int totalRecords)
        {
            if (flag == StatusSolicitacaoMembroComunidade.Todos)
            {
                totalRecords = DbSet/* .AsNoTracking() */.Where(x => x.UsuarioMembroId == usuarioId && x.Der == null).Count();

                var membros = DbSet/* .AsNoTracking() */
                                .Fetch(x => x.Comunidade)
                                .ThenFetch(x => x.Usuario)
                                .ThenFetch(x => x.Perfil)
                                .Where(x => x.UsuarioMembroId == usuarioId && x.Der == null)
                                .OrderByDescending(x => x.DataSolicitacao)
                                .Skip((paginaAtual - 1) * itensPagina)
                                .Take(itensPagina)
                                .ToList();

                return membros;
            }
            else
            {
                totalRecords = DbSet/* .AsNoTracking() */.Where(x => x.UsuarioMembroId == usuarioId && x.StatusSolicitacao == flag && x.Der == null).Count();

                var membros = DbSet/* .AsNoTracking() */
                                .Fetch(x => x.Comunidade)
                                .ThenFetch(x => x.Usuario)
                                .ThenFetch(x => x.Perfil)
                                .Where(x => x.UsuarioMembroId == usuarioId && x.StatusSolicitacao == flag && x.Der == null)
                                .OrderByDescending(x => x.DataSolicitacao)
                                .Skip((paginaAtual - 1) * itensPagina)
                                .Take(itensPagina)
                                .ToList();

                return membros;
            }
        }
    }
}