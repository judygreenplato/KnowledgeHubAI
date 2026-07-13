using AutoMapper;
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KnowledgeHub.Application.Mappings;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<Article, ArticleResponse>();
    }
}