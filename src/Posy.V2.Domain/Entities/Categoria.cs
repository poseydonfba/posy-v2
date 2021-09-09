namespace Posy.V2.Domain.Entities
{
    public class Categoria : EntityBase
    {
        public int CategoriaId { get; set; }
        public string Nome { get; set; }

        public Categoria()
        {
        }
    }
}
