using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Personal_Blogging_Platform_API.DTOs.Articles;
using Personal_Blogging_Platform_API.Services.Interfaces;

namespace Personal_Blogging_Platform_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string tag, [FromQuery] string title, [FromQuery] string author, [FromQuery] DateTime? publishedDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (items, total, current, size) = await _articleService.GetAllAsync(tag, title, author, publishedDate, pageNumber, pageSize);
            var resp = new
            {
                currentPage = current,
                pageSize = size,
                totalItems = total,
                totalPages = (int)Math.Ceiling(total / (double)size),
                items
            };
            return Ok(resp);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _articleService.GetByIdAsync(id);
            if (item == null) return NotFound(new { statusCode = 404, message = "Article not found", errors = new string[] { } });
            return Ok(item);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleCreateDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var article = await _articleService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = article.Id }, article);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ArticleUpdateDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role);
            var ok = await _articleService.UpdateAsync(id, dto, userId, role);
            if (!ok) return NotFound(new { statusCode = 404, message = "Article not found", errors = new string[] { } });
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role);
            var ok = await _articleService.DeleteAsync(id, userId, role);
            if (!ok) return NotFound(new { statusCode = 404, message = "Article not found", errors = new string[] { } });
            return NoContent();
        }
    }
}
