using System.Web.Mvc;

namespace Posy.V2.MVC.Models
{
    public class EditarPerfilComunidadeModel
    {
        public string Alias { get; set; }
        public string Nome { get; set; }
        public int CategoriaId { get; set; }

        [AllowHtml]
        public string DescricaoPerfil { get; set; }
    }
}