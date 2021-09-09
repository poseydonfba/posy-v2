using Posy.V2.Infra.CrossCutting.Common.Enums;
using System;

namespace Posy.V2.WF.Models
{
    public class EditarPerfilModel
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Alias { get; set; }
        public DateTime DataNascimento { get; set; }
        public Sexo Sexo { get; set; }
        public EstadoCivil EstadoCivil { get; set; }
        public string FrasePerfil { get; set; }
        public string DescricaoPerfil { get; set; }
        public string PaisId { get; set; }
    }
}