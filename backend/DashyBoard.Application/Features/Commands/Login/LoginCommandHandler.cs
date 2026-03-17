using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Login;

public class LoginCommandHandler(
    IRepository<Admin> repository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator
) : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    public async Task<Result<LoginResponseDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        // Find admin by HotelId
        var admins = await repository.FindAsync(
            a => a.HotelId == request.HotelId && !a.IsDeleted,
            cancellationToken
        );

        var admin = admins.FirstOrDefault();

        if (admin == null)
        {
            return Result<LoginResponseDto>.Failure("Invalid credentials");
        }

        // Verify password
        if (
            string.IsNullOrEmpty(admin.PasswordHash)
            || !passwordHasher.VerifyPassword(request.Password, admin.PasswordHash)
        )
        {
            return Result<LoginResponseDto>.Failure("Invalid credentials");
        }

        // Generate JWT token
        var token = jwtTokenGenerator.GenerateToken(admin);

        // Create response
        var adminDto = new AdminDto
        {
            Id = admin.Id,
            FirstName = admin.FirstName,
            LastName = admin.LastName,
            Role = admin.Role,
            HotelId = admin.HotelId,
        };

        var response = new LoginResponseDto { Token = token, Admin = adminDto };

        return Result<LoginResponseDto>.Success(response);
    }
}
