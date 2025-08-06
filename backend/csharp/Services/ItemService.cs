using csharp.Interface;
using csharp.Models;

namespace csharp.Services
{
    public class ItemService : IItemService
    {
        private readonly List<string> _items = new();
        private readonly List<AuditEntry> _auditTrail = new();
        public IEnumerable<string> GetItems()
        {
            return _items.ToList();
        }
        public string AddItem(string item, string performedBy)
        {
            var trimmedItem = item?.Trim();
            _items.Add(trimmedItem);
            _auditTrail.Add(new AuditEntry
            {
                Action = "Add",
                Item = trimmedItem,
                Timestamp = DateTime.UtcNow,
                performedBy = performedBy
            });
            return $"Item '{trimmedItem}' added successfully";
        }
        public string DeleteItem(string item, string performedBy)
        {
            var found = false;
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] == item)
                {
                    _items.RemoveAt(i);
                    found = true;
                    _auditTrail.Add(new AuditEntry
                    {
                        Action = "Delete",
                        Item = item,
                        Timestamp = DateTime.UtcNow,
                        performedBy = performedBy
                    });
                    break;
                }
            }
            return found ? $"Item '{item}' deleted successfully" : $"Item '{item}' not found";
        }
        public IEnumerable<AuditEntry> GetAuditTrail(string role)
        {
            if (role != "Admin")
                return Enumerable.Empty<AuditEntry>(); 

            return _auditTrail;
        }
    }
}