using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RagController : ControllerBase
{
    private readonly ISemanticSearchService _semanticSearchService;

    public RagController(
        ISemanticSearchService semanticSearchService)
    {
        _semanticSearchService = semanticSearchService;
    }

    [HttpPost("context")]
    public async Task<ActionResult<ContextResponse>> GetContext(
        ContextRequest request)
    {
        var results =
            await _semanticSearchService.SearchAsync(request.Question);

        var response = new ContextResponse
        {
            Chunks = results
                .Select(x => new ContextChunkDto
                {
                    Content = x.Content,
                    FileName = x.FileName
                })
                .ToList()
        };

        return Ok(response);
    }
}