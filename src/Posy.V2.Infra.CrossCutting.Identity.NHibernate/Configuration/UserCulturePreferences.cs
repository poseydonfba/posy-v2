using Newtonsoft.Json;

namespace Posy.V2.Infra.CrossCutting.Identity.Configuration
{
    // https://www.jerriepelser.com/blog/allowing-user-to-set-culture-settings-aspnet5-part2/
    public class UserCulturePreferences
    {
        [JsonProperty("c")]
        public string CurrencySymbol { get; set; }
        [JsonProperty("l")]
        public string Language { get; set; }
        [JsonProperty("ld")]
        public string LongDateFormat { get; set; }
        [JsonProperty("sd")]
        public string ShortDateFormat { get; set; }
    }
}
