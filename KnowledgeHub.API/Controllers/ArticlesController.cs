
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ArticlesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateArticle(
    CreateArticleRequest request)
    {
        var userIdClaim =
            User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        var article = new Article
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            CreatedAtUtc = DateTime.UtcNow,
            CreatedByUserId = Guid.Parse(userIdClaim.Value)
        };

        _dbContext.Articles.Add(article);

        await _dbContext.SaveChangesAsync();

        return Ok(new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            CreatedAtUtc = article.CreatedAtUtc
        });
    }
}
