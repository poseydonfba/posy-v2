using Posy.V2.Infra.CrossCutting.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace Posy.V2.Infra.CrossCutting.Identity.Model
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(UIConfigLogin))]
        public string Email { get; set; }

        //[Required]
        //[Display(Name = "Nome")]
        //public string Name { get; set; }

        //[Required]
        //[Display(Name = "Sobrenome")]
        //public string LastName { get; set; }


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