using KnowledgeHub.Infrastructure.Services;

namespace KnowledgeHub.Tests.Services;

public class SimilarityServiceTests
{
    private readonly SimilarityService
       _service;

    public SimilarityServiceTests()
    {
        _service =
            new SimilarityService();
    }
    //Verify Cosine Similarity returns 1 for identical vectors
    [Fact]
    public void CosineSimilarity_SameVectors_ReturnsOne()
    {
        // Arrange

        var vector1 =
            new List<float>
            {
                1,
                2,
                3
            };

        var vector2 =
            new List<float>
            {
                1,
                2,
                3
            };

        // Act

        var result =
            _service.CosineSimilarity(
                vector1,
                vector2);

        // Assert

        Assert.True(
            result > 0.99f);
    }
    [Fact]
    public void CosineSimilarity_OrthogonalVectors_ReturnsZero()
    {
        // Arrange

        var vector1 =
            new List<float>
            {
            1,
            0
            };

        var vector2 =
            new List<float>
            {
            0,
            1
            };

        // Act

        var result =
            _service.CosineSimilarity(
                vector1,
                vector2);

        // Assert

        Assert.True(
            Math.Abs(result) < 0.0001f);
    }
    [Fact]
    public void CosineSimilarity_OppositeVectors_ReturnsMinusOne()
    {
        // Arrange

        var vector1 =
            new List<float>
            {
            1,
            0
            };

        var vector2 =
            new List<float>
            {
            -1,
            0
            };

        // Act

        var result =
            _service.CosineSimilarity(
                vector1,
                vector2);

        // Assert

        Assert.True(
            result < -0.99f);
    }
    [Fact]
    public void CosineSimilarity_EmptyVectors_ReturnsZero()
    {
        // Arrange

        var vector1 = new List<float>();

        var vector2 = new List<float>();

        // Act

        var result =
            _service.CosineSimilarity(
                vector1,
                vector2);

        // Assert

        Assert.Equal(0f, result);
    }
    [Fact]
    public void CosineSimilarity_ZeroVectors_ReturnsZero()
    {
        // Arrange

        var vector1 =
            new List<float>
            {
            0,
            0,
            0
            };

        var vector2 =
            new List<float>
            {
            0,
            0,
            0
            };

        // Act

        var result =
            _service.CosineSimilarity(
                vector1,
                vector2);

        // Assert

        Assert.Equal(0f, result);
    }
    [Fact]
    public void CosineSimilarity_DifferentDimensions_ThrowsException()
    {
        // Arrange

        var vector1 =
            new List<float>
            {
            1,
            2,
            3
            };

        var vector2 =
            new List<float>
            {
            1,
            2
            };

        // Act & Assert

        Assert.Throws<ArgumentException>(
            () =>
                _service.CosineSimilarity(
                    vector1,
                    vector2));
    }
    [Fact]
    public void CosineSimilarity_NullVectorA_ThrowsException()
    {
        // Arrange

        List<float>? vector1 = null;

        var vector2 =
            new List<float>
            {
            1,
            2,
            3
            };

        // Act & Assert

        Assert.Throws<ArgumentNullException>(
            () =>
                _service.CosineSimilarity(
                    vector1!,
                    vector2));
    }


    [Fact]
    public void CosineSimilarity_NullVectorB_ThrowsException()
    {
        // Arrange

        var vector1 =
            new List<float>
            {
            1,
            2,
            3
            };

        List<float>? vector2 = null;

        // Act & Assert

        Assert.Throws<ArgumentNullException>(
            () =>
                _service.CosineSimilarity(
                    vector1,
                    vector2!));
    }
    [Fact]
    public void CosineSimilarity_1536DimensionVectors_ReturnsOne()
    {
        // Arrange

        var vector1 =
            Enumerable
                .Repeat(1.0f, 1536)
                .ToList();

        var vector2 =
            Enumerable
                .Repeat(1.0f, 1536)
                .ToList();

        // Act

        var result =
            _service.CosineSimilarity(
                vector1,
                vector2);

        // Assert

        Assert.Equal(
            1.0,
            result,
            precision: 6);
    }
    [Fact]
    public void CosineSimilarity_LargeNumbers_ReturnsExpectedResult()
    {
        // Arrange

        var vector1 =
            new List<float>
            {
            1000000f,
            2000000f,
            3000000f
            };

        var vector2 =
            new List<float>
            {
            1000000f,
            2000000f,
            3000000f
            };

        // Act

        var result =
            _service.CosineSimilarity(
                vector1,
                vector2);

        // Assert

        Assert.Equal(
            1.0,
            result,
            precision: 6);
    }
    [Fact]
    public void CosineSimilarity_SmallNumbers_ReturnsExpectedResult()
    {
        // Arrange

        var vector1 =
            new List<float>
            {
            0.000001f,
            0.000002f,
            0.000003f
            };

        var vector2 =
            new List<float>
            {
            0.000001f,
            0.000002f,
            0.000003f
            };

        // Act

        var result =
            _service.CosineSimilarity(
                vector1,
                vector2);

        // Assert

        Assert.Equal(
            1.0,
            result,
            precision: 6);
    }
    [Fact]
    public void CosineSimilarity_SingleDimensionVectors_ReturnsOne()
    {
        // Arrange

        var vector1 =
            new List<float>
            {
            5
            };

        var vector2 =
            new List<float>
            {
            5
            };

        // Act

        var result =
            _service.CosineSimilarity(
                vector1,
                vector2);

        // Assert

        Assert.True(
            result > 0.99f);
    }
    [Fact]
    public void CosineSimilarity_DifferentEmbeddings_ReturnsValidRange()
    {
        // Arrange

        var vector1 =
            new List<float>
            {
            1,
            2,
            3
            };

        var vector2 =
            new List<float>
            {
            7,
            8,
            9
            };

        // Act

        var result =
            _service.CosineSimilarity(
                vector1,
                vector2);

        // Assert

        Assert.InRange(
            result,
            -1f,
            1f);
    }
}