using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class ComunidadeAudit : Auditable
    {
        public int ComunidadeId { get; private set; }
        public int UsuarioId { get; private set; }
        public string Alias { get; private set; }
        public string Nome { get; private set; }
        public TipoComunidade TipoComunidade { get; private set; }
        public int CategoriaId { get; private set; }
        public byte[] DescricaoPerfil { get; private set; }
        public DateTime Dir { get; set; }
        public int Uar { get; set; }
        public DateTime Dar { get; set; }
        public int? Uer { get; set; }
        public DateTime? Der { get; set; }
    }
}
