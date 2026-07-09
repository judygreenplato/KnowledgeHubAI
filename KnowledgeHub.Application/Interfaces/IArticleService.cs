using KnowledgeHub.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeHub.Application.Interfaces;

    public interface IArticleService
    {
        Task<ArticleResponse> CreateAsync(
            CreateArticleRequest request);
    Task<List<ArticleResponse>> GetPublishedArticlesAsync(
    int page,
    int pageSize,
    string? search);

    Task<ArticleResponse> GetByIdAsync(Guid id);
    Task<ArticleResponse> UpdateAsync(
    Guid articleId,
    UpdateArticleRequest request);

    Task PublishAsync(Guid articleId);

    Task DeleteAsync(Guid articleId);

    Task<List<ArticleResponse>> GetMyArticlesAsync();
    }
