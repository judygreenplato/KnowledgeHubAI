namespace KnowledgeHub.Application.DTOs;

public class SearchResultDto
{
    public string Content { get; set; }
        = string.Empty;
    public string FileName { get; set; }
        = string.Empty;

    public double Score { get; set; }
}