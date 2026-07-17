using AutoMapper;
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Infrastructure.Services;

public class DocumentService : IDocumentService
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly PdfTextExtractor _pdfTextExtractor;
    private readonly ChunkingService _chunkingService;

    public DocumentService(
        AppDbContext dbContext,
        ICurrentUserService currentUserService,
        IMapper mapper,
        PdfTextExtractor pdfTextExtractor,
        ChunkingService chunkingService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _pdfTextExtractor = pdfTextExtractor;
        _chunkingService = chunkingService;
    }

    public async Task<DocumentResponse>
        UploadAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No file uploaded.");
        }

        var uploadsFolder =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads");

        Directory.CreateDirectory(uploadsFolder);

        var storedFileName =
            $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var filePath =
            Path.Combine(
                uploadsFolder,
                storedFileName);
        using (var stream =
              new FileStream(
                  filePath,
                  FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var extractedText = string.Empty;

        if (file.ContentType == "application/pdf")
        {
            extractedText =
                _pdfTextExtractor.ExtractText(filePath);
        }
        

        var document = new Document
        {
            Id = Guid.NewGuid(),
            FileName = file.FileName,
            StoredFileName = storedFileName,
            ContentType = file.ContentType,
            ExtractedText = extractedText,
            FileSize = file.Length,
            UploadedAtUtc = DateTime.UtcNow,
            UploadedByUserId =
                _currentUserService.UserId!.Value
        };

        _dbContext.Documents.Add(document);
        var chunks =
    _chunkingService.CreateChunks(extractedText);

        int index = 0;

        foreach (var chunk in chunks)
        {
            _dbContext.DocumentChunks.Add(
                new DocumentChunk
                {
                    Id = Guid.NewGuid(),
                    DocumentId = document.Id,
                    ChunkIndex = index++,
                    Content = chunk
                });
        }

        await _dbContext.SaveChangesAsync();

        var query = _dbContext.DocumentChunks.ToListAsync();

        return _mapper.Map<DocumentResponse>(document);
    }
}