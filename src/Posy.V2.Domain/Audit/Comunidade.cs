using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class ComunidadeAudit : Auditable
    {
        public Guid ComunidadeAuditId { get; private set; } = Guid.NewGuid();

        public Guid ComunidadeId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public string Alias { get; private set; }
        public string Nome { get; private set; }
        public TipoComunidade TipoComunidade { get; private set; }
        public int CategoriaId { get; private set; }
        public byte[] DescricaoPerfil { get; private set; }
        public DateTime Dir { get; set; }
        public Guid Uar { get; set; }
        public DateTime Dar { get; set; }
        public Guid? Uer { get; set; }
        public DateTime? Der { get; set; }
    }
}
