using csharp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Materialise.Candidate.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<string> _items = new();
        private static readonly List<AuditEntry> _auditTrail = new();

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetItems()
        {
            return Ok(_items.ToList());
        }

        [HttpPost]
        public ActionResult<string> AddItem([FromBody] string item)
        {
            var trimmedItem = item?.Trim();
            _items.Add(trimmedItem);
            _auditTrail.Add(new AuditEntry
            {
                Action = "Add",
                Item = trimmedItem,
                Timestamp = DateTime.UtcNow,
                performedBy = "TestUser"

            });
            return CreatedAtAction(nameof(GetItems), $"Item '{trimmedItem}' added successfully");
        }

        [HttpDelete]
        public ActionResult<string> DeleteItem([FromBody] string item)
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
                        performedBy = "TestUser"
                    });

                    break;
                }
            }
            return Ok($"Item '{item}' deleted successfully");
        }
        [HttpGet("audit")]
        public ActionResult<IEnumerable<AuditEntry>> GetAuditTrail([FromHeader] string role)
        {
            if (role != "Admin")
                return Unauthorized("Only admins can access the audit trail.");

            return Ok(_auditTrail);
        }
    }
}
