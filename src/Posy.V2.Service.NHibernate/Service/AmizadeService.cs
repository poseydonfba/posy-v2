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
    public class AmizadeService : IAmizadeService
    {
        private IAmizadeRepository _repository;
        private ICurrentUser _currentUser;

        public AmizadeService(IAmizadeRepository repository,
                              ICurrentUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 1 - Se ainda não tiver nenhuma solicitação enviada
        /// </summary>
        /// <param name="usuarioIdSolicitado">Usuario que foi enviado a solicitação de amizade</param>
        public void AdicionarSolicitacaoAmizade(int usuarioIdSolicitado)
        {
            var solicitacao = Obter(_currentUser.GetCurrentUserId(), usuarioIdSolicitado);
            if (solicitacao != null && solicitacao.StatusSolicitacao == SolicitacaoAmizade.Pendente)
                throw new Exception(Errors.SolicitacaoAmizadeJaEnviada);

            var amizade = new Amizade(_currentUser.GetCurrentUserId(), usuarioIdSolicitado);

            _repository.Insert(amizade);
        }

        /// <summary>
        /// 1 - Se existir uma solicitação de amizade
        /// </summary>
        /// <param name="usuarioIdParaExcluir"></param>
        public void ExcluirAmigo(int usuarioIdParaExcluir)
        {
            var amizade = Obter(_currentUser.GetCurrentUserId(), usuarioIdParaExcluir);
            if (amizade == null)
                throw new Exception(Errors.ExclusaoInvalida);

            amizade.Delete(_currentUser.GetCurrentUserId());

            _repository.Remove(amizade);
        }

        /// <summary>
        /// 1 - Se existir uma solicitação ainda não aprovada
        /// </summary>
        /// <param name="usuarioIdAceitar"></param>
        public void AceitarSolicitacaoAmizade(int usuarioIdAceitar)
        {
            var solicitacaoAmizade = Obter(_currentUser.GetCurrentUserId(), usuarioIdAceitar);
            if (solicitacaoAmizade == null ||
                solicitacaoAmizade?.StatusSolicitacao == SolicitacaoAmizade.Aprovado)
                throw new Exception(Errors.SolicitacaoAmizadeInvalida);

            solicitacaoAmizade.SetResposta(SolicitacaoAmizade.Aprovado);

            _repository.Update(solicitacaoAmizade);
        }

        /// <summary>
        /// 1 - Se exitir uma solicitação não Rejeitado
        /// </summary>
        /// <param name="usuarioIdRecusar"></param>
        public void RecusarSolicitacaoAmizade(int usuarioIdRecusar)
        {
            var solicitacaoAmizade = Obter(usuarioIdRecusar, _currentUser.GetCurrentUserId());
            if (solicitacaoAmizade == null ||
                solicitacaoAmizade?.StatusSolicitacao == SolicitacaoAmizade.Rejeitado)
                throw new Exception(Errors.SolicitacaoAmizadeInvalida);

            solicitacaoAmizade.SetResposta(SolicitacaoAmizade.Rejeitado);

            _repository.Update(solicitacaoAmizade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuarioId1"></param>
        /// <param name="usuarioId2"></param>
        /// <returns></returns>
        public Amizade Obter(int usuarioId1, int usuarioId2)
        {
            return _repository.Get(usuarioId1, usuarioId2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="usuarioIdAmigo"></param>
        /// <returns></returns>
        public Amizade ObterAmigo(int usuarioId, int usuarioIdAmigo)
        {
            var amizade = _repository.Get(usuarioId, usuarioIdAmigo);

            if (amizade == null)
                return null;

            return amizade.Aprovado && amizade.Der == null ? amizade : null;
        }

        /// <summary>
        /// Traz apenas do usuario logado
        /// </summary>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Amizade> SolicitacoesRecebidasPendentes(int paginaAtual, int itensPagina, out int totalRecords)
        {
            return _repository.Get(_currentUser.GetCurrentUserId(), paginaAtual, itensPagina, SolicitacaoAmizade.Pendente, out totalRecords);
        }

        /// <summary>
        /// 1 - Se for o usuario logado
        /// 2 - Se o usuario for amigo
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Usuario> ObterAmigos(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            if (usuarioId == _currentUser.GetCurrentUserId())
                return _repository.GetAmigos(usuarioId, paginaAtual, itensPagina, out totalRecords);

            var amizade = ObterAmigo(_currentUser.GetCurrentUserId(), usuarioId);
            if (amizade != null)
                return _repository.GetAmigos(usuarioId, paginaAtual, itensPagina, out totalRecords);

            totalRecords = 0;
            return new List<Usuario>();
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
