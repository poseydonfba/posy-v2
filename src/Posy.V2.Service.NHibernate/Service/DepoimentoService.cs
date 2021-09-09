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
    public class DepoimentoService : IDepoimentoService
    {
        private IDepoimentoRepository _repository;
        private IAmizadeService _amizadeService;
        private ICurrentUser _currentUser;

        public DepoimentoService(IDepoimentoRepository repository,
                                 IAmizadeService amizadeService,
                                 ICurrentUser currentUser)
        {
            _repository = repository;
            _amizadeService = amizadeService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enviadoParaId"></param>
        /// <param name="depoimento"></param>
        /// <returns></returns>
        public Depoimento EnviarDepoimento(int enviadoParaId, string depoimento)
        {
            var dep = new Depoimento(_currentUser.GetCurrentUserId(), enviadoParaId, depoimento);

            _repository.Insert(dep);

            return dep;
        }

        /// <summary>
        /// 1 - Se for um depoimento do usuario logado
        /// </summary>
        /// <param name="depoimentoId"></param>
        public void ExcluirDepoimento(int depoimentoId)
        {
            var recado = _repository.Get(depoimentoId);
            if (recado == null)
                throw new Exception(Errors.DepoimentoNaoEncontrado);

            recado.Delete(_currentUser.GetCurrentUserId());

            _repository.Remove(recado);
        }

        /// <summary>
        /// 1 - Se for um depoimento enviado para usuario logado
        /// 2 - Se for um depoimento com status diferente de Aceito
        /// </summary>
        /// <param name="depoimentoId"></param>
        public void AceitarDepoimento(int depoimentoId)
        {
            var depoimento = ObterDepoimento(depoimentoId);
            if (depoimento.EnviadoParaId != _currentUser.GetCurrentUserId() ||
                depoimento.StatusDepoimento == StatusDepoimento.Aceito)
                throw new Exception(Errors.DepoimentoNaoEncontrado);

            depoimento.SetResposta(StatusDepoimento.Aceito);

            _repository.Update(depoimento);
        }

        /// <summary>
        /// 1 - Se for um depoimento enviado para usuario logado
        /// 2 - Se for um depoimento com status diferente de NaoAceito
        /// </summary>
        /// <param name="depoimentoId"></param>
        public void RecusarDepoimento(int depoimentoId)
        {
            var depoimento = ObterDepoimento(depoimentoId);
            if (depoimento.EnviadoParaId != _currentUser.GetCurrentUserId() ||
                depoimento.StatusDepoimento == StatusDepoimento.NaoAceito)
                throw new Exception(Errors.DepoimentoNaoEncontrado);

            depoimento.SetResposta(StatusDepoimento.NaoAceito);

            _repository.Update(depoimento);
        }

        public Depoimento ObterDepoimento(int depoimentoId)
        {
            return _repository.Get(depoimentoId);
        }

        /// <summary>
        /// 1 - Se for do usuario logado
        /// 2 - Se for de uma amigo
        /// </summary>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Depoimento> ObterDepoimentosEnviados(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            if (usuarioId == _currentUser.GetCurrentUserId())
                return _repository.GetEnviados(usuarioId, paginaAtual, itensPagina, StatusDepoimento.Todos, out totalRecords);

            var amizade = _amizadeService.ObterAmigo(_currentUser.GetCurrentUserId(), usuarioId);
            if (amizade != null)
                return _repository.GetEnviados(usuarioId, paginaAtual, itensPagina, StatusDepoimento.Todos, out totalRecords);

            totalRecords = 0;
            return new List<Depoimento>();
        }

        /// <summary>
        /// 1 - Se for do usuario logado
        /// 2 - Se for de uma amigo
        /// </summary>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Depoimento> ObterDepoimentosRecebidos(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            if (usuarioId == _currentUser.GetCurrentUserId())
                return _repository.GetRecebidos(usuarioId, paginaAtual, itensPagina, StatusDepoimento.Todos, out totalRecords);

            var amizade = _amizadeService.ObterAmigo(_currentUser.GetCurrentUserId(), usuarioId);
            if (amizade != null)
                return _repository.GetRecebidos(usuarioId, paginaAtual, itensPagina, StatusDepoimento.Todos, out totalRecords);

            totalRecords = 0;
            return new List<Depoimento>();
        }

        public void Dispose()
        {
            _repository.Dispose();
            _amizadeService.Dispose();
        }
    }
}
