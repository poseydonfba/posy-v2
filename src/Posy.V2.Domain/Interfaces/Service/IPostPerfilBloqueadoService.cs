using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IPostPerfilBloqueadoService : IDisposable
    {
        void BloquearPostPerfil(Guid usuarioIdBloquear);
        PostPerfilBloqueado ObterPerfilBloqueado(Guid usuarioId, Guid usuarioIdBloqueado);
    }
}
