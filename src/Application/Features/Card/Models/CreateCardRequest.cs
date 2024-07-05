namespace Application.Features.Card.Models;

public class CreateCardRequest
{
    public required string PrintedName { get; set; } = string.Empty;
}