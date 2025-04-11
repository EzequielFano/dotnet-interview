using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/todoLists/{todoListId}/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/todoLists/{todoListId}/todoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems(long todoListId)
        {
            // Verifica que la TodoList exista
            var todoList = await _context.TodoList.FindAsync(todoListId);
            if (todoList == null)
            {
                return NotFound("TodoList no encontrada.");
            }

            var items = await _context.TodoItems
                .Where(ti => ti.TodoListId == todoListId)
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/todoLists/{todoListId}/todoItems/{id}
        [HttpGet("{id}", Name = "GetTodoItem")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long todoListId, long id)
        {
            var todoList = await _context.TodoList.FindAsync(todoListId);
            if (todoList == null)
            {
                return NotFound("TodoList no encontrada.");
            }

            var item = await _context.TodoItems
                .FirstOrDefaultAsync(ti => ti.Id == id && ti.TodoListId == todoListId);

            if (item == null)
            {
                return NotFound("TodoItem no encontrado.");
            }

            return Ok(item);
        }

        // POST: api/todoLists/{todoListId}/todoItems
        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodoItem(long todoListId, [FromBody] TodoItem newTodoItem)
        {
            // Verifica que la TodoList exista
            var todoList = await _context.TodoList.FindAsync(todoListId);
            if (todoList == null)
            {
                return NotFound("TodoList no encontrada.");
            }

            newTodoItem.TodoListId = todoListId;
            _context.TodoItems.Add(newTodoItem);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetTodoItem", new { todoListId = todoListId, id = newTodoItem.Id }, newTodoItem);
        }

        // PUT: api/todoLists/{todoListId}/todoItems/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long todoListId, long id, [FromBody] TodoItem updatedTodoItem)
        {
            if (id != todoListId)
            {
                return BadRequest("El ID del TodoItem no existe.");
            }

            var todoList = await _context.TodoList.FindAsync(todoListId);
            if (todoList == null)
            {
                return NotFound("TodoList no encontrada.");
            }

            var existingItem = await _context.TodoItems
                .FirstOrDefaultAsync(ti => ti.Id == id && ti.TodoListId == todoListId);

            if (existingItem == null)
            {
                return NotFound("TodoItem no encontrado en la TodoList especificada.");
            }

            existingItem.ItemName = updatedTodoItem.ItemName;

            _context.Entry(existingItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/todoLists/{todoListId}/todoItems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long todoListId, long id)
        {
            var todoList = await _context.TodoList.FindAsync(todoListId);
            if (todoList == null)
            {
                return NotFound("TodoList no encontrada.");
            }

            var todoItem = await _context.TodoItems
                .FirstOrDefaultAsync(ti => ti.Id == id && ti.TodoListId == todoListId);

            if (todoItem == null)
            {
                return NotFound("TodoItem no encontrado.");
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

