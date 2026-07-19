using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeHub.API.Controllers;


[ApiController]
[Route("api/documents")]
[Authorize]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;

    public DocumentsController(
        IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult>
        Upload(IFormFile file)
    {
        var result =
            await _documentService
                .UploadAsync(file);

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<
    List<DocumentListItemDto>>>
    GetDocuments()
    {
        var documents =
            await _documentService
                .GetDocumentsAsync();

        return Ok(documents);
    }
}