using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using System;

namespace Posy.V2.Service
{
    public class PostOcultoService : IPostOcultoService
    {
        private IPostOcultoRepository _repository;
        private ICurrentUser _currentUser;

        public PostOcultoService(IPostOcultoRepository repository,
                                 ICurrentUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Pode ocultar o post de qualquer usuario, por que o usuario que esta ocultando 
        /// será sempre o usuario logado
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="postPerfilId"></param>
        public void OcultarPost(Guid postPerfilId)
        {
            var post = new PostOculto(_currentUser.GetCurrentUserId(), postPerfilId, StatusPostOculto.Oculto);

            _repository.Insert(post);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
