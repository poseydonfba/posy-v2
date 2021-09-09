using System.Collections.Generic;

namespace Posy.V2.Domain.Entities
{
    public class Categoria : EntityBase
    {
        public virtual string Nome { get; set; }

        public virtual ICollection<Comunidade> Comunidades { get; set; }

        public Categoria()
        {
        }
    }
}
