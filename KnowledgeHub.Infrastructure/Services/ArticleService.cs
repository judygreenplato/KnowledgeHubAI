using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using KnowledgeHub.Application.Exceptions;

namespace KnowledgeHub.Infrastructure.Services;

public class ArticleService : IArticleService
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IArticleAuthorizationService _authorizationService;
    public ArticleService(
        AppDbContext dbContext,
        ICurrentUserService currentUserService,
        IArticleAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<ArticleResponse> CreateAsync(
        CreateArticleRequest request)
    {
       
        var userId =
            _currentUserService.UserId!.Value;

        var article = new Article
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            CategoryId = request.CategoryId,
            CreatedByUserId = userId,
            CreatedAtUtc = DateTime.UtcNow,
            IsPublished = false
        };

        _dbContext.Articles.Add(article);

        await _dbContext.SaveChangesAsync();

        return new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            CreatedAtUtc = article.CreatedAtUtc
        };
    }

    public async Task<List<ArticleResponse>>
        GetMyArticlesAsync()
    {
        var userId =
            _currentUserService.UserId!.Value;

        return await _dbContext.Articles
            .Where(a => a.CreatedByUserId == userId)
            .OrderByDescending(a => a.CreatedAtUtc)
            .Select(a => new ArticleResponse
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                IsPublished=a.IsPublished,
                CategoryId=a.CategoryId,
                CreatedAtUtc = a.CreatedAtUtc
            })
            .ToListAsync();
    }

    public async Task PublishAsync(Guid articleId)
    {
        var article = await _dbContext.Articles
             .FirstOrDefaultAsync(a => a.Id == articleId);

        if (article == null)
        {
            throw new NotFoundException("Article not found");
        }


        if (!_authorizationService
         .CanModify(article))
        {
            throw new ForbiddenException("You cannot modify this article");
        }

        article.IsPublished = true;
        article.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

       
    }

    public async Task DeleteAsync(Guid articleId)
    {
        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.Id == articleId);

        if (article == null)
        {
            throw new NotFoundException("Article not found");
        }

        if (!_authorizationService.CanModify(article))
        {
            throw new ForbiddenException("You cannot modify this article");
        }

        _dbContext.Articles.Remove(article);

        await _dbContext.SaveChangesAsync();
    }
    public async Task<List<ArticleResponse>>
    GetPublishedArticlesAsync(
        int page,
        int pageSize,
        string? search)
    {
        var query = _dbContext.Articles
            .Where(a => a.IsPublished);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(a =>
                a.Title.Contains(search) ||
                a.Content.Contains(search));
        }

        return await query
            .OrderByDescending(a => a.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new ArticleResponse
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                IsPublished = a.IsPublished,
                CategoryId = a.CategoryId,
                CreatedAtUtc = a.CreatedAtUtc
            })
            .ToListAsync();
    }
    public async Task<ArticleResponse>
    GetByIdAsync(Guid articleId)
    {
        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.Id == articleId &&
            a.IsPublished);

        if (article == null)
        {
            throw new NotFoundException("Article not found");
        }

        return new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            IsPublished = article.IsPublished,
            CategoryId = article.CategoryId,
            CreatedAtUtc = article.CreatedAtUtc
        };
    }
    public async Task<ArticleResponse> UpdateAsync(
    Guid articleId,
    UpdateArticleRequest request)
    {
        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.Id == articleId);

        if (article == null)
        {
            throw new NotFoundException("Article not found");
        }

        if (!_authorizationService
            .CanModify(article))
        {
            throw new ForbiddenException("You cannot modify this article");
        }

        article.Title = request.Title;
        article.Content = request.Content;
        article.CategoryId = request.CategoryId;
        article.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            CategoryId = article.CategoryId,
            CreatedAtUtc = article.CreatedAtUtc
            
        };
    }
}