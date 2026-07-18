using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeHub.Domain.Entities;

public class DocumentEmbedding
{
    public Guid Id { get; set; }

    public Guid DocumentChunkId { get; set; }

    public string EmbeddingJson { get; set; }
        = string.Empty;
}