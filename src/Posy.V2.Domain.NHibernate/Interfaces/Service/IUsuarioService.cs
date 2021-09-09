using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IUsuarioService : IDisposable
    {
        void UpdateCulture(string language);
        Usuario SaveOrUpdate(Usuario usuario);
        Usuario GetUsuario(int id);
    }
}
