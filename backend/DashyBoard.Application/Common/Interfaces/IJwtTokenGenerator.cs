using DashyBoard.Domain.Entities;

namespace DashyBoard.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Admin admin);
}
