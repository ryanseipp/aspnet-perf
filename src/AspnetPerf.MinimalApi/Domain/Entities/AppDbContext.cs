using Microsoft.EntityFrameworkCore;

namespace AspnetPerf.MinimalApi.Domain.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();

    public DbSet<Todo> Todos => Set<Todo>();
}
