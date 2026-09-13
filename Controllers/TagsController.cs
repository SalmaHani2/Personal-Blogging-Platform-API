using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Blogging_Platform_API.DTOs.Tags;
using Personal_Blogging_Platform_API.Services.Interfaces;

namespace Personal_Blogging_Platform_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAllAsync();
            return Ok(tags);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagDto dto)
        {
            var tag = await _tagService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { name = tag.Name }, tag);
        }
    }
}
