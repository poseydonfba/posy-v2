using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface ITopicoService : IDisposable
    {
        Topico SalvarTopico(int comunidadeId, string titulo, string descricao, TipoTopico fixo);
        void ExcluirTopico(int topicoId);
        void ExcluirTopicoPermanente(int topicoId);
        Topico Obter(int topicoId);
        IEnumerable<Topico> ObterTopicos(int comunidadeId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
