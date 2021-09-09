using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Posy.V2.Service
{
    public class MembroService : IMembroService
    {
        private IMembroRepository _repository;
        private IComunidadeService _comunidadeService;
        private IModeradorService _moderadorService;
        private IPerfilService _perfilService;
        private IAmizadeService _amizadeService;
        private ICurrentUser _currentUser;

        public MembroService(IMembroRepository repository,
                             IComunidadeService comunidadeService,
                             IModeradorService moderadorService,
                             IPerfilService perfilService,
                             IAmizadeService amizadeService,
                             ICurrentUser currentUser)
        {
            _repository = repository;
            _comunidadeService = comunidadeService;
            _moderadorService = moderadorService;
            _perfilService = perfilService;
            _amizadeService = amizadeService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 1 - Se a comunidade existir
        /// 2 - Se nao for membro
        /// </summary>
        /// <param name="comunidadeId"></param>
        /// <returns></returns>
        public Membro SolicitarParticipacao(int comunidadeId)
        {
            var comunidade = _comunidadeService.Obter(comunidadeId);
            if (comunidade == null)
                throw new Exception(Errors.ComunidadeInvalida);

            var m = Obter(comunidadeId, _currentUser.GetCurrentUserId());
            if (m != null && m?.StatusSolicitacao != StatusSolicitacaoMembroComunidade.Rejeitado)
                throw new Exception(Errors.ComunidadeInvalida);

            var membro = new Membro(comunidadeId, _currentUser.GetCurrentUserId());

            _repository.Insert(membro);

            return membro;
        }

        public void AdicionarMembro(int comunidadeId, int usuarioMembroId)
        {
            var membro = new Membro(comunidadeId, usuarioMembroId);
            membro.AdicionarMembro();

            _repository.Insert(membro);
        }

        /// <summary>
        /// 1 - Se form membro
        /// 2 - Se o usuario logado for dono da comunidade
        /// 2 - Se o usuario logado for moderador da comunidade
        /// </summary>
        /// <param name="comunidadeId"></param>
        /// <param name="usuarioMembroId"></param>
        public void ExcluirMembro(int comunidadeId, int usuarioMembroId)
        {
            var m = Obter(comunidadeId, usuarioMembroId);
            if (m == null)
                throw new Exception(Errors.ComunidadeInvalida);

            var comunidade = _comunidadeService.Obter(comunidadeId);
            if (comunidade == null)
                throw new Exception(Errors.ComunidadeInvalida);

            var moderador = _moderadorService.Obter(comunidadeId, _currentUser.GetCurrentUserId());
            if (comunidade.UsuarioId != _currentUser.GetCurrentUserId() &&
                moderador == null)
                throw new Exception(Errors.ComunidadeInvalida);

            var membro = new Membro(comunidadeId, usuarioMembroId);
            membro.Delete(_currentUser.GetCurrentUserId());

            _repository.Remove(membro);
        }

        /// <summary>
        /// 1 - Se houver solicitação pendente
        /// 2 - Se o usuario logado for dono da comunidade
        /// 2 - Se o usuario logado for moderador da comunidade
        /// </summary>
        /// <param name="comunidadeId"></param>
        /// <param name="usuarioId"></param>
        public void AceitarMembro(int comunidadeId, int usuarioId)
        {
            var membro = Obter(comunidadeId, usuarioId);
            if (membro == null || membro?.StatusSolicitacao != StatusSolicitacaoMembroComunidade.Pendente)
                throw new Exception(Errors.ComunidadeInvalida);

            var comunidade = _comunidadeService.Obter(comunidadeId);
            if (comunidade == null)
                throw new Exception(Errors.ComunidadeInvalida);

            var moderador = _moderadorService.Obter(comunidadeId, _currentUser.GetCurrentUserId());
            if (comunidade.UsuarioId != _currentUser.GetCurrentUserId() &&
                moderador == null)
                throw new Exception(Errors.ComunidadeInvalida);

            membro.SetResposta(StatusSolicitacaoMembroComunidade.Aprovado, _currentUser.GetCurrentUserId());

            _repository.Update(membro);
        }

        /// <summary>
        /// 1 - Se houver solicitação pendente
        /// 2 - Se o usuario logado for dono da comunidade
        /// 2 - Se o usuario logado for moderador da comunidade
        /// </summary>
        /// <param name="comunidadeId"></param>
        /// <param name="usuarioId"></param>
        public void RecusarMembro(int comunidadeId, int usuarioId)
        {
            var membro = Obter(comunidadeId, usuarioId);
            if (membro == null || membro?.StatusSolicitacao != StatusSolicitacaoMembroComunidade.Pendente)
                throw new Exception(Errors.ComunidadeInvalida);

            var comunidade = _comunidadeService.Obter(comunidadeId);
            if (comunidade == null)
                throw new Exception(Errors.ComunidadeInvalida);

            var moderador = _moderadorService.Obter(comunidadeId, _currentUser.GetCurrentUserId());
            if (comunidade.UsuarioId != _currentUser.GetCurrentUserId() &&
                moderador == null)
                throw new Exception(Errors.ComunidadeInvalida);

            membro.SetResposta(StatusSolicitacaoMembroComunidade.Rejeitado, _currentUser.GetCurrentUserId());

            _repository.Update(membro);
        }

        public Membro Obter(int comunidadeId, int usuarioId)
        {
            return _repository.Get(comunidadeId, usuarioId);
        }

        /// <summary>
        /// 1 - Se a comunidade existir
        /// 2 - Se for dono da comunidade
        /// 3 - Se for moderador da comunidade
        /// 4 - Se for membro da comunidade
        /// </summary>
        /// <param name="comunidadeId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Membro> ObterMembros(int comunidadeId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            var comunidade = _comunidadeService.Obter(comunidadeId);
            if (comunidade == null)
            {
                totalRecords = 0;
                return new List<Membro>();
            }

            var membro = Obter(comunidadeId, _currentUser.GetCurrentUserId());
            if (membro != null)
                return _repository.GetMembros(comunidadeId, paginaAtual, itensPagina, StatusSolicitacaoMembroComunidade.Aprovado, out totalRecords).ToList();

            var moderador = _moderadorService.Obter(comunidadeId, _currentUser.GetCurrentUserId());
            if (comunidade.UsuarioId == _currentUser.GetCurrentUserId() || moderador != null)
                return _repository.GetMembros(comunidadeId, paginaAtual, itensPagina, StatusSolicitacaoMembroComunidade.Aprovado, out totalRecords).ToList();

            totalRecords = 0;
            return new List<Membro>();
        }

        /// <summary>
        /// 1 - Se a comunidade existir
        /// 2 - Se for dono da comunidade
        /// 3 - Se for moderador da comunidade
        /// </summary>
        /// <param name="comunidadeId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Membro> ObterMembrosPendentes(int comunidadeId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            var comunidade = _comunidadeService.Obter(comunidadeId);
            if (comunidade == null)
            {
                totalRecords = 0;
                return new List<Membro>();
            }

            var moderador = _moderadorService.Obter(comunidadeId, _currentUser.GetCurrentUserId());
            if (comunidade.UsuarioId == _currentUser.GetCurrentUserId() || moderador != null)
                return _repository.GetMembros(comunidadeId, paginaAtual, itensPagina, StatusSolicitacaoMembroComunidade.Pendente, out totalRecords).ToList();

            totalRecords = 0;
            return new List<Membro>();
        }

        /// <summary>
        /// 1 - Se usuario existir
        /// 2 - Se for o usuario logado
        /// 3 - Se for amigo do usuario logado
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Membro> ObterComunidades(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            var perfil = _perfilService.Obter(usuarioId);
            if (perfil == null)
            {
                totalRecords = 0;
                return new List<Membro>();
            }

            if (usuarioId == _currentUser.GetCurrentUserId())
                return _repository.GetComunidades(usuarioId, paginaAtual, itensPagina, StatusSolicitacaoMembroComunidade.Aprovado, out totalRecords).ToList();

            var amizade = _amizadeService.ObterAmigo(_currentUser.GetCurrentUserId(), usuarioId);
            if (amizade == null)
            {
                totalRecords = 0;
                return new List<Membro>();
            }

            return _repository.GetComunidades(usuarioId, paginaAtual, itensPagina, StatusSolicitacaoMembroComunidade.Aprovado, out totalRecords).ToList();
        }

        public void Dispose()
        {
            _repository.Dispose();
            _comunidadeService.Dispose();
            _moderadorService.Dispose();
            _perfilService.Dispose();
            _amizadeService.Dispose();
        }
    }
}
