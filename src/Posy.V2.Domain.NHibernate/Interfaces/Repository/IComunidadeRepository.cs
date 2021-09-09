using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IComunidadeRepository : IRepository<Comunidade, int>
    {
        //Comunidade Get(string alias);
        Comunidade GetById(int comunidadeId);
        Comunidade GetByAlias(string alias);
        IEnumerable<Comunidade> Get(int paginaAtual, int itensPagina, out int totalRecords);
        TopicoPost GetUltimoPost(int comunidadeId);
    }
}
