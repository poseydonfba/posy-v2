using System.ComponentModel.DataAnnotations;

namespace Posy.V2.Infra.CrossCutting.Identity.Model
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Número do Telefone")]
        public string Number { get; set; }
    }
}