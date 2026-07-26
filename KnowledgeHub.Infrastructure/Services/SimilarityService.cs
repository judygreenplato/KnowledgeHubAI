using KnowledgeHub.Application.Interfaces;

namespace KnowledgeHub.Infrastructure.Services;

public class SimilarityService : ISimilarityService
{
    public double CosineSimilarity(
        List<float> vectorA,
        List<float> vectorB)
    {
        if (vectorA == null)
        {
            throw new ArgumentNullException(
                nameof(vectorA));
        }

        if (vectorB == null)
        {
            throw new ArgumentNullException(
                nameof(vectorB));
        }

        if (vectorA.Count != vectorB.Count)
        {
            throw new ArgumentException(
                "Vectors must have the same dimensions.");
        }

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

        if (magnitudeA == 0 ||
        magnitudeB == 0)
        {
            return 0;
        }

       
       

        return dotProduct /
               (
                   Math.Sqrt(magnitudeA)
                   *
                   Math.Sqrt(magnitudeB)
               );
    }
}