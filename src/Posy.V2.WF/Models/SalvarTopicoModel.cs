using Posy.V2.Domain.Enums;

namespace Posy.V2.WF.Models
{
    public class SalvarTopicoModel
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public TipoTopico Fixo { get; set; }
    }
}