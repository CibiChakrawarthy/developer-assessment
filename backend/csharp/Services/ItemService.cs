using csharp.Interface;
using csharp.Models;

namespace csharp.Services
{
    public class ItemService : IItemService
    {
        // In-memory list to store item names
        private readonly List<string> _items = new();

        // In-memory list to store audit logs
        private readonly List<AuditEntry> _auditTrail = new();

        /// <summary>
        /// Retrieves all items currently in the list.
        /// </summary>
        public IEnumerable<string> GetItems()
        {
            return _items.ToList();
        }

        /// <summary>
        /// Adds a new item to the list and creates an audit entry for it.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="performedBy">User who performed the action</param>

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

        /// <summary>
        /// Deletes the given item and creates an audit entry if found.
        /// </summary>
        /// <param name="item">The item to delete</param>
        /// <param name="performedBy">User who performed the action</param>
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

        /// <summary>
        /// Retrieves the audit trail only if the role is Admin.
        /// </summary>
        /// <param name="role">Role of the requesting user</param>
        public IEnumerable<AuditEntry> GetAuditTrail(string role)
        {
            if (role != "Admin")
                return Enumerable.Empty<AuditEntry>(); 

            return _auditTrail;
        }
    }
}