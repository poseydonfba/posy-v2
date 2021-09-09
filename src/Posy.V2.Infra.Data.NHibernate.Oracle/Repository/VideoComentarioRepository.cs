using NHibernate.Linq;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class VideoComentarioRepository : Repository<VideoComentario, int>, IVideoComentarioRepository
    {
        public VideoComentarioRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<VideoComentario> Get(int videoId, int paginaAtual, int itensPagina)
        {
            if (itensPagina == 0)
                return DbSet
                        .Where(x => x.VideoId == videoId && x.Der == null)
                        .OrderByDescending(x => x.DataComentario)
                        .Fetch(x => x.Usuario)
                        .ThenFetch(x => x.Perfil)
                        .ToList();
            else
                return DbSet
                        .Where(x => x.VideoId == videoId && x.Der == null)
                        .OrderByDescending(x => x.DataComentario)
                        .Skip((paginaAtual - 1) * itensPagina)
                        .Take(itensPagina)
                        .Fetch(x => x.Usuario)
                        .ThenFetch(x => x.Perfil)
                        .ToList();
        }
    }
}