using csharp.Interface;
using csharp.Models;
using csharp.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace csharp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IItemService itemService, ILogger<ItemsController> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<string>>> GetItems()
        {
            try
            {
                var result = _itemService.GetItems();
                return Ok(new ApiResponse<IEnumerable<string>>
                {
                    Data = result,
                    Message = "Items retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get items.");
                return StatusCode(500, new ApiResponse<IEnumerable<string>>
                {
                    Data = null,
                    Message = $"Error getting items: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public ActionResult<ApiResponse<string>> AddItem([FromBody] ItemRequestDto request)
        {
            try
            {
                var performedBy = User.Identity?.Name ?? "Unknown";
                var result = _itemService.AddItem(request.Value, performedBy);

                return Ok(new ApiResponse<string>
                {
                    Data = result,
                    Message = $"Item added by {performedBy}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add item.");
                return StatusCode(500, new ApiResponse<string>
                {
                    Data = null,
                    Message = $"Error adding item: {ex.Message}"
                });
            }
        }

        [HttpDelete]
        public ActionResult<ApiResponse<string>> DeleteItem([FromBody] ItemRequestDto item)
        {
            try
            {
                var performedBy = User.Identity?.Name ?? "Unknown";
                var result = _itemService.DeleteItem(item.Value, performedBy);

                return Ok(new ApiResponse<string>
                {
                    Data = result,
                    Message = $"Item deleted by {performedBy}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete item.");
                return StatusCode(500, new ApiResponse<string>
                {
                    Data = null,
                    Message = $"Error deleting item: {ex.Message}"
                });
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("audit")]
        public ActionResult<ApiResponse<IEnumerable<AuditEntry>>> GetAuditTrail([FromHeader] string role)
        {
            try
            {
                var result = _itemService.GetAuditTrail("Admin");
                return Ok(new ApiResponse<IEnumerable<AuditEntry>>
                {
                    Data = result,
                    Message = "Audit trail fetched successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get audit trail.");
                return StatusCode(500, new ApiResponse<IEnumerable<AuditEntry>>
                {
                    Data = null,
                    Message = $"Error fetching audit trail: {ex.Message}"
                });
            }
        }
    }
}
