using System.ComponentModel.DataAnnotations;

namespace csharp.Models
{
    public class ItemRequestDto
    {
       
            [Required]
            public string? Value { get; set; }
        
    }
}
