using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SearchController : ControllerBase
{
    private readonly SemanticSearchService _semanticSearchService;

    public SearchController(
        SemanticSearchService semanticSearchService)
    {
        _semanticSearchService = semanticSearchService;
    }

    [HttpPost]
    public async Task<IActionResult>
        Search(
            QuestionRequest request)
    {
        var result =
            await _semanticSearchService
                .SearchAsync(
                    request.Question);

        return Ok(result);
    }
}