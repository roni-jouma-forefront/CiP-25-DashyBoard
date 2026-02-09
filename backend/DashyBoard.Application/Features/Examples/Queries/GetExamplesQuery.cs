using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Examples.Queries;

public record GetExamplesQuery : IRequest<List<ExampleDto>>;
