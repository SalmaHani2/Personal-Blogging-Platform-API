using System.ComponentModel.DataAnnotations;
namespace Personal_Blogging_Platform_API.DTOs.Articles
{
    public class ArticleUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        public List<string> Tags { get; set; } = new();
    }
}
