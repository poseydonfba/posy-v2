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
    public class RecadoService : IRecadoService
    {
        private IRecadoRepository _repository;
        private IAmizadeService _amizadeService;
        private ICurrentUser _currentUser;

        public RecadoService(IRecadoRepository repository,
                             IAmizadeService amizadeService,
                             ICurrentUser currentUser)
        {
            _repository = repository;
            _amizadeService = amizadeService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Só envia recado pelo usuario logado
        /// </summary>
        /// <param name="enviadoParaId"></param>
        /// <param name="recado"></param>
        /// <returns></returns>
        public Recado EnviarRecado(int enviadoParaId, string recado)
        {
            var rec = new Recado(_currentUser.GetCurrentUserId(), enviadoParaId, recado);

            _repository.Insert(rec);

            return rec;
        }

        /// <summary>
        /// 1 - Se foi enviado para o usuario logado
        /// 2 - Se foi enviado pelo usuario logado
        /// </summary>
        /// <param name="recadoId"></param>
        public void ExcluirRecado(int recadoId)
        {
            var recado = ObterRecado(recadoId);
            if (recado.EnviadoParaId != _currentUser.GetCurrentUserId() &&
                recado.EnviadoPorId != _currentUser.GetCurrentUserId())
                throw new Exception(Errors.ExclusaoInvalida);

            recado.Delete(_currentUser.GetCurrentUserId());

            _repository.Remove(recado);
        }

        /// <summary>
        /// 1 - Se o recado foi enviado para o usuario logado
        /// </summary>
        /// <param name="recadoId"></param>
        public void MarcarComoLido(int recadoId)
        {
            var recado = ObterRecado(recadoId);
            if (recado == null)
                return;

            if (recado.StatusRecado == StatusRecado.NaoLido &&
                recado.EnviadoParaId == _currentUser.GetCurrentUserId())
            {
                recado.SetLeitura(StatusRecado.Lido);

                _repository.Update(recado);
            }
        }

        public Recado ObterRecado(int recadoId)
        {
            return _repository.Get(recadoId);
        }

        /// <summary>
        /// 1 - Se usuarioId for usuario logado
        /// 2 - Se usuarioId for amigo
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Recado> ObterRecadosEnviadosERecebidos(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            if (usuarioId == _currentUser.GetCurrentUserId())
                return _repository.GetEnviadosERecebidos(usuarioId, paginaAtual, itensPagina, StatusRecado.Todos, out totalRecords);

            var amizade = _amizadeService.ObterAmigo(_currentUser.GetCurrentUserId(), usuarioId);
            if (amizade != null)
                return _repository.GetEnviadosERecebidos(usuarioId, paginaAtual, itensPagina, StatusRecado.Todos, out totalRecords);

            totalRecords = 0;
            return new List<Recado>();
        }

        /// <summary>
        /// 1 - Se usuarioId for usuario logado
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Recado> ObterRecadosRecebidosNaoLidos(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            if (usuarioId == _currentUser.GetCurrentUserId())
                return _repository.GetRecebidos(usuarioId, paginaAtual, itensPagina, StatusRecado.NaoLido, out totalRecords);

            totalRecords = 0;
            return new List<Recado>();
        }

        public void Dispose()
        {
            _repository.Dispose();
            _amizadeService.Dispose();
        }
    }
}
