namespace Personal_Blogging_Platform_API.DTOs.Articles
{
    public class ArticleResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int AuthorId { get; set; }
        public string AuthorUsername { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
