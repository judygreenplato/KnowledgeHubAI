using KnowledgeHub.Application.DTOs;

namespace KnowledgeHub.Application.Interfaces;

public interface IPythonAgentService
{
    Task<ChatResponse> AskAsync(string question);
}
