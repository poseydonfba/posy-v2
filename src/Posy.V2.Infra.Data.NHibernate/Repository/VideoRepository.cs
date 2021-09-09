using NHibernate.Linq;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class VideoRepository : Repository<Video, int>, IVideoRepository
    {
        public VideoRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<Video> Get(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            var videos = DbSet.Fetch(x => x.Usuario).ThenFetch(x => x.Perfil).Where(x => x.UsuarioId == usuarioId && x.Der == null);

            totalRecords = videos.Count();

            return videos
                    .OrderByDescending(x => x.DataVideo)
                    .Skip((paginaAtual - 1) * itensPagina)
                    .Take(itensPagina)
                    .ToList();
        }
    }
}