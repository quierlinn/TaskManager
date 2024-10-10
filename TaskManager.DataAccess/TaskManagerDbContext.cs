using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
namespace TaskManager.DataAccess;

public class TaskManagerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5438;Database=TaskManager;Username=postgres;Password=12345");
    }
}