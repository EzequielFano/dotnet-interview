using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options) { }

    public DbSet<TodoList> TodoList { get; set; } = default!;
    public DbSet<TodoItem> TodoItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuramos la relación: un TodoList tiene muchos TodoItems
        modelBuilder.Entity<TodoList>()
            .HasMany(tl => tl.TodoItems)
            .WithOne(ti => ti.TodoList)
            .HasForeignKey(ti => ti.TodoListId);

        base.OnModelCreating(modelBuilder);
    }
}
