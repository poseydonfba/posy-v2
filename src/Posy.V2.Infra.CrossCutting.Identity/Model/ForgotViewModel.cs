using System.ComponentModel.DataAnnotations;

namespace Posy.V2.Infra.CrossCutting.Identity.Model
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}