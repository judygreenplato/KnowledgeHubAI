using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeHub.Infrastructure.Services;

public class ChunkingService
{
    public List<string>
        CreateChunks(
            string text,
            int chunkSize = 1000)
    {
        var chunks = new List<string>();

        for (
            int i = 0;
            i < text.Length;
            i += chunkSize)
        {
            chunks.Add(
                text.Substring(
                    i,
                    Math.Min(
                        chunkSize,
                        text.Length - i)));
        }

        return chunks;
    }
}