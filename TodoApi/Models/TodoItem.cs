using System.Text.Json.Serialization;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public required string ItemName { get; set; }
        public string? Description { get; set; } 
        public TodoItemStatus Status { get; set; } = TodoItemStatus.Created;
        public required long TodoListId { get; set; }

        [JsonIgnore]
        public TodoList? TodoList { get; set; }
    }
}
