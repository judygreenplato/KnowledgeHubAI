using KnowledgeHub.Application.DTOs;

namespace KnowledgeHub.Application.Interfaces;



    public interface IRagService
    {
        Task<ChatResponse>AskAsync(string question);
    }

