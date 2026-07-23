using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SearchController : ControllerBase
{
    private readonly ISemanticSearchService _semanticSearchService;

    public SearchController(
        ISemanticSearchService semanticSearchService)
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