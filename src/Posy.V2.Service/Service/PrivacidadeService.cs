using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using System;

namespace Posy.V2.Service
{
    public class PrivacidadeService : IPrivacidadeService
    {
        private IPrivacidadeRepository _repository;
        private ICurrentUser _currentUser;

        public PrivacidadeService(IPrivacidadeRepository repository,
                                  ICurrentUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public void IncluirPrivacidade(Guid usuarioId, int verRecado, int escreverRecado)
        {
            _repository.Insert(new Privacidade(usuarioId, verRecado, escreverRecado));
        }

        /// <summary>
        /// Salva apenas do usuario logado
        /// </summary>
        /// <param name="verRecado"></param>
        /// <param name="escreverRecado"></param>
        public void SalvarPrivacidade(int verRecado, int escreverRecado)
        {
            var privacidade = Obter();
            if (privacidade == null)
                throw new Exception(Errors.UsuarioInvalido);

            privacidade.Edit(verRecado, escreverRecado);

            _repository.Update(privacidade);
        }

        /// <summary>
        /// Retorna apenas do usuario logado
        /// </summary>
        /// <returns></returns>
        public Privacidade Obter()
        {
            return _repository.GetByUsuario(_currentUser.GetCurrentUserId());
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
