using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using System;
using System.Collections.Generic;

namespace Posy.V2.Service
{
    public class ComunidadeService : IComunidadeService
    {
        private IComunidadeRepository _repository;
        private IModeradorService _moderadorService;
        private ICurrentUser _currentUser;

        public ComunidadeService(IComunidadeRepository repository,
                                 IModeradorService moderadorService,
                                 ICurrentUser currentUser)
        {
            _repository = repository;
            _moderadorService = moderadorService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="categoriaId"></param>
        /// <param name="descricaoPerfil"></param>
        /// <returns></returns>
        public Comunidade CriarComunidade(string nome, int categoriaId, string descricaoPerfil)
        {
            var comunidade = new Comunidade(_currentUser.GetCurrentUserId(), nome, categoriaId, descricaoPerfil);

            _repository.Insert(comunidade);

            return comunidade;
        }

        /// <summary>
        /// 1 - Se existir
        /// 2 - Se a comunidade for do usuario logado
        /// 3 - Se for moderador
        /// </summary>
        /// <param name="comunidadeId"></param>
        /// <param name="alias"></param>
        /// <param name="nome"></param>
        /// <param name="categoriaId"></param>
        /// <param name="descricaoPerfil"></param>
        /// <returns></returns>
        public Comunidade EditarComunidadePerfil(Guid comunidadeId, string alias, string nome, int categoriaId, string descricaoPerfil)
        {
            var comunidade = Obter(comunidadeId);
            if (comunidade == null)
                throw new Exception(Errors.ComunidadeInvalida);

            var moderador = _moderadorService.Obter(comunidadeId, _currentUser.GetCurrentUserId());

            if (comunidade.UsuarioId != _currentUser.GetCurrentUserId() &&
                moderador == null)
                throw new Exception(Errors.ComunidadeInvalida);

            comunidade.Edit(alias, nome, categoriaId, descricaoPerfil, _currentUser.GetCurrentUserId());

            _repository.Update(comunidade);

            return comunidade;
        }

        /// <summary>
        /// 1 - Se existir
        /// 2 - Se a comunidade for do usuario logado
        /// 3 - Se for moderador
        /// </summary>
        /// <param name="comunidadeId"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public Comunidade EditarComunidadePrivacidade(Guid comunidadeId, TipoComunidade tipo)
        {
            var comunidade = Obter(comunidadeId);
            if (comunidade == null)
                throw new Exception(Errors.ComunidadeInvalida);

            var moderador = _moderadorService.Obter(comunidadeId, _currentUser.GetCurrentUserId());

            if (comunidade.UsuarioId != _currentUser.GetCurrentUserId() &&
                moderador == null)
                throw new Exception(Errors.ComunidadeInvalida);

            comunidade.SetPrivacidade(tipo, _currentUser.GetCurrentUserId());

            _repository.Update(comunidade);

            return comunidade;
        }

        /// <summary>
        /// 1 - Se existir
        /// 2 - Se a comunidade for do usuario logado
        /// 3 - Se for moderador
        /// </summary>
        /// <param name="comunidadeId"></param>
        public void ExcluirComunidade(Guid comunidadeId)
        {
            var comunidade = Obter(comunidadeId);
            if (comunidade == null)
                throw new Exception(Errors.ComunidadeInvalida);

            var moderador = _moderadorService.Obter(comunidadeId, _currentUser.GetCurrentUserId());

            if (comunidade.UsuarioId != _currentUser.GetCurrentUserId() &&
                moderador == null)
                throw new Exception(Errors.ComunidadeInvalida);

            comunidade.Delete(_currentUser.GetCurrentUserId());

            _repository.Remove(comunidade);
        }

        public Comunidade Obter(string alias)
        {
            return _repository.GetByAlias(alias);
        }

        public Comunidade Obter(Guid comunidadeId)
        {
            return _repository.GetById(comunidadeId);
        }

        public IEnumerable<Comunidade> Obter(int paginaAtual, int itensPagina, out int totalRecords)
        {
            return _repository.Get(paginaAtual, itensPagina, out totalRecords);
        }

        public TopicoPost ObterUltimoPost(Guid comunidadeId)
        {
            return _repository.GetUltimoPost(comunidadeId);
        }

        public void Dispose()
        {
            _repository.Dispose();
            _moderadorService.Dispose();
        }
    }
}
