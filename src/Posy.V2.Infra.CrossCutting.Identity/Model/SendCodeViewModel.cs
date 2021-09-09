using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Posy.V2.Infra.CrossCutting.Identity.Model
{
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }

        [HiddenInput]
        public Guid UserId { get; set; }
    }
}