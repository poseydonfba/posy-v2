using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;

namespace Posy.V2.Service
{
    public class TopicoService : ITopicoService
    {
        private ITopicoRepository _repository;
        private IComunidadeService _comunidadeService;
        private IModeradorService _moderadorService;
        private IMembroService _membroService;
        private ICurrentUser _currentUser;

        public TopicoService(ITopicoRepository repository,
                             IComunidadeService comunidadeService,
                             IModeradorService moderadorService,
                             IMembroService membroService,
                             ICurrentUser currentUser)
        {
            _repository = repository;
            _comunidadeService = comunidadeService;
            _moderadorService = moderadorService;
            _membroService = membroService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 1 - Se a comunidade existir
        /// </summary>
        /// <param name="comunidadeId"></param>
        /// <param name="titulo"></param>
        /// <param name="descricao"></param>
        /// <param name="fixo"></param>
        /// <returns></returns>
        public Topico SalvarTopico(int comunidadeId, string titulo, string descricao, TipoTopico fixo)
        {
            var comunidade = _comunidadeService.Obter(comunidadeId);
            if (comunidade == null)
                throw new Exception("Erro");

            var topico = new Topico(comunidadeId, _currentUser.GetCurrentUserId(), titulo, descricao, fixo);

            _repository.Insert(topico);

            return topico;
        }

        /// <summary>
        /// 1 - Se topico existir
        /// 2 - Se for o dono do topico
        /// 3 - Se for o dono da comunidade
        /// 4 - Se for moderador da comunidade
        /// </summary>
        /// <param name="topicoId"></param>
        public void ExcluirTopico(int topicoId)
        {
            var t = Obter(topicoId);
            if (t == null)
                throw new Exception("Erro");

            if (t.UsuarioId == _currentUser.GetCurrentUserId())
            {
                Excluir(topicoId);
                return;
            }

            var comunidade = _comunidadeService.Obter(t.ComunidadeId);
            if (comunidade.UsuarioId == _currentUser.GetCurrentUserId())
            {
                Excluir(topicoId);
                return;
            }

            var moderador = _moderadorService.Obter(comunidade.Id, _currentUser.GetCurrentUserId());
            if (moderador != null)
            {
                Excluir(topicoId);
                return;
            }

            throw new Exception("Erro");
        }
        private void Excluir(int topicoId)
        {
            var topico = Obter(topicoId);
            topico.Delete(_currentUser.GetCurrentUserId());

            _repository.Remove(topico);
        }

        /// <summary>
        /// 1 - Se topico existir
        /// 2 - Se for o dono do topico
        /// 3 - Se for o dono da comunidade
        /// 4 - Se for moderador da comunidade
        /// </summary>
        /// <param name="topicoId"></param>
        public void ExcluirTopicoPermanente(int topicoId)
        {
            var t = Obter(topicoId);
            if (t == null)
                throw new Exception("Erro");

            if (t.UsuarioId == _currentUser.GetCurrentUserId())
            {
                ExcluirPermanente(topicoId);
                return;
            }

            var comunidade = _comunidadeService.Obter(t.ComunidadeId);
            if (comunidade.UsuarioId == _currentUser.GetCurrentUserId())
            {
                ExcluirPermanente(topicoId);
                return;
            }

            var moderador = _moderadorService.Obter(comunidade.Id, _currentUser.GetCurrentUserId());
            if (moderador != null)
            {
                ExcluirPermanente(topicoId);
                return;
            }

            throw new Exception("Erro");
        }
        private void ExcluirPermanente(int topicoId)
        {
            var topico = Obter(topicoId);
            topico.DeletePermanente(_currentUser.GetCurrentUserId());

            _repository.RemovePermanente(topico);
        }

        public Topico Obter(int topicoId)
        {
            return _repository.Get(topicoId);
        }

        /// <summary>
        /// 1 - Se a comunidade existir
        /// 2 - Se for o dono da comunidade
        /// 3 - Se for moderador da comunidade
        /// 4 - Se for membro da comunidade
        /// </summary>
        /// <param name="comunidadeId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Topico> ObterTopicos(int comunidadeId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            var comunidade = _comunidadeService.Obter(comunidadeId);
            if (comunidade == null)
            {
                totalRecords = 0;
                return new List<Topico>();
            }

            if (comunidade.UsuarioId == _currentUser.GetCurrentUserId())
                return _repository.GetTopicos(comunidadeId, paginaAtual, itensPagina, out totalRecords);

            var moderador = _moderadorService.Obter(comunidade.Id, _currentUser.GetCurrentUserId());
            if (moderador != null)
                return _repository.GetTopicos(comunidadeId, paginaAtual, itensPagina, out totalRecords);

            var membro = _membroService.Obter(comunidade.Id, _currentUser.GetCurrentUserId());
            if (membro != null)
                return _repository.GetTopicos(comunidadeId, paginaAtual, itensPagina, out totalRecords);

            totalRecords = 0;
            return new List<Topico>();
        }

        public void Dispose()
        {
            _repository.Dispose();
            _comunidadeService.Dispose();
            _moderadorService.Dispose();
            _membroService.Dispose();
        }
    }
}
