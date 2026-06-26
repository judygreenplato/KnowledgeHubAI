using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

    [HttpGet("admin-only")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminOnly()
    {
        return Ok(new
        {
            Message = "Welcome Admin!"
        });
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
    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(64));
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
       
        var tokenString =
             GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _dbContext.SaveChangesAsync();

        //return Ok(new
        // {
        //   Token = tokenString
        //});
        return Ok(new LoginResponse
        {
            AccessToken = tokenString,
            RefreshToken = refreshToken
        });

    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
    RefreshTokenRequest request)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u =>
                u.RefreshToken == request.RefreshToken);

        if (user == null)
        {
            return Unauthorized(new
            {
                Message = "Invalid refresh token"
            });
        }

        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Unauthorized(new
            {
                Message = "Refresh token expired"
            });
        }

        var newAccessToken = GenerateJwtToken(user);

        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime =
            DateTime.UtcNow.AddDays(7);

        await _dbContext.SaveChangesAsync();

        return Ok(new LoginResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }
}