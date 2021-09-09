using Newtonsoft.Json;
using System;

namespace Posy.V2.MVC.Models
{
    public class ChatResponsePerfilModel
    {
        [JsonProperty("n")]
        public string Nome { get; set; }
        [JsonProperty("i")]
        public Guid Id { get; set; }
        [JsonProperty("c")]
        public string ConnectionId { get; set; }
        [JsonProperty("f")]
        public string Frase { get; set; }
    }
}