using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _dbContext;

    public DashboardService(
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DashboardDto>
        GetDashboardAsync()
    {
        return new DashboardDto
        {
            Articles =
                await _dbContext
                    .Articles
                    .CountAsync(),

            Categories =
                await _dbContext
                    .Categories
                    .CountAsync(),

            Documents =
                await _dbContext
                    .Documents
                    .CountAsync(),

            Users =
                await _dbContext
                    .Users
                    .CountAsync(),

            Embeddings = await _dbContext
            .DocumentEmbeddings 
            .CountAsync()
        };
    }
}