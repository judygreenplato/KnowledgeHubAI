

using System.Security.Claims;
using KnowledgeHub.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace KnowledgeHub.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var value = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            return Guid.TryParse(value, out var id)
                ? id
                : null;
        }
    }

    public string? Email =>
        _httpContextAccessor
            .HttpContext?
            .User
            .FindFirst(ClaimTypes.Email)?
            .Value;

    public bool IsAuthenticated =>
        _httpContextAccessor
            .HttpContext?
            .User
            .Identity?
            .IsAuthenticated ?? false;

    public bool IsAdmin =>
        _httpContextAccessor
            .HttpContext?
            .User
            .IsInRole("Admin") ?? false;
}
