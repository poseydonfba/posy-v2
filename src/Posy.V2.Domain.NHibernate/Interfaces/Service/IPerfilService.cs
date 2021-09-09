using Posy.V2.Domain.Entities;
using Posy.V2.Infra.CrossCutting.Common.Enums;
using System;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IPerfilService : IDisposable
    {
        void Inserir(Perfil perfil);
        void Alterar(Perfil perfil);
        Perfil Obter(int usuarioId);
        Perfil Obter(string alias);
        Perfil EditarPerfil(int usuarioId, string nome, string sobrenome, string alias, DateTime dataNascimento, Sexo sexo, EstadoCivil estadoCivil, string frasePerfil, string descricaoPerfil, string paisId);
    }
}