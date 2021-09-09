using Posy.V2.Infra.CrossCutting.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace Posy.V2.Infra.CrossCutting.Identity.Model
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(UIConfigLogin))]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha", ResourceType = typeof(UIConfigLogin))]
        public string Password { get; set; }

        [Display(Name = "LembrarLogin", ResourceType = typeof(UIConfigLogin))]
        public bool RememberMe { get; set; }
    }
}