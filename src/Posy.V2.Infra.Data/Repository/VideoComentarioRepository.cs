using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class VideoComentarioRepository : Repository<VideoComentario, Guid>, IVideoComentarioRepository
    {
        public VideoComentarioRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<VideoComentario> Get(Guid videoId, int paginaAtual, int itensPagina)
        {
            if (itensPagina == 0)
                return _db.VideoComentarios
                        .Where(x => x.VideoId == videoId && x.Der == null)
                        .OrderByDescending(x => x.DataComentario)
                        .Include("Usuario")
                        .Include("Usuario.Perfil")
                        .ToList();
            else
                return _db.VideoComentarios
                        .Where(x => x.VideoId == videoId && x.Der == null)
                        .OrderByDescending(x => x.DataComentario)
                        .Skip((paginaAtual - 1) * itensPagina)
                        .Take(itensPagina)
                        .Include("Usuario")
                        .Include("Usuario.Perfil")
                        .ToList();
        }
    }
}