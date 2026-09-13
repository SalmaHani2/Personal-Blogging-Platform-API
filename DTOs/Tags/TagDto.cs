using System.ComponentModel.DataAnnotations;

namespace Personal_Blogging_Platform_API.DTOs.Tags
{
    public class TagDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
