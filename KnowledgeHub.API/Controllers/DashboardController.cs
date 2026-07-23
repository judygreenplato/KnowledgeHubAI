using KnowledgeHub.Application.Interfaces;
using KnowledgeHub.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController
    : ControllerBase
{
    private readonly IDashboardService
        _dashboardService;

    public DashboardController(
        IDashboardService dashboardService)
    {
        _dashboardService =
            dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult>
        GetDashboard()
    {
        var result =
            await _dashboardService
                .GetDashboardAsync();

        return Ok(result);
    }
}