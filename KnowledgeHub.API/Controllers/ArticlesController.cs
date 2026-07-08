
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace KnowledgeHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IArticleAuthorizationService _articleAuthorizationService;
    private readonly ICurrentUserService _currentUserService;
    public ArticlesController(AppDbContext dbContext, ICurrentUserService currentUserService,IArticleAuthorizationService articleAuthorizationService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _articleAuthorizationService = articleAuthorizationService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateArticle(
    CreateArticleRequest request)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return Unauthorized();
        }

        var userId = _currentUserService.UserId!.Value;

        var article = new Article
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            CreatedAtUtc = DateTime.UtcNow,
            CreatedByUserId = userId,
            CategoryId = request .CategoryId,
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

       

        if (!_articleAuthorizationService
         .CanModify(article))
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



        if (!_articleAuthorizationService
        .CanModify(article))
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


        if (!_articleAuthorizationService
         .CanModify(article))
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
    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMyArticles()
    {
 

        if (!_currentUserService.IsAuthenticated)
        {
            return Unauthorized();
        }
        var userId = _currentUserService.UserId!.Value;
        var articles = await _dbContext.Articles
            .Where(a => a.CreatedByUserId == userId)
            .OrderByDescending(a => a.CreatedAtUtc)
            .Select(a => new
            {
                a.Id,
                a.Title,
                a.Content,
                a.IsPublished,
                a.CreatedAtUtc,
                a.UpdatedAtUtc
            })
            .ToListAsync();

        return Ok(articles);
    }

}
