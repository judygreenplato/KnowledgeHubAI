namespace KnowledgeHub.Application.Interfaces;

public interface ISimilarityService
{
    double CosineSimilarity(
        List<float> vectorA,
        List<float> vectorB);
}