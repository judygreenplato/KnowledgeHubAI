using System.Net;
using System.Net.Http.Json;
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Infrastructure.Integrations.Python;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace KnowledgeHub.Tests.Integration;

public class ChatIntegrationTests
    : IClassFixture<WebApplicationFactory<KnowledgeHub.API.Program>>
{
    private readonly WebApplicationFactory<KnowledgeHub.API.Program> _factory;

    public ChatIntegrationTests(
        WebApplicationFactory<KnowledgeHub.API.Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Chat_Should_Return_200_When_PythonAgent_Returns_Response()
    {
        // Arrange

        var pythonAgentMock =
            new Mock<IPythonAgentService>();

        pythonAgentMock
            .Setup(x => x.AskAsync(It.IsAny<string>()))
            .ReturnsAsync(new ChatResponse
            {
                Answer = "Test answer",
                Sources = new List<string>()
            });

        var factory =
            _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the real Python service
                    services.RemoveAll<IPythonAgentService>();

                    // Add our fake Python service
                    services.AddSingleton(
                        pythonAgentMock.Object);

                    // Replace authentication
                    services
                        .AddAuthentication(
                            "Test")
                        .AddScheme<
                            AuthenticationSchemeOptions,
                            TestAuthenticationHandler>(
                            "Test",
                            options => { });
                });
            });

        var client = factory.CreateClient();

        client.DefaultRequestHeaders
            .Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Test");

        var request = new ChatRequest
        {
            Question = "What is KnowledgeHub?"
        };

        // Act

        var response =
            await client.PostAsJsonAsync(
                "/api/Chat",
                request);

        // Assert

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }
}