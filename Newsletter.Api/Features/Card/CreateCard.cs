using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Newsletter.Api.Contracts;
using Newsletter.Api.Database;
using Newsletter.Api.Entities;
using Newsletter.Api.Shared;

namespace Newsletter.Api.Features.Articles;

public static class CreateCard
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

            var card = new Card
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
            var command = request.Adapt<CreateCard.Command>();

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        });
    }
}
