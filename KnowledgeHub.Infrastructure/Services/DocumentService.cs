using AutoMapper;
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;

namespace KnowledgeHub.Infrastructure.Services;

public class DocumentService : IDocumentService
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly PdfTextExtractor _pdfTextExtractor;

    public DocumentService(
        AppDbContext dbContext,
        ICurrentUserService currentUserService,
        IMapper mapper,
        PdfTextExtractor pdfTextExtractor)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _pdfTextExtractor = pdfTextExtractor;
    }

    public async Task<DocumentResponse>
        UploadAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new Exception("No file uploaded.");
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

        await _dbContext.SaveChangesAsync();

       

        return _mapper.Map<DocumentResponse>(document);
    }
}