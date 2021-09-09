using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using System;

namespace Posy.V2.Service
{
    public class PostPerfilBloqueadoService : IPostPerfilBloqueadoService
    {
        private IPostPerfilBloqueadoRepository _repository;
        private ICurrentUser _currentUser;

        public PostPerfilBloqueadoService(IPostPerfilBloqueadoRepository repository,
                                          ICurrentUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Pode bloquear qualquer usuario, por que o usuario que esta bloqueando 
        /// será sempre o usuario logado
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="usuarioIdBloquear"></param>
        public void BloquearPostPerfil(Guid usuarioIdBloquear)
        {
            var perfilBloquear = new PostPerfilBloqueado(_currentUser.GetCurrentUserId(), usuarioIdBloquear);

            _repository.Insert(perfilBloquear);
        }

        public PostPerfilBloqueado ObterPerfilBloqueado(Guid usuarioId, Guid usuarioIdBloqueado)
        {
            return _repository.Get(usuarioId, usuarioIdBloqueado);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
