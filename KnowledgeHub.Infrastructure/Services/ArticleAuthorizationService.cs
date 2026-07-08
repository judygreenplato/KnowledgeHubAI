using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Domain.Entities;

namespace KnowledgeHub.Infrastructure.Services;

public class ArticleAuthorizationService
    : IArticleAuthorizationService
{
    private readonly ICurrentUserService
        _currentUserService;

    public ArticleAuthorizationService(
        ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public bool CanModify(Article article)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return false;
        }

        return article.CreatedByUserId ==
                   _currentUserService.UserId
               || _currentUserService.IsAdmin;
    }
}