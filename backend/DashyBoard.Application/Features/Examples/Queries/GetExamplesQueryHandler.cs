using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Examples.Queries;

public class GetExamplesQueryHandler : IRequestHandler<GetExamplesQuery, List<ExampleDto>>
{
    // Inject your repository or DbContext here

    public async Task<List<ExampleDto>> Handle(
        GetExamplesQuery request,
        CancellationToken cancellationToken
    )
    {
        // TODO: Implement query logic
        return await Task.FromResult(new List<ExampleDto>());
    }
}
