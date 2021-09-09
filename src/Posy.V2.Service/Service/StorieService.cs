using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;

namespace Posy.V2.Service
{
    public class StorieService : IStorieService
    {
        private readonly IStorieRepository _storieRepository;

        public StorieService(IStorieRepository storieRepository)
        {
            _storieRepository = storieRepository;
        }

        public IEnumerable<Storie> ObterStories(Guid usuarioId, int paginaAtual, int itensPagina)
        {
            //return _storieRepository.Get(filter => filter.UsuarioId == usuarioId, null, include => include.Usuario.Perfil);
            //return _storieRepository.Get(null, null, include => include.Usuario.Perfil);
            var stories = _storieRepository.ObterStories(usuarioId);

            return stories;

            //var results = stories.GroupBy(p => p.Usuario.Perfil)
            //                .Select(group => new Usuario
            //                {
            //                    Perfil = group.Key,
            //                    Stories = group.ToList()
            //                })
            //                .ToList();

            //var results = from e in stories
            //              group new { e } by e.Usuario.Perfil into g
            //              orderby g.Key
            //              select new Usuario
            //              {
            //                  Perfil = g.Key,
            //                  Stories = g.Select(c => c.e.).Distinct()
            //              };

            //return results;
        }

        public void Dispose()
        {
            _storieRepository.Dispose();
        }
    }
}
