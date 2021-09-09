using System;

namespace Posy.V2.Domain.Audit
{
    public class Auditable
    {
        public DateTime AuditStartDate { get; set; }
        public string AuditStartOperation { get; set; }
        public Guid AuditStartUserID { get; set; }
        public string AuditStartUsername { get; set; }
        public Guid AuditStartTransactionGUID { get; set; }

        public DateTime AuditEndDate { get; set; }
        public string AuditEndOperation { get; set; }
        public Guid AuditEndUserID { get; set; }
        public string AuditEndUsername { get; set; }
        public Guid AuditEndTransactionGUID { get; set; }

        public string ChangedColumns { get; set; }
    }
}
