using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Dtos;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoListsController : ControllerBase
    {
        private readonly ITodoService _todoService;
        public TodoListsController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoList>>> GetTodoLists()
        {
            var lists = await _todoService.GetTodoListsAsync();
            return Ok(lists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> GetTodoList(long id)
        {
            var list = await _todoService.GetTodoListAsync(id);
            if (list == null)
                return NotFound("TodoList not found");
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<TodoList>> CreateTodoList(CreateTodoList dto)
        {
            var newList = await _todoService.CreateTodoListAsync(dto);
            return CreatedAtAction(nameof(GetTodoList), new { id = newList.Id }, newList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoList(long id, UpdateTodoList dto)
        {
            var updatedList = await _todoService.UpdateTodoListAsync(id, dto);
            if (updatedList == null)
                return NotFound("TodoList not found");
            return Ok(updatedList);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoList(long id)
        {
            var result = await _todoService.DeleteTodoListAsync(id);
            if (!result)
                return NotFound("TodoList not found");
            return NoContent();
        }
    }
}
