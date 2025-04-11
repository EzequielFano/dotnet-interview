using System.Text.Json.Serialization;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public required string ItemName { get; set; }
        public long TodoListId { get; set; }

        [JsonIgnore]
        public TodoList? TodoList { get; set; }
        //public required string OldName {  get; set; }
    }
}
