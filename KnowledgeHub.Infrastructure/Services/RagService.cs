using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using KnowledgeHub.Application.Exceptions;
using AutoMapper;

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

    public async Task<ChatResponse>
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

        var answer =
        await _openAIService
            .GenerateAnswerAsync(
                question,
                context);

        return new ChatResponse
        {
            Answer = answer,

            Sources =
                chunks
                    .Select(x => x.FileName)
                    .Distinct()
                    .ToList()
        };
        
    }
}