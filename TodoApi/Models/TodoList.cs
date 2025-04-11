using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models;

public class TodoList
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();

}
