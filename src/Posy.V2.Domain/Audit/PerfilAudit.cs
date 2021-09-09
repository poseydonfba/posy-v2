using Posy.V2.Infra.CrossCutting.Common.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class PerfilAudit : Auditable
    {
        public Guid PerfilAuditId { get; private set; } = Guid.NewGuid();

        public Guid UsuarioId { get; private set; }
        public string Nome { get; private set; }
        public string Sobrenome { get; private set; }
        public string Alias { get; private set; }
        public string PaisId { get; set; }
        public DateTime DataNascimento { get; private set; }
        public Sexo Sexo { get; private set; }
        public EstadoCivil EstadoCivil { get; private set; }
        public byte[] FrasePerfil { get; private set; }
        public byte[] DescricaoPerfil { get; private set; }
        public DateTime Dar { get; set; }
    }
}
