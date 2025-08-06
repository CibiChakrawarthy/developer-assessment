using csharp.Interface;
using csharp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace csharp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IItemService itemService,ILogger<ItemsController> logger)
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
        public ActionResult<string> AddItem([FromBody] ItemRequestDto request)
        {
            try            
            {
                var performedBy = User.Identity?.Name ?? "Unknown";
                var result = _itemService.AddItem(request.Value, performedBy);
                return CreatedAtAction(nameof(GetItems), result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add item.");
                return StatusCode(500, $"Error adding item: {ex.Message}");
            }
        }

        [HttpDelete]
        public ActionResult<string> DeleteItem([FromBody] ItemRequestDto item)
        {
            try
            {
                var performedBy = User.Identity?.Name ?? "Unknown";
                var result = _itemService.DeleteItem(item.Value, performedBy);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete item.");
                return StatusCode(500, $"Error deleting item: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("audit")]
        public ActionResult<IEnumerable<AuditEntry>> GetAuditTrail([FromHeader] string role)
        {
            return Ok(_itemService.GetAuditTrail("Admin"));
        }
    }
}
