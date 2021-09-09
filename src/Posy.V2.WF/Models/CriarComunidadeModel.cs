using Posy.V2.Domain.Entities;

namespace Posy.V2.WF.Models
{
    public class CriarComunidadeModel
    {
        public string Nome { get; set; }
        public int CategoriaId { get; set; }
        public string DescricaoPerfil { get; set; }
    }
}