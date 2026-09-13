using System.ComponentModel.DataAnnotations;
namespace Personal_Blogging_Platform_API.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
        public ICollection<Tag> Tags => ArticleTags?.Select(at => at.Tag).ToList() ?? new List<Tag>();
    }
}
