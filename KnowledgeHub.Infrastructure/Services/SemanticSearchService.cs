using System.Text.Json;
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Infrastructure.Services;

public class SemanticSearchService
{
    private readonly AppDbContext _dbContext;

    private readonly IOpenAIService _openAIService;

    private readonly SimilarityService _similarityService;

    public SemanticSearchService(
        AppDbContext dbContext,
        IOpenAIService openAIService,
        SimilarityService similarityService)
    {
        _dbContext = dbContext;

        _openAIService = openAIService;

        _similarityService =
            similarityService;
    }

    public async Task<List<SearchResultDto>>
        SearchAsync(
            string question)
    {
        var questionEmbedding =
            await _openAIService
                .CreateEmbeddingAsync(
                    question);

        var embeddings =
            await _dbContext
                .DocumentEmbeddings
                .Include(x => x.DocumentChunk)
                .ToListAsync();

        var embeddingCount = embeddings.Count;

        var results =
            new List<SearchResultDto>();

        foreach (var embedding
                 in embeddings)
        {
            var chunkEmbedding =
                JsonSerializer
                    .Deserialize<List<float>>(
                        embedding.EmbeddingJson)!;

            var score =
                _similarityService
                    .CosineSimilarity(
                        questionEmbedding,
                        chunkEmbedding);

            results.Add(
                new SearchResultDto
                {
                    Content =
                        embedding
                            .DocumentChunk
                            .Content,

                    Score = score
                });
        }

        return results
            .OrderByDescending(
                x => x.Score)
            .Take(3)
            .ToList();
    }
}