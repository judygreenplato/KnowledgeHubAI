using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public CategoriesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }




    [HttpPost]
    [Authorize(Roles = "Admin")]


    public async Task<IActionResult> CreateCategory(
    CreateCategoryRequest request)
    {
        var categoryExists = await _dbContext.Categories
      .AnyAsync(c => c.Name == request.Name);

        if (categoryExists)
        {
            return BadRequest("Category already exists.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Category name is required.");
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name
            
        };

        _dbContext.Categories.Add(category);

        await _dbContext.SaveChangesAsync();

        return Ok(new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name
        });
    }
}