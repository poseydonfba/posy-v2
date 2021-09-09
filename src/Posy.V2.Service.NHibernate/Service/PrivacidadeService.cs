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
        private IUsuarioService _usuarioService;
        private ICurrentUser _currentUser;

        public PrivacidadeService(IPrivacidadeRepository repository,
                                  IUsuarioService usuarioService,
                                  ICurrentUser currentUser)
        {
            _repository = repository;
            _usuarioService = usuarioService;
            _currentUser = currentUser;
        }

        public void IncluirPrivacidade(int usuarioId, int verRecado, int escreverRecado)
        {
            var user = _usuarioService.GetUsuario(usuarioId);
            var privacidade = new Privacidade(usuarioId, verRecado, escreverRecado);
            privacidade.Usuario = user;
            _repository.Insert(privacidade);
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
