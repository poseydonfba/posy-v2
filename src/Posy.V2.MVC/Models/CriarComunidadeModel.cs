using Posy.V2.Domain.Entities;
using System.Web.Mvc;

namespace Posy.V2.MVC.Models
{
    public class CriarComunidadeModel
    {
        public string Nome { get; set; }
        public int CategoriaId { get; set; }

        [AllowHtml]
        public string DescricaoPerfil { get; set; }
    }
}