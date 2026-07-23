using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController
    : ControllerBase
{
    private readonly
        IRagService
        _ragService;

    public ChatController(
        IRagService ragService)
    {
        _ragService =
            ragService;
    }

    [HttpPost]
    public async Task<IActionResult>
        Ask(
            ChatRequest request)
    {
        var response =
            await _ragService
                .AskAsync(
                    request.Question);


        return Ok(response);
    }
}