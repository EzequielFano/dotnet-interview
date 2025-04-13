// File: TodoApi/Models/TodoList.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class TodoList
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public DateTime? CreatedDate { get; set; }  
        public DateTime? ClosedDate { get; set; }      
        public TodoListStatus Status { get; set; } = TodoListStatus.Created;
        public long? Days { get; set; }                
        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();

        /// <summary>
        /// Updates the status and the date fields based on the statuses of the associated TodoItems.
        /// Logic:
        /// - If there are no items: status remains Created.
        /// - If at least one item is InProgress or Completed, status becomes InProgress and if no CreatedDate, assign current date.
        /// - If all items are Completed, assign ClosedDate (if not already set) and calculate Days, then set status to Completed.
        /// </summary>
        public void UpdateStatus()
        {
            if (TodoItems == null || !TodoItems.Any())
            {
                Status = TodoListStatus.Created;
                return;
            }

            if (TodoItems.All(i => i.Status == TodoItemStatus.Completed))
            {
                if (Status != TodoListStatus.Completed)
                {
                    ClosedDate = DateTime.Now;
                    if (CreatedDate.HasValue)
                    {
                        Days = (long)(ClosedDate.Value - CreatedDate.Value).TotalDays;
                    }
                }
                Status = TodoListStatus.Completed;
            }
            else if (TodoItems.Any(i => i.Status == TodoItemStatus.InProgress || i.Status == TodoItemStatus.Completed))
            {
                Status = TodoListStatus.InProgress;
                if (!CreatedDate.HasValue)
                {
                    CreatedDate = DateTime.Now;
                }
            }
            else
            {
                Status = TodoListStatus.Created;
            }
        }
    }
}
