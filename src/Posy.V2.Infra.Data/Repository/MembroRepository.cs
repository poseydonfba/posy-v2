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
    public class MembroRepository : Repository<Membro, Guid>, IMembroRepository
    {
        public MembroRepository(DatabaseContext context) : base(context)
        {

        }

        public Membro Get(Guid comunidadeId, Guid usuarioId)
        {
            var membro = _db.Membros
                    .Include("UsuarioMembro.Perfil")
                    .Include("UsuarioResposta.Perfil")
                    .Where(x => x.ComunidadeId == comunidadeId && x.UsuarioMembroId == usuarioId && x.Der == null)
                    .OrderByDescending(x => x.DataSolicitacao)
                    .FirstOrDefault();

            return membro;
        }

        public IEnumerable<Membro> GetMembros(Guid comunidadeId, int paginaAtual, int itensPagina, StatusSolicitacaoMembroComunidade flag, out int totalRecords)
        {
            if (flag == StatusSolicitacaoMembroComunidade.Todos)
            {
                totalRecords = _db.Membros.Where(x => x.ComunidadeId == comunidadeId && x.Der == null).Count();

                var membros = _db.Membros
                                .Include("UsuarioMembro.Perfil")
                                .Include("UsuarioResposta.Perfil")
                                .Where(x => x.ComunidadeId == comunidadeId && x.Der == null)
                                .OrderByDescending(x => x.DataSolicitacao)
                                .Skip((paginaAtual - 1) * itensPagina)
                                .Take(itensPagina)
                                .ToList();

                return membros;
            }
            else
            {
                totalRecords = _db.Membros.Where(x => x.ComunidadeId == comunidadeId && x.StatusSolicitacao == flag && x.Der == null).Count();

                var membros = _db.Membros
                                .Include("UsuarioMembro.Perfil")
                                .Include("UsuarioResposta.Perfil")
                                .Where(x => x.ComunidadeId == comunidadeId && x.StatusSolicitacao == flag && x.Der == null)
                                .OrderByDescending(x => x.DataSolicitacao)
                                .Skip((paginaAtual - 1) * itensPagina)
                                .Take(itensPagina)
                                .ToList();

                return membros;
            }
        }

        public IEnumerable<Membro> GetComunidades(Guid usuarioId, int paginaAtual, int itensPagina, StatusSolicitacaoMembroComunidade flag, out int totalRecords)
        {
            if (flag == StatusSolicitacaoMembroComunidade.Todos)
            {
                totalRecords = _db.Membros.AsNoTracking().Where(x => x.UsuarioMembroId == usuarioId && x.Der == null).Count();

                var membros = _db.Membros.AsNoTracking()
                                .Include("Comunidade")
                                .Include("Comunidade.Usuario.Perfil")
                                .Where(x => x.UsuarioMembroId == usuarioId && x.Der == null)
                                .OrderByDescending(x => x.DataSolicitacao)
                                .Skip((paginaAtual - 1) * itensPagina)
                                .Take(itensPagina)
                                .ToList();

                return membros;
            }
            else
            {
                totalRecords = _db.Membros.AsNoTracking().Where(x => x.UsuarioMembroId == usuarioId && x.StatusSolicitacao == flag && x.Der == null).Count();

                var membros = _db.Membros.AsNoTracking()
                                .Include("Comunidade")
                                .Include("Comunidade.Usuario.Perfil")
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