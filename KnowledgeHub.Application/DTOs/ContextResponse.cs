using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KnowledgeHub.Application.DTOs;

public class ContextResponse
{
    public List<ContextChunkDto> Chunks { get; set; } = new();
}