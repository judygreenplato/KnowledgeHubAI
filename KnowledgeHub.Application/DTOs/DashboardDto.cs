// DashboardDto.cs

namespace KnowledgeHub.Application.DTOs;

public class DashboardDto
{
    public int Articles { get; set; }

    public int Categories { get; set; }

    public int Documents { get; set; }

    public int Users { get; set; }
    public int Embeddings { get; set; }
}