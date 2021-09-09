using Posy.V2.Domain.Enums;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Connection : EntityBase
    {
        public virtual int UsuarioId { get; set; }
        public virtual string UserAgent { get; set; }
        public virtual bool Connected { get; set; }
        public virtual DateTime DataConnected { get; set; }
        public virtual DateTime? DataDisconnected { get; set; }
        public virtual TipoDesconexao? TipoDesconexao { get; set; }

        public virtual Usuario Usuario { get; set; }
    }

    
}
