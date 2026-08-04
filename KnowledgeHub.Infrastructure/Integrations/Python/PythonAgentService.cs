using System.Net.Http.Json;
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;

namespace KnowledgeHub.Infrastructure.Integrations.Python;

public class PythonAgentService : IPythonAgentService
{
    private readonly HttpClient _httpClient;

    public PythonAgentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ChatResponse> AskAsync(string question)
    {
        var request = new PythonChatRequest
        {
            Question = question
        };

        var response = await _httpClient.PostAsJsonAsync(
            "/chat",
            request);

        response.EnsureSuccessStatusCode();

        var pythonResponse =
            await response.Content.ReadFromJsonAsync<PythonChatResponse>();

        if (pythonResponse == null)
        {
            throw new Exception(
                "Python Agent returned an empty response.");
        }

        return new ChatResponse
        {
            Answer = pythonResponse.Answer,
            Sources = pythonResponse.Sources
        };
    }
}