using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;

namespace Posy.V2.Service
{
    public class VisitantePerfilService : IVisitantePerfilService
    {
        private IVisitantePerfilRepository _repository;
        private ICurrentUser _currentUser;

        public VisitantePerfilService(IVisitantePerfilRepository repository,
                                      ICurrentUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuarioIdVisitado"></param>
        public void SalvarVisita(int usuarioIdVisitado)
        {
            if (_currentUser.GetCurrentUserId() != usuarioIdVisitado)
            {
                var visita = new VisitantePerfil(_currentUser.GetCurrentUserId(), usuarioIdVisitado);

                _repository.Insert(visita);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<Perfil> ObterVisitantes(int take)
        {
            return _repository.GetVisitantes(_currentUser.GetCurrentUserId(), take);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
