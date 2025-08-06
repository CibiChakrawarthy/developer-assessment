using System;
using System.Data;

namespace csharp.Models
{
    public class AuditEntry
    {
        public string? Action { get; set; }
        public string? Item { get; set; }
        public DateTime? Timestamp { get; set; }
        public string performedBy { get; set; }

    }
}
