using System;

namespace Application.Features.Card.Models;

public class CardResponse
{
    public Ulid Id { get; set; }

    public string PrintedName { get; set; } = string.Empty;

    public int Status { get; set; }
}