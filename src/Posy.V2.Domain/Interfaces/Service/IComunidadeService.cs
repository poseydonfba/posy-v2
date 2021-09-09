using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IComunidadeService : IDisposable
    {
        Comunidade Obter(Guid comunidadeId);
        Comunidade Obter(string alias);
        IEnumerable<Comunidade> Obter(int paginaAtual, int itensPagina, out int totalRecords);
        Comunidade CriarComunidade(string nome, int categoriaId, string descricaoPerfil);
        Comunidade EditarComunidadePerfil(Guid comunidadeId, string alias, string nome, int categoriaId, string descricaoPerfil);
        Comunidade EditarComunidadePrivacidade(Guid comunidadeId, TipoComunidade tipo);
        void ExcluirComunidade(Guid comunidadeId);
        TopicoPost ObterUltimoPost(Guid comunidadeId);
    }
}
