

namespace KnowledgeHub.Application.DTOs;

public class DocumentListItemDto
{
    public Guid Id { get; set; }

    public string FileName { get; set; }
        = string.Empty;

    public long FileSize { get; set; }

    public DateTime UploadedAtUtc { get; set; }
}