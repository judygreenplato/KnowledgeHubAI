using AutoMapper;
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Domain.Entities;

namespace KnowledgeHub.Application.Mappings;

public class DocumentProfile : Profile
{
    public DocumentProfile()
    {
        CreateMap<Document, DocumentResponse>();
    }
}
