// File: TodoApi/Services/ITodoService.cs
using TodoApi.Models;
using TodoApi.Dtos;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        #region TodoList
        Task<IEnumerable<TodoList>> GetTodoListsAsync();
        Task<TodoList?> GetTodoListAsync(long id);
        Task<TodoList> CreateTodoListAsync(CreateTodoList dto);
        Task<TodoList?> UpdateTodoListAsync(long id, UpdateTodoList dto);
        Task<bool> DeleteTodoListAsync(long id);
        #endregion

        #region TodoItem
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync(long todoListId);
        Task<TodoItem?> GetTodoItemAsync(long todoListId, long id);
        Task<TodoItem> CreateTodoItemAsync(long todoListId, TodoItem item);
        Task<TodoItem?> UpdateTodoItemAsync(long todoListId, TodoItem item);
        Task<bool> DeleteTodoItemAsync(long todoListId, long id);
        #endregion
    }
}
