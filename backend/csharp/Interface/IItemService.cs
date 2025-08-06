using csharp.Models;

namespace csharp.Interface
{
    
        public interface IItemService
        {
            IEnumerable<string> GetItems();
            string AddItem(string item, string performedBy);
            string DeleteItem(string item, string performedBy);
            IEnumerable<AuditEntry> GetAuditTrail(string role);
        }
    
}
