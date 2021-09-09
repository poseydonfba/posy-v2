using System;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IUsuarioService : IDisposable
    {
        void UpdateCulture(string language);
    }
}
