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
    private readonly IPythonAgentService _pythonAgentService;

    public ChatController(IPythonAgentService pythonAgentService)
    {
        _pythonAgentService = pythonAgentService;
    }

    [HttpPost]
    public async Task<IActionResult>
        Ask(
            ChatRequest request)
    {
        var response =
    await _pythonAgentService
        .AskAsync(request.Question);

        return Ok(response);
    }
}