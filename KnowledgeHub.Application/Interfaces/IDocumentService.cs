using Microsoft.AspNetCore.Http;
using KnowledgeHub.Application.DTOs;

public interface IDocumentService
{
    Task<DocumentResponse> UploadAsync(
        IFormFile file);
}
