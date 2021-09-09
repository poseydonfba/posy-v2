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
    public class AmizadeRepository : Repository<Amizade, int>, IAmizadeRepository
    {
        public AmizadeRepository(DatabaseContext context) : base(context)
        {

        }

        public Amizade Get(int usuarioId1, int usuarioId2)
        {
            var amigosSolicitados = (from t in _db.Session.Query<Amizade>()
                                     where t.SolicitadoPorId == usuarioId1
                                     && t.SolicitadoParaId == usuarioId2
                                     && t.Der == null
                                     select t);

            var amizade = amigosSolicitados
                    .Fetch(x => x.SolicitadoPor)
                    .ThenFetch(x => x.Perfil)
                    .Fetch(x => x.SolicitadoPara)
                    .ThenFetch(x => x.Perfil)
                    .OrderByDescending(x => x.DataSolicitacao)
                    .FirstOrDefault();

            if (amizade == null)
            {
                var amigosSolicitantes = (from t in _db.Session.Query<Amizade>()
                                          where t.SolicitadoPorId == usuarioId2
                                          && t.SolicitadoParaId == usuarioId1
                                          && t.Der == null
                                          select t);

                amizade = amigosSolicitantes
                    .Fetch(x => x.SolicitadoPor)
                    .ThenFetch(x => x.Perfil)
                    .Fetch(x => x.SolicitadoPara)
                    .ThenFetch(x => x.Perfil)
                    .OrderByDescending(x => x.DataSolicitacao)
                    .FirstOrDefault();
            }

            return amizade;
        }

        public Amizade GetSolicitacao(int usuarioIdSolicitante, int usuarioIdSolicitado)
        {
            var amizadeSolicitacao = (from t in _db.Session.Query<Amizade>()
                                      where t.SolicitadoPorId == usuarioIdSolicitante
                                      && t.SolicitadoParaId == usuarioIdSolicitado
                                      && t.Der == null
                                      select t);

            var amizade = amizadeSolicitacao
                .OrderByDescending(x => x.DataSolicitacao)
                .FirstOrDefault();

            return amizade;
        }
        
        /// <summary>
        /// Retorna todas as solicitações recebidas de um usuario
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="flag"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Amizade> Get(int usuarioId, int paginaAtual, int itensPagina, SolicitacaoAmizade flag, out int totalRecords)
        {
            if (flag == SolicitacaoAmizade.Todos)
            {
                totalRecords = _db.Session.Query<Amizade>().Where(x => x.SolicitadoParaId == usuarioId).Count();

                var amizades = _db.Session.Query<Amizade>()
                                .Fetch(x => x.SolicitadoPor)
                                .ThenFetch(x => x.Perfil)
                                .Fetch(x => x.SolicitadoPara)
                                .ThenFetch(x => x.Perfil)
                                .Where(x => x.SolicitadoParaId == usuarioId)
                                .OrderByDescending(x => x.DataSolicitacao)
                                .Skip((paginaAtual - 1) * itensPagina)
                                .Take(itensPagina)
                                .ToList();

                return amizades;
            }
            else
            {
                totalRecords = _db.Session.Query<Amizade>().Where(x => x.SolicitadoParaId == usuarioId && x.StatusSolicitacao == flag).Count();

                var amizades = _db.Session.Query<Amizade>()
                                .Fetch(x => x.SolicitadoPor)
                                .ThenFetch(x => x.Perfil)
                                .Fetch(x => x.SolicitadoPara)
                                .ThenFetch(x => x.Perfil)
                                .Where(x => x.SolicitadoParaId == usuarioId && x.StatusSolicitacao == flag)
                                .OrderByDescending(x => x.DataSolicitacao)
                                .Skip((paginaAtual - 1) * itensPagina)
                                .Take(itensPagina)
                                .ToList();

                return amizades;
            }
        }

        /// <summary>
        /// Retorna todos os amigos de um usuario
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <returns></returns>
        public IEnumerable<Usuario> GetAmigos(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            var amigosSolicitados = (from t in _db.Session.Query<Amizade>()
                                     where t.SolicitadoPorId == usuarioId
                                     && t.StatusSolicitacao == SolicitacaoAmizade.Aprovado
                                     && t.Der == null
                                     select t.SolicitadoPara);

            var amigosSolicitantes = (from t in _db.Session.Query<Amizade>()
                                      where t.SolicitadoParaId == usuarioId
                                      && t.StatusSolicitacao == SolicitacaoAmizade.Aprovado
                                      && t.Der == null
                                      select t.SolicitadoPor);

            totalRecords = amigosSolicitados.Concat(amigosSolicitantes).Count();

            var amigos = amigosSolicitados.Concat(amigosSolicitantes)
                                        .Fetch(x => x.Perfil)
                                        .OrderByDescending(x => x.Dir)
                                        .Skip((paginaAtual - 1) * itensPagina)
                                        .Take(itensPagina)
                                        .ToList();

            return amigos;
        }

    }
}
