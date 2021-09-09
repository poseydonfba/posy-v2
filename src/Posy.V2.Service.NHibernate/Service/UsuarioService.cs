using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;

namespace Posy.V2.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private ICurrentUser _currentUser;

        public UsuarioService(IUsuarioRepository repository,
                              ICurrentUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public void UpdateCulture(string language)
        {
            var user = _repository.Get(_currentUser.GetCurrentUserId());
            user.Language = language;

            _repository.Update(user);
        }

        public Usuario GetUsuario(int id)
        {
            return _repository.Get(id);
        }

        public Usuario SaveOrUpdate(Usuario usuario)
        {
            _repository.SaveOrUpdate(usuario);

            return usuario;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
