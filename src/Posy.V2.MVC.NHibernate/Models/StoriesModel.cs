using Newtonsoft.Json;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Posy.V2.MVC.Models
{
    public class StoriesModel
    {
        //[JsonProperty("usuarioId")]
        public int UsuarioId { get; set; }
        //[JsonProperty("name")]
        public string Name { get; set; }
        //[JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }
        //[ScriptIgnore]
        //[JsonProperty("stories")]
        public List<StorieModel> Stories { get; set; }
    }

    public class StorieModel
    {
        public int StorieId { get; set; }
        public int UsuarioId { get; set; }
        public StorieType StorieType { get; set; }
        public int Length { get; set; }
        public string Src { get; set; }
        public string Preview { get; set; }
        public string Link { get; set; }
        public string LinkText { get; set; }
        public string Seen { get; set; }
        public string Time { get; set; }
        public DateTime DataStorie { get; set; }
    }
}