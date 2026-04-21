using DashyBoard.Application.DTOs;
using DashyBoard.Application.Features.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Login with hotel ID and password
    /// </summary>
    /// <param name="loginRequest">Login credentials</param>
    /// <returns>JWT token and admin information</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        if (loginRequest.HotelId == Guid.Empty)
        {
            return BadRequest("HotelId is required");
        }

        if (string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            return BadRequest("Password is required");
        }

        try
        {
            var command = new LoginCommand(loginRequest.HotelId, loginRequest.Password);
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return Unauthorized(new { message = result.Errors.FirstOrDefault() });
            }

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while processing your request"
            );
        }
    }
}
