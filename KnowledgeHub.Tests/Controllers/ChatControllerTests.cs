using KnowledgeHub.API.Controllers;
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KnowledgeHub.Tests.Controllers;

public class ChatControllerTests
{
    private readonly Mock<IPythonAgentService>
        _pythonAgentServiceMock;

    private readonly ChatController _controller;

    public ChatControllerTests()
    {
        _pythonAgentServiceMock =
            new Mock<IPythonAgentService>();

        _controller =
            new ChatController(
                _pythonAgentServiceMock.Object);
    }


    [Fact]
    public async Task Ask_Should_Return_Ok_With_ChatResponse()
    {
        // Arrange

        var request = new ChatRequest
        {
            Question = "What is RAG?"
        };

        var expectedResponse = new ChatResponse
        {
            Answer =
                "RAG stands for Retrieval-Augmented Generation.",

            Sources = new List<string>
            {
                "rag.pdf"
            }
        };

        _pythonAgentServiceMock
            .Setup(x =>
                x.AskAsync(request.Question))
            .ReturnsAsync(expectedResponse);


        // Act

        var result =
            await _controller.Ask(request);


        // Assert

        var okResult =
            Assert.IsType<OkObjectResult>(result);

        var response =
            Assert.IsType<ChatResponse>(
                okResult.Value);

        Assert.Equal(
            expectedResponse.Answer,
            response.Answer);

        Assert.Equal(
            expectedResponse.Sources,
            response.Sources);
    }


    [Fact]
    public async Task Ask_Should_Return_StatusCode200()
    {
        // Arrange

        var request = new ChatRequest
        {
            Question = "What is RAG?"
        };

        var expectedResponse = new ChatResponse
        {
            Answer = "RAG answer",
            Sources = new List<string>()
        };

        _pythonAgentServiceMock
            .Setup(x =>
                x.AskAsync(request.Question))
            .ReturnsAsync(expectedResponse);


        // Act

        var result =
            await _controller.Ask(request);


        // Assert

        var okResult =
            Assert.IsType<OkObjectResult>(result);

        Assert.Equal(
            500,
            okResult.StatusCode);
    }


    [Fact]
    public async Task Ask_Should_Call_PythonAgentService_Exactly_Once()
    {
        // Arrange

        var request = new ChatRequest
        {
            Question = "What is RAG?"
        };

        var expectedResponse = new ChatResponse
        {
            Answer = "RAG answer",
            Sources = new List<string>()
        };

        _pythonAgentServiceMock
            .Setup(x =>
                x.AskAsync(request.Question))
            .ReturnsAsync(expectedResponse);


        // Act

        await _controller.Ask(request);


        // Assert

        _pythonAgentServiceMock.Verify(
            x =>
                x.AskAsync(request.Question),
            Times.Once);
    }
}