using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IPrivacidadeService : IDisposable
    {
        void IncluirPrivacidade(int usuarioId, int verRecado, int escreverRecado);
        void SalvarPrivacidade(int verRecado, int escreverRecado);
        Privacidade Obter();
    }
}
