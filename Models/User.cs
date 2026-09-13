using System.ComponentModel.DataAnnotations;
namespace Personal_Blogging_Platform_API.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = "User";

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
