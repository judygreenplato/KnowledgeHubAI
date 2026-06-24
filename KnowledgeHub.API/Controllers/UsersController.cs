using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace KnowledgeHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly PasswordHasher<User> _passwordHasher;

    public UsersController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _passwordHasher = new PasswordHasher<User>();
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
     
       var users = await _dbContext.Users
     .Select(u => new UserResponse
     {
         Id = u.Id,
         Email = u.Email
     })
     .ToListAsync();

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = request.Password, // temporary
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            Message = "User created successfully"
        });
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (existingUser != null)
        {
            return BadRequest(new
            {
                Message = "Email already exists"
            });
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            CreatedAtUtc = DateTime.UtcNow
        };

        user.PasswordHash =
            _passwordHasher.HashPassword(user, request.Password);

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            Message = "User registered successfully"
        });
    }
}