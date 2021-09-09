using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;

namespace Posy.V2.Service
{
    public class TopicoPostService : ITopicoPostService
    {
        private ITopicoPostRepository _repository;
        private ITopicoService _topicoService;
        private IComunidadeService _comunidadeService;
        private IModeradorService _moderadorService;
        private IMembroService _membroService;
        private ICurrentUser _currentUser;

        public TopicoPostService(ITopicoPostRepository repository,
                                 IComunidadeService comunidadeService,
                                 IModeradorService moderadorService,
                                 IMembroService membroService,
                                 ITopicoService topicoService,
                                 ICurrentUser currentUser)
        {
            _repository = repository;
            _topicoService = topicoService;
            _comunidadeService = comunidadeService;
            _moderadorService = moderadorService;
            _membroService = membroService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 1 - Se topico existir
        /// </summary>
        /// <param name="topicoId"></param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        public TopicoPost SalvarPost(int topicoId, string descricao)
        {
            var topico = _topicoService.Obter(topicoId);
            if (topico == null)
                throw new Exception("Erro");

            var post = new TopicoPost(topicoId, _currentUser.GetCurrentUserId(), descricao);

            _repository.Insert(post);

            return post;
        }

        /// <summary>
        /// 1 - Se post existir
        /// 2 - Se for dono do post
        /// 3 - Se for dono da comunidade
        /// 4 - Se for moderador da comunidade
        /// </summary>
        /// <param name="postId"></param>
        public void ExcluirTopicoPost(int postId)
        {
            var p = Obter(postId);
            if (p == null)
                throw new Exception("Erro");

            if (p.UsuarioId == _currentUser.GetCurrentUserId())
            {
                Excluir(postId);
                return;
            }

            var topico = _topicoService.Obter(p.TopicoId);

            var comunidade = _comunidadeService.Obter(topico.ComunidadeId);
            if (comunidade.UsuarioId == _currentUser.GetCurrentUserId())
            {
                Excluir(postId);
                return;
            }

            var moderador = _moderadorService.Obter(comunidade.Id, _currentUser.GetCurrentUserId());
            if (moderador != null)
            {
                Excluir(postId);
                return;
            }

            throw new Exception("Erro");
        }
        public void Excluir(int postId)
        {
            var post = Obter(postId);
            post.Delete(_currentUser.GetCurrentUserId());

            _repository.Remove(post);
        }

        /// <summary>
        /// 1 - Se post existir
        /// 2 - Se for dono do post
        /// 3 - Se for dono da comunidade
        /// 4 - Se for moderador da comunidade
        /// </summary>
        /// <param name="postId"></param>
        public void ExcluirTopicoPostPermanente(int postId)
        {
            var p = Obter(postId);
            if (p == null)
                throw new Exception("Erro");

            if (p.UsuarioId == _currentUser.GetCurrentUserId())
            {
                ExcluirPermanente(postId);
                return;
            }

            var topico = _topicoService.Obter(p.TopicoId);

            var comunidade = _comunidadeService.Obter(topico.ComunidadeId);
            if (comunidade.UsuarioId == _currentUser.GetCurrentUserId())
            {
                ExcluirPermanente(postId);
                return;
            }

            var moderador = _moderadorService.Obter(comunidade.Id, _currentUser.GetCurrentUserId());
            if (moderador != null)
            {
                ExcluirPermanente(postId);
                return;
            }

            throw new Exception("Erro");
        }
        public void ExcluirPermanente(int postId)
        {
            var post = Obter(postId);
            post.DeletePermanente(_currentUser.GetCurrentUserId());

            _repository.RemovePermanente(post);
        }

        public TopicoPost Obter(int postId)
        {
            return _repository.Get(postId);
        }

        /// <summary>
        /// 1 - Se o post existir
        /// 2 - Se for o dono da comunidade
        /// 3 - Se for moderador da comunidade
        /// 4 - Se for membro da comunidade
        /// </summary>
        /// <param name="topicoId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <param name="ultimoPost"></param>
        /// <returns></returns>
        public IEnumerable<TopicoPost> ObterPosts(int topicoId, int paginaAtual, int itensPagina, out int totalRecords, out TopicoPost ultimoPost)
        {
            var topico = _topicoService.Obter(topicoId);
            if (topico == null)
            {
                ultimoPost = null;
                totalRecords = 0;
                return new List<TopicoPost>();
            }

            var comunidade = _comunidadeService.Obter(topico.ComunidadeId);

            if (comunidade.UsuarioId == _currentUser.GetCurrentUserId())
                return _repository.GetPosts(topicoId, paginaAtual, itensPagina, out totalRecords, out ultimoPost);

            var moderador = _moderadorService.Obter(comunidade.Id, _currentUser.GetCurrentUserId());
            if (moderador != null)
                return _repository.GetPosts(topicoId, paginaAtual, itensPagina, out totalRecords, out ultimoPost);

            var membro = _membroService.Obter(comunidade.Id, _currentUser.GetCurrentUserId());
            if (membro != null)
                return _repository.GetPosts(topicoId, paginaAtual, itensPagina, out totalRecords, out ultimoPost);

            ultimoPost = null;
            totalRecords = 0;
            return new List<TopicoPost>();
        }

        public void Dispose()
        {
            _repository.Dispose();
            _topicoService.Dispose();
            _comunidadeService.Dispose();
            _moderadorService.Dispose();
            _membroService.Dispose();
        }
    }
}
