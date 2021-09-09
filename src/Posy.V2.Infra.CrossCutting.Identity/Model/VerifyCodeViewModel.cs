using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Posy.V2.Infra.CrossCutting.Identity.Model
{
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Código")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Lembrar este Browser?")]
        public bool RememberBrowser { get; set; }

        [HiddenInput]
        public Guid UserId { get; set; }
    }
}