using System;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IPostOcultoService : IDisposable
    {
        void OcultarPost(int postPerfilId);

        //void MostrarPost(Guid usuarioId, Guid postPerfilId);
    }
}
