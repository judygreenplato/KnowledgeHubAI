using KnowledgeHub.Application.DTOs;

namespace KnowledgeHub.Application.Interfaces;

public interface ISemanticSearchService
{
    Task<List<SearchResultDto>> SearchAsync(
        string question);
}
