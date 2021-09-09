using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface ITopicoService : IDisposable
    {
        Topico SalvarTopico(Guid comunidadeId, string titulo, string descricao, TipoTopico fixo);
        void ExcluirTopico(Guid topicoId);
        void ExcluirTopicoPermanente(Guid topicoId);
        Topico Obter(Guid topicoId);
        IEnumerable<Topico> ObterTopicos(Guid comunidadeId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
