using AutoMapper;
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using KnowledgeHub.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KnowledgeHub.Application.Tests;

public class ArticleServiceTests
{
    private readonly Mock<ICurrentUserService>
        _currentUserServiceMock;

    private readonly Mock<IArticleAuthorizationService>
        _authorizationServiceMock;

    private readonly Mock<IMapper>
        _mapperMock;

    private readonly AppDbContext _dbContext;

    private readonly ArticleService _articleService;

    private readonly Guid _userId =
        Guid.NewGuid();

    public ArticleServiceTests()
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

        _dbContext =
            new AppDbContext(options);

        _currentUserServiceMock =
            new Mock<ICurrentUserService>();

        _authorizationServiceMock =
            new Mock<IArticleAuthorizationService>();

        _mapperMock =
            new Mock<IMapper>();

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(_userId);

        _articleService =
            new ArticleService(
                _dbContext,
                _currentUserServiceMock.Object,
                _authorizationServiceMock.Object,
                _mapperMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Create_Article_Successfully()
    {
        // Arrange

        var request = new CreateArticleRequest
        {
            Title = "Test Article",
            Content = "This is test article content.",
            Summary = "Test summary",
            CategoryId = Guid.NewGuid()
        };

        var expectedResponse =
            new ArticleResponse();

        _mapperMock
            .Setup(x =>
                x.Map<ArticleResponse>(
                    It.IsAny<Article>()))
            .Returns(expectedResponse);


        // Act

        var result =
            await _articleService.CreateAsync(
                request);


        // Assert

        Assert.NotNull(result);

        Assert.Equal(
            expectedResponse,
            result);

        var article =
            await _dbContext.Articles
                .SingleAsync();

        Assert.Equal(
            request.Title,
            article.Title);

        Assert.Equal(
            request.Content,
            article.Content);

        Assert.Equal(
            request.CategoryId,
            article.CategoryId);

        Assert.Equal(
            _userId,
            article.CreatedByUserId);

        Assert.False(
            article.IsPublished);
    }


    [Fact]
    public async Task CreateAsync_Should_Assign_Current_User_Id()
    {
        // Arrange

        var request = new CreateArticleRequest
        {
            Title = "Test Article",
            Content = "Test content",
            Summary = "Test summary",
            CategoryId = Guid.NewGuid()
        };


        var expectedResponse =
            new ArticleResponse();

        _mapperMock
            .Setup(x =>
                x.Map<ArticleResponse>(
                    It.IsAny<Article>()))
            .Returns(expectedResponse);


        // Act

        await _articleService.CreateAsync(
            request);


        // Assert

        var article =
            await _dbContext.Articles
                .SingleAsync();

        Assert.Equal(
            _userId,
            article.CreatedByUserId);
    }


    [Fact]
    public async Task CreateAsync_Should_Use_Provided_Summary()
    {
        // Arrange

        var request = new CreateArticleRequest
        {
            Title = "Test Article",
            Content = "This is the article content.",
            Summary = "My custom summary",
            CategoryId = Guid.NewGuid()
        };


        _mapperMock
            .Setup(x =>
                x.Map<ArticleResponse>(
                    It.IsAny<Article>()))
            .Returns(new ArticleResponse());


        // Act

        await _articleService.CreateAsync(
            request);


        // Assert

        var article =
            await _dbContext.Articles
                .SingleAsync();

        Assert.Equal(
            "My custom summary",
            article.Summary);
    }


    [Fact]
    public async Task CreateAsync_Should_Generate_Summary_When_Summary_Is_Not_Provided()
    {
        // Arrange

        var content =
            "This is a long article content that does not provide a summary.";

        var request = new CreateArticleRequest
        {
            Title = "Test Article",
            Content = content,
            Summary = null,
            CategoryId = Guid.NewGuid()
        };


        _mapperMock
            .Setup(x =>
                x.Map<ArticleResponse>(
                    It.IsAny<Article>()))
            .Returns(new ArticleResponse());


        // Act

        await _articleService.CreateAsync(
            request);


        // Assert

        var article =
            await _dbContext.Articles
                .SingleAsync();

        var expectedSummary =
            content.Substring(
                0,
                Math.Min(100, content.Length));

        Assert.Equal(
            expectedSummary,
            article.Summary);
    }


    [Fact]
    public async Task CreateAsync_Should_Call_Mapper()
    {
        // Arrange

        var request = new CreateArticleRequest
        {
            Title = "Test Article",
            Content = "Test content",
            Summary = "Test summary",
            CategoryId = Guid.NewGuid()
        };


        _mapperMock
            .Setup(x =>
                x.Map<ArticleResponse>(
                    It.IsAny<Article>()))
            .Returns(new ArticleResponse());


        // Act

        await _articleService.CreateAsync(
            request);


        // Assert

        _mapperMock.Verify(
            x =>
                x.Map<ArticleResponse>(
                    It.Is<Article>(
                        a =>
                            a.Title == request.Title &&
                            a.Content == request.Content &&
                            a.CreatedByUserId == _userId)),
            Times.Once);
    }


    [Fact]
    public async Task CreateAsync_Should_Save_Article()
    {
        // Arrange

        var request = new CreateArticleRequest
        {
            Title = "Test Article",
            Content = "Test content",
            Summary = "Test summary",
            CategoryId = Guid.NewGuid()
        };


        _mapperMock
            .Setup(x =>
                x.Map<ArticleResponse>(
                    It.IsAny<Article>()))
            .Returns(new ArticleResponse());


        // Act

        await _articleService.CreateAsync(
            request);


        // Assert

        var articleCount =
            await _dbContext.Articles.CountAsync();

        Assert.Equal(
            1,
            articleCount);
    }
}