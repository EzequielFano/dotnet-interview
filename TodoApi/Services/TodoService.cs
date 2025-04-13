// File: TodoApi/Services/TodoService.cs
using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly TodoContext _context;
        public TodoService(TodoContext context)
        {
            _context = context;
        }

        #region TodoList Service
        public async Task<IEnumerable<TodoList>> GetTodoListsAsync()
        {
            return await _context.TodoList.Include(tl => tl.TodoItems).ToListAsync();
        }

        public async Task<TodoList?> GetTodoListAsync(long id)
        {
            return await _context.TodoList
                                 .Include(tl => tl.TodoItems)
                                 .FirstOrDefaultAsync(tl => tl.Id == id);
        }

        public async Task<TodoList> CreateTodoListAsync(CreateTodoList dto)
        {
            var todoList = new TodoList
            {
                Name = dto.Name,
                Status = TodoListStatus.Created,
                CreatedDate = null,
                ClosedDate = null,
                Days = null
            };

            _context.TodoList.Add(todoList);
            await _context.SaveChangesAsync();
            return todoList;
        }

        public async Task<TodoList?> UpdateTodoListAsync(long id, UpdateTodoList dto)
        {
            var todoList = await _context.TodoList.Include(tl => tl.TodoItems)
                                                   .FirstOrDefaultAsync(tl => tl.Id == id);
            if (todoList == null) return null;

            todoList.Name = dto.Name;
            todoList.UpdateStatus();

            await _context.SaveChangesAsync();
            return todoList;
        }

        public async Task<bool> DeleteTodoListAsync(long id)
        {
            var todoList = await _context.TodoList.FindAsync(id);
            if (todoList == null) return false;

            _context.TodoList.Remove(todoList);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region TodoItem Service
        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync(long todoListId)
        {
            return await _context.TodoItems.Where(i => i.TodoListId == todoListId).ToListAsync();
        }

        public async Task<TodoItem?> GetTodoItemAsync(long todoListId, long id)
        {
            return await _context.TodoItems.FirstOrDefaultAsync(i => i.Id == id && i.TodoListId == todoListId);
        }

        public async Task<TodoItem> CreateTodoItemAsync(long todoListId, TodoItem item)
        {
            var todoList = await _context.TodoList.Include(tl => tl.TodoItems)
                                                   .FirstOrDefaultAsync(tl => tl.Id == todoListId);
            if (todoList == null)
            {
                throw new Exception("TodoList not found");
            }
            item.Status = TodoItemStatus.Created;
            item.TodoListId = todoListId;

            _context.TodoItems.Add(item);
            todoList.TodoItems.Add(item);

            todoList.UpdateStatus();

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<TodoItem?> UpdateTodoItemAsync(long todoListId, TodoItem item)
        {
            var todoList = await _context.TodoList.Include(tl => tl.TodoItems)
                                                   .FirstOrDefaultAsync(tl => tl.Id == todoListId);
            if (todoList == null) return null;

            var existingItem = await _context.TodoItems.FirstOrDefaultAsync(i => i.Id == item.Id && i.TodoListId == todoListId);
            if (existingItem == null) return null;

            existingItem.ItemName = item.ItemName;
            existingItem.Description = item.Description;
            existingItem.Status = item.Status;

            todoList.UpdateStatus();

            await _context.SaveChangesAsync();
            return existingItem;
        }

        public async Task<bool> DeleteTodoItemAsync(long todoListId, long id)
        {
            var todoList = await _context.TodoList.Include(tl => tl.TodoItems)
                                                   .FirstOrDefaultAsync(tl => tl.Id == todoListId);
            if (todoList == null) return false;

            var item = await _context.TodoItems.FirstOrDefaultAsync(i => i.Id == id && i.TodoListId == todoListId);
            if (item == null) return false;

            _context.TodoItems.Remove(item);

            todoList.UpdateStatus();

            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
