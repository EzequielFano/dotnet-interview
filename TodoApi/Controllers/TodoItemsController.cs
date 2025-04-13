using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/todolists/{todoListId}/todoitems")]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoService _todoService;
        public TodoItemsController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems(long todoListId)
        {
            var items = await _todoService.GetTodoItemsAsync(todoListId);
            return Ok(items);
        }

        [HttpGet("{id}", Name = "GetTodoItem")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long todoListId, long id)
        {
            var item = await _todoService.GetTodoItemAsync(todoListId, id);
            if (item == null)
                return NotFound("TodoItem not found");
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodoItem(long todoListId, TodoItem item)
        {
            try
            {
                var newItem = await _todoService.CreateTodoItemAsync(todoListId, item);
                return CreatedAtRoute("GetTodoItem", new { todoListId = todoListId, id = newItem.Id }, newItem);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long todoListId, long id, TodoItem item)
        {
            if (id != item.Id)
                return BadRequest("TodoItem ID mismatch.");

            var updatedItem = await _todoService.UpdateTodoItemAsync(todoListId, item);
            if (updatedItem == null)
                return NotFound("TodoItem not found");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long todoListId, long id)
        {
            var result = await _todoService.DeleteTodoItemAsync(todoListId, id);
            if (!result)
                return NotFound("TodoItem not found");
            return NoContent();
        }
    }
}
