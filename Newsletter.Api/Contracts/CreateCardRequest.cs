namespace Newsletter.Api.Contracts;

public class CreateCardRequest
{
    public required string PrintedName { get; set; } = string.Empty;
}