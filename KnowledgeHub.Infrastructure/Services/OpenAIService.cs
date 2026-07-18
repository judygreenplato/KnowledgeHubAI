using KnowledgeHub.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Embeddings;
using System.Linq;

public class OpenAIService
    : IOpenAIService
{
    private readonly OpenAIClient _client;

    public OpenAIService(
        IConfiguration configuration)
    {
        _client =
            new OpenAIClient(
                configuration["OpenAI:ApiKey"]);
    }

    public async Task<List<float>>
        CreateEmbeddingAsync(
            string text)
    {
        var embeddingClient =
            _client.GetEmbeddingClient(
                "text-embedding-3-small");

        var result =
            await embeddingClient
                .GenerateEmbeddingAsync(
                    text);

        var embedding =
     result.Value.ToFloats();

        return embedding
            .ToArray()
            .ToList();
    }
}