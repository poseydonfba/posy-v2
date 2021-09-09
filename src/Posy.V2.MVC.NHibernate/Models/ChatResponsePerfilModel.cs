using Newtonsoft.Json;
using System;

namespace Posy.V2.MVC.Models
{
    public class ChatResponsePerfilModel
    {
        [JsonProperty("n")]
        public string Nome { get; set; }
        [JsonProperty("i")]
        public int Id { get; set; }
        [JsonProperty("c")]
        public int ConnectionId { get; set; }
        [JsonProperty("f")]
        public string Frase { get; set; }
    }
}