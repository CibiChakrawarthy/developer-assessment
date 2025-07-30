using Microsoft.AspNetCore.Mvc;

namespace Materialise.Candidate.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<string> _items = new();

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
                    break;
                }
            }
            return Ok($"Item '{item}' deleted successfully");
        }
    }
}
