using csharp.Models;
using csharp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Materialise.Candidate.Backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(ItemService itemService,ILogger<ItemsController> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetItems()
        {
            try
            {
                return Ok(_itemService.GetItems());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get items.");
                return StatusCode(500, $"Error getting items: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<string> AddItem([FromBody] string item)
        {
            try
            {
                var result = _itemService.AddItem(item);
                return CreatedAtAction(nameof(GetItems), result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add item.");
                return StatusCode(500, $"Error adding item: {ex.Message}");
            }
        }

        [HttpDelete]
        public ActionResult<string> DeleteItem([FromBody] string item)
        {
            try
            {
                var result = _itemService.DeleteItem(item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete item.");
                return StatusCode(500, $"Error deleting item: {ex.Message}");
            }
        }

        [HttpGet("audit")]
        public ActionResult<IEnumerable<AuditEntry>> GetAuditTrail([FromHeader] string role)
        {
            try
            {
                if (role != "Admin")
                    return Unauthorized("Only admins can access the audit trail.");

                return Ok(_itemService.GetAuditTrail(role));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve audit trail.");
                return StatusCode(500, $"Error getting audit trail: {ex.Message}");
            }
        }
    }
}
