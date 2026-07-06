
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            CreatedByUserId = Guid.Parse(userIdClaim.Value),
            IsPublished = false,
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

    [HttpGet]
    public async Task<IActionResult> GetArticles(int page = 1,
    int pageSize = 10, string? search = null)
    {
        var query = _dbContext.Articles
    .Where(a => a.IsPublished)
    .AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(a =>
                a.Title.Contains(search) ||
                a.Content.Contains(search));
        }

        var totalCount = await query
    .CountAsync();

        var articles = await query
            .OrderByDescending(a => a.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new ArticleResponse
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                CreatedAtUtc = a.CreatedAtUtc
            })
            .ToListAsync();

        return Ok(articles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetArticle(Guid id)
    {
        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            return NotFound(new
            {
                Message = "Article not found"
            });
        }

        return Ok(new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            CreatedAtUtc = article.CreatedAtUtc
        });
    }
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateArticle(
    Guid id,
    UpdateArticleRequest request)
    {
        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            return NotFound(new
            {
                Message = "Article not found"
            });
        }

        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        var roleClaim =
            User.FindFirst(ClaimTypes.Role);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        var currentUserId =
            Guid.Parse(userIdClaim.Value);

        var currentUserRole =
            roleClaim?.Value;

        var isOwner =
            article.CreatedByUserId == currentUserId;

        var isAdmin =
            currentUserRole == "Admin";

        if (!isOwner && !isAdmin)
        {
            return Forbid();
        }

        article.Title = request.Title;
        article.Content = request.Content;
        article.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return Ok(new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            CreatedAtUtc = article.CreatedAtUtc
        });
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteArticle(Guid id)
    {
        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            return NotFound(new
            {
                Message = "Article not found"
            });
        }

        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        var roleClaim =
            User.FindFirst(ClaimTypes.Role);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        var currentUserId =
            Guid.Parse(userIdClaim.Value);

        var currentUserRole =
            roleClaim?.Value;

        var isOwner =
            article.CreatedByUserId == currentUserId;

        var isAdmin =
            currentUserRole == "Admin";

        if (!isOwner && !isAdmin)
        {
            return Forbid();
        }

        _dbContext.Articles.Remove(article);

        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            Message = "Article deleted successfully"
        });
    }

    [HttpPost("{id}/publish")]
    [Authorize]
    public async Task<IActionResult> PublishArticle(Guid id)
    {
        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            return NotFound(new
            {
                Message = "Article not found"
            });
        }

        var userIdClaim =
            User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier);

        var roleClaim =
            User.FindFirst(
                System.Security.Claims.ClaimTypes.Role);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        var currentUserId =
            Guid.Parse(userIdClaim.Value);

        var currentUserRole =
            roleClaim?.Value;

        var isOwner =
            article.CreatedByUserId == currentUserId;

        var isAdmin =
            currentUserRole == "Admin";

        if (!isOwner && !isAdmin)
        {
            return Forbid();
        }

        article.IsPublished = true;
        article.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            Message = "Article published successfully"
        });
    }

}
