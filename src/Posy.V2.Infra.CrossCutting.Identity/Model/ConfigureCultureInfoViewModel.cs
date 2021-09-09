using System.Collections.Generic;
using System.Web.Mvc;

namespace Posy.V2.Infra.CrossCutting.Identity.Model
{
    public class ConfigureCultureInfoViewModel
    {
        public List<SelectListItem> AvailableCurrencySymbols { get; }

        public List<SelectListItem> AvailableLanguages { get; }

        public List<SelectListItem> AvailableLongDateFormats { get; }

        public List<SelectListItem> AvailableShortDateFormats { get; }
        public string CurrencySymbol { get; set; }
        public string Language { get; set; }
        public string LongDateFormat { get; set; }
        public string ShortDateFormat { get; set; }

        public ConfigureCultureInfoViewModel()
        {
            AvailableCurrencySymbols = new List<SelectListItem>
            {
                // Source code omitted for brevity. 
            };

            AvailableLanguages = new List<SelectListItem>
            {
                new SelectListItem { Value="pt-BR", Text="Brasil" },
                new SelectListItem { Value="en-US", Text="Estados Unidos" }
            };

            AvailableLongDateFormats = new List<SelectListItem>
            {
                // Source code omitted for brevity. 
            };

            AvailableShortDateFormats = new List<SelectListItem>
            {
                // Source code omitted for brevity. 
            };
        }
    }
}
