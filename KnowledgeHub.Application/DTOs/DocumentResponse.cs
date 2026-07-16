using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeHub.Application.DTOs;

public class DocumentResponse
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;
    public string ExtractedText { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime UploadedAtUtc { get; set; }



}