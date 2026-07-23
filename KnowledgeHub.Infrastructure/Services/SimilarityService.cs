using KnowledgeHub.Application.Interfaces;

namespace KnowledgeHub.Infrastructure.Services;

public class SimilarityService : ISimilarityService
{
    public double CosineSimilarity(
        List<float> vectorA,
        List<float> vectorB)
    {
        double dotProduct = 0;

        double magnitudeA = 0;

        double magnitudeB = 0;

        for (int i = 0; i < vectorA.Count; i++)
        {
            dotProduct +=
                vectorA[i] * vectorB[i];

            magnitudeA +=
                vectorA[i] * vectorA[i];

            magnitudeB +=
                vectorB[i] * vectorB[i];
        }

        return dotProduct /
               (
                   Math.Sqrt(magnitudeA)
                   *
                   Math.Sqrt(magnitudeB)
               );
    }
}