using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class TopicoAudit : Auditable
    {
        public int TopicoId { get; private set; }
        public int ComunidadeId { get; private set; }
        public int UsuarioId { get; private set; }
        public DateTime DataTopico { get; private set; }
        public string Titulo { get; private set; }
        public byte[] Descricao { get; private set; }
        public TipoTopico TipoTopico { get; private set; }
        public int? Uer { get; private set; }
        public DateTime? Der { get; private set; }
        public int? Uerp { get; private set; }
        public DateTime? Derp { get; private set; }
    }
}
