using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeHub.Application.Interfaces;

public interface IOpenAIService
{
    Task<List<float>>
        CreateEmbeddingAsync(
            string text);
}