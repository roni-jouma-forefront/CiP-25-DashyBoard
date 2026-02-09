using DashyBoard.Application.Common.Models;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Examples.Commands;

public class CreateExampleCommandHandler : IRequestHandler<CreateExampleCommand, Result<Guid>>
{
    // Inject your repository or DbContext here
    
    public async Task<Result<Guid>> Handle(CreateExampleCommand request, CancellationToken cancellationToken)
    {
        var entity = new ExampleEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // TODO: Add to database
        // await _repository.AddAsync(entity, cancellationToken);

        return await Task.FromResult(Result<Guid>.Success(entity.Id));
    }
}
