using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;

namespace KnowledgeHub.Infrastructure.Services;

public class RagService
{
    private readonly
        SemanticSearchService
        _semanticSearchService;

    private readonly
        IOpenAIService
        _openAIService;

    public RagService(
        SemanticSearchService
            semanticSearchService,

        IOpenAIService
            openAIService)
    {
        _semanticSearchService =
            semanticSearchService;

        _openAIService =
            openAIService;
    }

    public async Task<string>
        AskAsync(
            string question)
    {
        var chunks =
            await _semanticSearchService
                .SearchAsync(
                    question);

        var context =
            string.Join(
                Environment.NewLine,
                chunks.Select(
                    x => x.Content));

        return await
            _openAIService
                .GenerateAnswerAsync(
                    question,
                    context);
    }
}