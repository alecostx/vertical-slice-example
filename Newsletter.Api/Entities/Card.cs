using Application.Entities;

namespace Newsletter.Api.Entities;

public class Card : Entity
{
    public string PrintedName { get; set; } = string.Empty;

    public int Status { get; set; }
}
