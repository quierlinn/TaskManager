using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Abstractions;

namespace TaskManager.DataAccess;

public class TaskManagerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }

    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options)
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=TaskManager.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username);
    }
}