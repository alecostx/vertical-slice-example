using Application.Features.Card.Models;
using Application.Shared.Domain.Models;
using Application.Shared.Infra.Database;
using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Card.UseCases;

public static class CreateCardHandle
{
    public class Command : IRequest<Result<CardResponse>>
    {
        public string PrintedName { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.PrintedName).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<CardResponse>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext dbContext, IValidator<Command> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task<Result<CardResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Result.Failure<CardResponse>(new Error(
                    "CreateArticle.Validation",
                    validationResult.ToString()));
            }

            var card = new Application.Shared.Infra.Database.Entities.Card
            {
                PrintedName = request.PrintedName,
                Status = 1,
            };

            _dbContext.Add(card);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CardResponse
            {
                Id = card.Id,
                PrintedName = card.PrintedName,
                Status = card.Status,
            };
        }
    }
}

public class CreateArticleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/card", async (CreateCardRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateCardHandle.Command>();

            var result = await sender.Send(command);

            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });
    }
}
