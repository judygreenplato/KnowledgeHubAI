using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController
    : ControllerBase
{
    private readonly
        RagService
        _ragService;

    public ChatController(
        RagService ragService)
    {
        _ragService =
            ragService;
    }

    [HttpPost]
    public async Task<IActionResult>
        Ask(
            ChatRequest request)
    {
        var answer =
            await _ragService
                .AskAsync(
                    request.Question);

        return Ok(
            new ChatResponse
            {
                Answer = answer
            });
    }
}