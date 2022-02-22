using Microsoft.EntityFrameworkCore;
using hospital.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using hospital.Entity;

namespace hospital.Data; 
public class RowDbContext : IdentityDbContext<QueueUser>
{
    public RowDbContext (DbContextOptions<RowDbContext> options)
        : base(options) { }
    public DbSet<RowViewModel> rows { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RowViewModel>().HasIndex(i => i.Phone).IsUnique();
    }
}