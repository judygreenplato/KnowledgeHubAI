using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace KnowledgeHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public UsersController(
    AppDbContext dbContext,
    IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null)
        {
            return Unauthorized(new
            {
                Message = "Invalid email or password"
            });
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new
            {
                Message = "Invalid email or password"
            });
        }
        var claims = new[]
     {
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Email, user.Email)
      };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        var tokenString =
            new JwtSecurityTokenHandler()
                .WriteToken(token);

        return Ok(new
        {
            Token = tokenString
        });

    }
}