using System.ComponentModel.DataAnnotations;

namespace Posy.V2.Infra.CrossCutting.Identity.NHibernate.Model
{
    public class LoginViewModel
    {
        [Display(Name = "User name")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
