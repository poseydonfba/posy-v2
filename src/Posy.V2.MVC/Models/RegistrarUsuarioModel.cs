using System;

namespace Posy.V2.MVC.Models
{
    public class RegistrarUsuarioModel
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string ConfirmacaoSenha { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }
        public string PaisId { get; set; }
    }
}