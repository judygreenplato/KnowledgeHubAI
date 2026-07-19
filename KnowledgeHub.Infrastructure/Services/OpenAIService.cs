using KnowledgeHub.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Embeddings;
using System.Linq;
using OpenAI.Chat;

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
    public async Task<string>
    GenerateAnswerAsync(
        string question,
        string context)
{
    var chatClient =
        _client.GetChatClient(
            "gpt-4o-mini");

    var messages =
        new List<ChatMessage>
        {
            ChatMessage.CreateSystemMessage(
                """
                You are a helpful assistant.

                Answer ONLY using the
                provided context.

                If the answer is not
                available in the context,
                say:

                "I could not find that
                information in the documents."
                """
            ),

            ChatMessage.CreateUserMessage(
                $"""
                Context:

                {context}

                Question:

                {question}
                """
            )
        };

    var result =
        await chatClient
            .CompleteChatAsync(
                messages);

    return result
        .Value
        .Content[0]
        .Text;
}
}