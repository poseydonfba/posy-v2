using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Audit
{
    public class ConnectionAudit : Auditable
    {
        public string ConnectionId { get; set; }
        public int UsuarioId { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
        public DateTime DataConnected { get; set; }
        public DateTime? DataDisconnected { get; set; }
        public TipoDesconexao? TipoDesconexao { get; set; }
    }
}
