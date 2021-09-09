using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Posy.V2.Service
{
    public class ModeradorService : IModeradorService
    {
        private IModeradorRepository _repository;
        //private IComunidadeService _comunidadeService; // Dando conflito circular de injeção de dependencia
        private IPerfilService _perfilService;
        //private IMembroService _membroService;
        private IAmizadeService _amizadeService;
        private ICurrentUser _currentUser;

        public ModeradorService(IModeradorRepository repository,
                                //IComunidadeService comunidadeService,
                                IPerfilService perfilService,
                                //IMembroService membroService,
                                IAmizadeService amizadeService,
                                ICurrentUser currentUser)
        {
            _repository = repository;
            //_comunidadeService = comunidadeService;
            _perfilService = perfilService;
            //_membroService = membroService;
            _amizadeService = amizadeService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 1 - Se a comunidade existir
        /// 2 - Se nao for moderador
        /// </summary>
        /// <param name="comunidadeId"></param>
        /// <param name="usuarioModeradorId"></param>
        /// <returns></returns>
        public Moderador AdicionarModerador(int comunidadeId, int usuarioModeradorId)
        {
            //var comunidade = _comunidadeService.Obter(comunidadeId);
            //if (comunidade == null)
            //    throw new Exception("Erro");

            var m = Obter(comunidadeId, usuarioModeradorId);
            if (m != null)
                throw new Exception("Erro");

            var moderador = new Moderador(comunidadeId, usuarioModeradorId, _currentUser.GetCurrentUserId());

            _repository.Insert(moderador);

            return moderador;
        }

        /// <summary>
        /// 1 - Se comunidade e usuario existir
        /// 2 - Se for dono da comunidade
        /// </summary>
        /// <param name="comunidadeId"></param>
        /// <param name="usuarioId"></param>
        public void ExcluirModerador(int comunidadeId, int usuarioId)
        {
            //var comunidade = _comunidadeService.Obter(comunidadeId);
            //if (comunidade == null)
            //    throw new Exception("Erro");

            //if (comunidade.UsuarioId != _currentUser.GetCurrentUserId())
            //    throw new Exception("Erro");

            var perfil = _perfilService.Obter(usuarioId);
            if (perfil == null)
                throw new Exception("Erro");

            var moderador = Obter(comunidadeId, usuarioId);
            moderador.Delete(_currentUser.GetCurrentUserId());

            _repository.Remove(moderador);
        }

        public Moderador Obter(int comunidadeId, int usuarioId)
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
        /// <returns></returns>
        public IEnumerable<Moderador> ObterModeradores(int comunidadeId)
        {
            //var comunidade = _comunidadeService.Obter(comunidadeId);
            //if (comunidade == null)
            //    return new List<Moderador>();

            //if (comunidade.UsuarioId == _currentUser.GetCurrentUserId())
            //    return _repository.GetModeradores(comunidadeId).ToList();

            //var moderador = Obter(comunidadeId, _currentUser.GetCurrentUserId());
            //if (moderador != null)
            //    return _repository.GetModeradores(comunidadeId).ToList();

            //var membro = _membroService.Obter(comunidadeId, _currentUser.GetCurrentUserId());
            //if (membro != null)
            //    return _repository.GetModeradores(comunidadeId).ToList();

            //return new List<Moderador>();

            return _repository.GetModeradores(comunidadeId).ToList();
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
        public IEnumerable<Moderador> ObterModeradores(int comunidadeId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            //var comunidade = _comunidadeService.Obter(comunidadeId);
            //if (comunidade == null)
            //{
            //    totalRecords = 0;
            //    return new List<Moderador>();
            //}

            //if (comunidade.UsuarioId == _currentUser.GetCurrentUserId())
            //    return _repository.GetModeradores(comunidadeId, paginaAtual, itensPagina, out totalRecords).ToList();

            //var moderador = Obter(comunidadeId, _currentUser.GetCurrentUserId());
            //if (moderador != null)
            //    return _repository.GetModeradores(comunidadeId, paginaAtual, itensPagina, out totalRecords).ToList();

            //var membro = _membroService.Obter(comunidadeId, _currentUser.GetCurrentUserId());
            //if (membro != null)
            //    return _repository.GetModeradores(comunidadeId, paginaAtual, itensPagina, out totalRecords).ToList();

            //totalRecords = 0;
            //return new List<Moderador>();

            return _repository.GetModeradores(comunidadeId, paginaAtual, itensPagina, out totalRecords).ToList();
        }

        /// <summary>
        /// 1 - Se usuario existir
        /// 2 - Se for amigo do usuario logado
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Moderador> ObterComunidades(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            var perfil = _perfilService.Obter(usuarioId);
            if (perfil == null)
            {
                totalRecords = 0;
                return new List<Moderador>();
            }

            var amizade = _amizadeService.ObterAmigo(_currentUser.GetCurrentUserId(), usuarioId);
            if (amizade == null)
            {
                totalRecords = 0;
                return new List<Moderador>();
            }

            return _repository.GetComunidades(usuarioId, paginaAtual, itensPagina, out totalRecords).ToList();
        }

        public void Dispose()
        {
            _repository.Dispose();
            //_comunidadeService.Dispose();
            _perfilService.Dispose();
            //_membroService.Dispose();
            _amizadeService.Dispose();
        }
    }
}
