using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MosaicApp.Models;

namespace MosaicApp.Contexts;

public class MosaicDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Position> Positions { get; set; }
    public DbSet<Architector> Architectors { get; set; }

    public MosaicDbContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
