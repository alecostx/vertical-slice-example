using System;

namespace Application.Shared.Infra.Database.Entities;

public class Entity
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}