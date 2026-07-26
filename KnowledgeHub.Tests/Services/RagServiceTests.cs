using Moq;

using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Infrastructure.Services;

namespace KnowledgeHub.Tests.Services;

public class RagServiceTests
{
    private readonly
        Mock<ISemanticSearchService>
        _semanticSearchServiceMock;

    private readonly
        Mock<IOpenAIService>
        _openAIServiceMock;

    private readonly
        RagService
        _ragService;

    public RagServiceTests()
    {
        _semanticSearchServiceMock =
            new Mock<ISemanticSearchService>();

        _openAIServiceMock =
            new Mock<IOpenAIService>();

        _ragService =
            new RagService(
                _semanticSearchServiceMock.Object,
                _openAIServiceMock.Object);
    }

    [Fact]
    public async Task
    AskAsync_CallsSemanticSearchServiceOnce()
    {
        // Arrange

        _semanticSearchServiceMock
            .Setup(x =>
                x.SearchAsync(It.IsAny<string>()))
            .ReturnsAsync(
                new List<SearchResultDto>());

        _openAIServiceMock
            .Setup(x =>
                x.GenerateAnswerAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
            .ReturnsAsync("AI Answer");

        // Act

        await _ragService
            .AskAsync(
                "What is Dependency Injection?");

        // Assert

        _semanticSearchServiceMock
            .Verify(
                x => x.SearchAsync(
                    "What is Dependency Injection?"),
                Times.Once);
    }

  

    [Fact]
    public async Task AskAsync_CallsOpenAIWithCorrectQuestion()
    {
        // Arrange

        var question =
            "What is Dependency Injection?";

        var chunks =
            new List<SearchResultDto>
            {
            new SearchResultDto
            {
                Content = "Dependency Injection is a design pattern.",
                FileName = "dotnet.pdf",
                Score = 0.95
            }
            };

        _semanticSearchServiceMock
            .Setup(x => x.SearchAsync(question))
            .ReturnsAsync(chunks);

        _openAIServiceMock
            .Setup(x => x.GenerateAnswerAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync("AI Answer");

        // Act

        await _ragService.AskAsync(question);

        // Assert

        _openAIServiceMock.Verify(
            x => x.GenerateAnswerAsync(
                question,
                It.IsAny<string>()),
            Times.Once);
    }
    [Fact]
    public async Task AskAsync_ReturnsChatResponseCorrectly()
    {
        // Arrange

        var question =
            "What is Dependency Injection?";

        var chunks =
            new List<SearchResultDto>
            {
            new SearchResultDto
            {
                Content = "Dependency Injection reduces coupling.",
                FileName = "dotnet.pdf",
                Score = 0.95
            },

            new SearchResultDto
            {
                Content = "ASP.NET Core supports Dependency Injection.",
                FileName = "aspnet.pdf",
                Score = 0.90
            }
            };

        _semanticSearchServiceMock
            .Setup(x => x.SearchAsync(question))
            .ReturnsAsync(chunks);

        _openAIServiceMock
            .Setup(x => x.GenerateAnswerAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync("Dependency Injection is a design pattern.");

        // Act

        var result =
            await _ragService.AskAsync(question);

        // Assert

        Assert.NotNull(result);

        Assert.Equal(
            "Dependency Injection is a design pattern.",
            result.Answer);

        Assert.Equal(
            2,
            result.Sources.Count);

        Assert.Contains(
            "dotnet.pdf",
            result.Sources);

        Assert.Contains(
            "aspnet.pdf",
            result.Sources);
    }
    [Fact]
    public async Task AskAsync_ReturnsDistinctSources()
    {
        // Arrange

        var question =
            "What is Dependency Injection?";

        var chunks =
            new List<SearchResultDto>
            {
            new SearchResultDto
            {
                Content = "DI is a design pattern.",
                FileName = "dotnet.pdf",
                Score = 0.95
            },

            new SearchResultDto
            {
                Content = "Constructor Injection.",
                FileName = "dotnet.pdf",
                Score = 0.92
            },

            new SearchResultDto
            {
                Content = "Service Lifetimes.",
                FileName = "dependency.pdf",
                Score = 0.90
            }
            };

        _semanticSearchServiceMock
            .Setup(x => x.SearchAsync(question))
            .ReturnsAsync(chunks);

        _openAIServiceMock
            .Setup(x => x.GenerateAnswerAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync("AI Answer");

        // Act

        var result =
            await _ragService.AskAsync(question);

        // Assert

        Assert.Equal(
            2,
            result.Sources.Count);

        Assert.Contains(
            "dotnet.pdf",
            result.Sources);

        Assert.Contains(
            "dependency.pdf",
            result.Sources);
    }
    [Fact]
    public async Task AskAsync_HandlesEmptySearchResults()
    {
        // Arrange

        var question =
            "What is Quantum Computing?";

        var chunks =
            new List<SearchResultDto>();

        _semanticSearchServiceMock
            .Setup(x => x.SearchAsync(question))
            .ReturnsAsync(chunks);

        _openAIServiceMock
            .Setup(x => x.GenerateAnswerAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync("I could not find relevant information.");

        // Act

        var result =
            await _ragService.AskAsync(question);

        // Assert

        Assert.NotNull(result);

        Assert.Equal(
            "I could not find relevant information.",
            result.Answer);

        Assert.Empty(
            result.Sources);

        _openAIServiceMock.Verify(
            x => x.GenerateAnswerAsync(
                question,
                string.Empty),
            Times.Once);
    }
    [Fact]
    public async Task AskAsync_PropagatesOpenAIException()
    {
        // Arrange

        var question =
            "What is Dependency Injection?";

        var chunks =
            new List<SearchResultDto>
            {
            new SearchResultDto
            {
                Content = "Dependency Injection",
                FileName = "dotnet.pdf",
                Score = 0.95
            }
            };

        _semanticSearchServiceMock
            .Setup(x => x.SearchAsync(question))
            .ReturnsAsync(chunks);

        _openAIServiceMock
            .Setup(x => x.GenerateAnswerAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ThrowsAsync(
                new Exception("OpenAI failed"));

        // Act & Assert

        var exception =
            await Assert.ThrowsAsync<Exception>(
                () => _ragService.AskAsync(question));

        Assert.Equal(
            "OpenAI failed",
            exception.Message);
    }
    [Fact]
    public async Task AskAsync_PropagatesSemanticSearchException()
    {
        // Arrange

        var question =
            "What is Dependency Injection?";

        _semanticSearchServiceMock
            .Setup(x => x.SearchAsync(question))
            .ThrowsAsync(
                new Exception("Database error"));

        // Act & Assert

        var exception =
            await Assert.ThrowsAsync<Exception>(
                () => _ragService.AskAsync(question));

        Assert.Equal(
            "Database error",
            exception.Message);
    }
    [Fact]
    
    public async Task AskAsync_BuildsContextInCorrectOrder()
    {
        // Arrange

        var question =
            "Explain Dependency Injection";

        var chunks =
            new List<SearchResultDto>
            {
            new SearchResultDto
            {
                Content =
                    "First Chunk",
                FileName =
                    "first.pdf",
                Score = 0.98
            },

            new SearchResultDto
            {
                Content =
                    "Second Chunk",
                FileName =
                    "second.pdf",
                Score = 0.95
            },

            new SearchResultDto
            {
                Content =
                    "Third Chunk",
                FileName =
                    "third.pdf",
                Score = 0.93
            }
            };

        _semanticSearchServiceMock
            .Setup(x =>
                x.SearchAsync(question))
            .ReturnsAsync(chunks);

        _openAIServiceMock
            .Setup(x =>
                x.GenerateAnswerAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
            .ReturnsAsync("AI Answer");

        // Act

        await _ragService
            .AskAsync(question);

        // Assert

        var expectedContext =
            string.Join(
                Environment.NewLine,
                chunks.Select(x => x.Content));

        _openAIServiceMock.Verify(
            x => x.GenerateAnswerAsync(
                question,
                expectedContext),
            Times.Once);

        _semanticSearchServiceMock.Verify(
            x => x.SearchAsync(question),
            Times.Once);
    }
    [Fact]
    public async Task AskAsync_HandlesSingleChunk()
    {
        // Arrange

        var question =
            "What is JWT?";

        var chunks =
            new List<SearchResultDto>
            {
            new SearchResultDto
            {
                Content = "JWT is a JSON Web Token.",
                FileName = "jwt.pdf",
                Score = 0.98
            }
            };

        _semanticSearchServiceMock
            .Setup(x => x.SearchAsync(question))
            .ReturnsAsync(chunks);

        _openAIServiceMock
            .Setup(x => x.GenerateAnswerAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync("JWT Answer");

        // Act

        var result =
            await _ragService.AskAsync(question);

        // Assert

        Assert.Single(result.Sources);

        Assert.Equal(
            "jwt.pdf",
            result.Sources.First());

        _openAIServiceMock.Verify(
            x => x.GenerateAnswerAsync(
                question,
                "JWT is a JSON Web Token."),
            Times.Once);
    }
}
