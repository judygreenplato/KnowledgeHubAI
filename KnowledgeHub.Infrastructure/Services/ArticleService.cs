using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using KnowledgeHub.Application.Exceptions;
using  AutoMapper;

namespace KnowledgeHub.Infrastructure.Services;

public class ArticleService : IArticleService
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IArticleAuthorizationService _authorizationService;
    private readonly IMapper _mapper;
    public ArticleService(
        AppDbContext dbContext,
        ICurrentUserService currentUserService,
        IArticleAuthorizationService authorizationService,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
        _mapper = mapper;
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
            Summary = GenerateSummary(request.Content,request.Summary),
            CategoryId = request.CategoryId,
            CreatedByUserId = userId,
            CreatedAtUtc = DateTime.UtcNow,
            IsPublished = false
        };

        _dbContext.Articles.Add(article);

        await _dbContext.SaveChangesAsync();

       return _mapper.Map<ArticleResponse>(article);
    }

    public async Task<List<ArticleResponse>>
        GetMyArticlesAsync()
    {
        var userId =
            _currentUserService.UserId!.Value;

       var articles= await _dbContext.Articles
            .Where(a => a.CreatedByUserId == userId)
            .OrderByDescending(a => a.CreatedAtUtc)
            .ToListAsync();
        return _mapper.Map<List<ArticleResponse>>(articles);
            
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

        var articles = await query
             .OrderByDescending(a => a.CreatedAtUtc)
             .Skip((page - 1) * pageSize)
             .Take(pageSize).ToListAsync();
        return _mapper.Map<List<ArticleResponse>>(
    articles);

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

        return _mapper.Map<ArticleResponse>(
     article);
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
        article.Summary = GenerateSummary(request.Content,request.Summary);
        article.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ArticleResponse>(article);
    }
    private static string GenerateSummary(
    string content,
    string? summary)
    {
        if (!string.IsNullOrWhiteSpace(summary))
        {
            return summary;
        }

        return content.Substring(
            0,
            Math.Min(100, content.Length));
    }
}