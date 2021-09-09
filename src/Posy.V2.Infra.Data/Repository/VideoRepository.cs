using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class VideoRepository : Repository<Video, Guid>, IVideoRepository
    {
        public VideoRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<Video> Get(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            var videos = _db.Videos.Include("Usuario").Include("Usuario.Perfil").Where(x => x.UsuarioId == usuarioId && x.Der == null);

            totalRecords = videos.Count();

            return videos
                    .OrderByDescending(x => x.DataVideo)
                    .Skip((paginaAtual - 1) * itensPagina)
                    .Take(itensPagina)
                    .ToList();
        }
    }
}