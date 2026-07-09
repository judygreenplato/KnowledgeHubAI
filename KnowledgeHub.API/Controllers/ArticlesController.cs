
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    
   
    private readonly IArticleService _articleService;
    public ArticlesController( IArticleService articleService)
    {
       
        _articleService = articleService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateArticle(
    CreateArticleRequest request)
    {
        var result =
         await _articleService.CreateAsync(request);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetArticles(int page = 1,
    int pageSize = 10, string? search = null)
    {
        var articles =
          await _articleService
              .GetPublishedArticlesAsync(
                  page,
                  pageSize,
                  search);

        return Ok(articles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetArticle(Guid id)
    {
        var article =
        await _articleService
            .GetByIdAsync(id);

        return Ok(article);
    }
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateArticle(
    Guid id,
    UpdateArticleRequest request)
    {
        var article =
         await _articleService
             .UpdateAsync(id, request);

        return Ok(article);
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteArticle(Guid id)
    {
      await _articleService.DeleteAsync(id);

        return Ok(new
        {
            Message = "Article deleted successfully"
        });
    }

    [HttpPost("{id}/publish")]
    [Authorize]
    public async Task<IActionResult> PublishArticle(Guid id)
    {
        await _articleService.PublishAsync(id);

        return Ok(new
        {
            Message = "Article published successfully"
        });
    }
    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMyArticles()
    {


        var articles =
        await _articleService.GetMyArticlesAsync();

        return Ok(articles);
    }

}
