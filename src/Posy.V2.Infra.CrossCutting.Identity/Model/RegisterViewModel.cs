using Posy.V2.Infra.CrossCutting.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace Posy.V2.Infra.CrossCutting.Identity.Model
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(UIConfigLogin))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, /*ErrorMessage = "TamanhoDeSenha", */ MinimumLength = 6, ErrorMessageResourceType = typeof(UIConfigLogin), ErrorMessageResourceName = "TamanhoDeSenha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha", ResourceType = typeof(UIConfigLogin))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmarSenha", ResourceType = typeof(UIConfigLogin))]
        [Compare("Password", /*ErrorMessage = "AsSenhasNaoSeCoincidem",*/ ErrorMessageResourceType = typeof(UIConfigLogin), ErrorMessageResourceName = "AsSenhasNaoSeCoincidem")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Nome", ResourceType = typeof(UIConfigLogin))]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Sobrenome", ResourceType = typeof(UIConfigLogin))]
        public string Sobrenome { get; set; }

        [Required]
        [Display(Name = "DataDeNascimento", ResourceType = typeof(UIConfigLogin))]
        public int Dia { get; set; }

        [Required]
        [Display(Name = "Mes", ResourceType = typeof(UIConfigLogin))]
        public int Mes { get; set; }

        [Required]
        [Display(Name = "Ano", ResourceType = typeof(UIConfigLogin))]
        public int Ano { get; set; }

        [Required]
        [Display(Name = "Sexo", ResourceType = typeof(UIConfigLogin))]
        public int Sexo { get; set; }

        [Required]
        [Display(Name = "EstadoCivil", ResourceType = typeof(UIConfigLogin))]
        public int EstadoCivil { get; set; }

        [Required]
        [Display(Name = "InformeoTextoDaImagem", ResourceType = typeof(UIConfigLogin))]
        public string Captcha { get; set; }
    }
}