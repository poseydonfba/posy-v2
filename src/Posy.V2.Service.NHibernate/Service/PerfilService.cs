using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using Posy.V2.Infra.CrossCutting.Common.Enums;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Sanitizer;
using System;

namespace Posy.V2.Service
{
    public class PerfilService : IPerfilService
    {
        private IPerfilRepository _repository;
        private ICacheService _cacheService;

        public PerfilService(IPerfilRepository repository,
                             ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public void Inserir(Perfil perfil)
        {
            _repository.Insert(perfil);
        }

        public void Alterar(Perfil perfil)
        {
            _repository.Update(perfil);
        }

        public Perfil EditarPerfil(int usuarioId, string nome, string sobrenome, string alias, DateTime dataNascimento, Sexo sexo, EstadoCivil estadoCivil, string frasePerfil, string descricaoPerfil, string paisId)
        {
            var perfil = _repository.GetByUsuario(usuarioId);

            if (perfil == null)
                throw new Exception(Errors.PerfilNaoEncontrado);

            perfil.Edit(nome, sobrenome, alias, dataNascimento, sexo, estadoCivil, frasePerfil, HtmlSanitizer.SanitizeHtml(descricaoPerfil), paisId);
            perfil.Validate();

            _repository.Update(perfil);

            return perfil;
        }

        public Perfil Obter(int usuarioId)
        {
            var perfil = _cacheService.GetOrSet(
                () => _repository.GetByUsuario(usuarioId),
                usuarioId.ToString(),
                new TimeSpan(0, 60, 0));

            return perfil;
        }

        public Perfil Obter(string alias)
        {
            var perfil = _cacheService.GetOrSet(
                () => _repository.GetByAlias(alias),
                alias,
                new TimeSpan(0, 60, 0));

            return perfil;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
