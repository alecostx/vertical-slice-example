using Application.Shared.Infra.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Shared.Infra.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Card> Articles { get; set; }
}
