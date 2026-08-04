using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeHub.Application.DTOs;

public class ContextChunkDto
{
    public string Content { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;
}
