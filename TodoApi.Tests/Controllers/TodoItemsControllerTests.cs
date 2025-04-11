using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Controllers;
using TodoApi.Models;

namespace TodoApi.Tests.Controllers
{
    public class TodoItemsControllerTests
    {

        private DbContextOptions<TodoContext> DatabaseContextOptions()
        {
            return new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }
        [Fact]
        public async Task GetTodoItems_Returns_Items_When_TodoList_Exists()
        {
            using var context = new TodoContext(DatabaseContextOptions());
            var todoList = new TodoList { Id = 1, Name = "Lista de Prueba" };
            context.TodoList.Add(todoList);
            context.TodoItems.Add(new TodoItem { Id = 1, ItemName = "Item 1", TodoListId = 1 });
            context.TodoItems.Add(new TodoItem { Id = 2, ItemName = "Item 2", TodoListId = 1 });
            await context.SaveChangesAsync();

            var controller = new TodoItemsController(context);

            var actionResult = await controller.GetTodoItems(1);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(okResult.Value);
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public async Task GetTodoItem_Returns_Item_When_Exists()
        {
            using var context = new TodoContext(DatabaseContextOptions());
            var todoList = new TodoList { Id = 1, Name = "Lista de Prueba" };
            var todoItem = new TodoItem { Id = 1, ItemName = "Item de Prueba", TodoListId = 1 };
            context.TodoList.Add(todoList);
            context.TodoItems.Add(todoItem);
            await context.SaveChangesAsync();

            var controller = new TodoItemsController(context);

            var actionResult = await controller.GetTodoItem(1, 1);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var item = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal("Item de Prueba", item.ItemName);
        }

        [Fact]
        public async Task CreateTodoItem_Returns_CreatedAtRoute_When_Successful()
        {
            using var context = new TodoContext(DatabaseContextOptions());
            var todoList = new TodoList { Id = 1, Name = "Lista de Prueba" };
            context.TodoList.Add(todoList);
            await context.SaveChangesAsync();

            var controller = new TodoItemsController(context);
            var newItem = new TodoItem { ItemName = "Nuevo Item" };

            var actionResult = await controller.CreateTodoItem(1, newItem);

            var createdResult = Assert.IsType<CreatedAtRouteResult>(actionResult.Result);
            var createdItem = Assert.IsType<TodoItem>(createdResult.Value);
            Assert.Equal(1, createdItem.TodoListId);
            Assert.NotEqual(0, createdItem.Id); 
        }

        [Fact]
        public async Task UpdateTodoItem_Returns_NoContent_When_Successful()
        {
            using var context = new TodoContext(DatabaseContextOptions());
            var todoList = new TodoList { Id = 1L, Name = "Lista de Prueba" };
            var todoItem = new TodoItem { Id = 1L, ItemName = "Item Original", TodoListId = 1L };
            context.TodoList.Add(todoList);
            context.TodoItems.Add(todoItem);
            await context.SaveChangesAsync();

            var controller = new TodoItemsController(context);
            var updatedItem = new TodoItem { Id = 1L, ItemName = "NombreNuevo" };

            var result = await controller.UpdateTodoItem(1L, 1L, updatedItem);

            Assert.IsType<NoContentResult>(result);
            var itemInDb = await context.TodoItems.FindAsync(1L);
            Assert.Equal("NombreNuevo", itemInDb.ItemName);
        }

        [Fact]
        public async Task DeleteTodoItem_Returns_NoContent_When_Successful()
        {
            using var context = new TodoContext(DatabaseContextOptions());
            var todoList = new TodoList { Id = 1L, Name = "Lista de Prueba" };
            var todoItem = new TodoItem { Id = 1L, ItemName = "Item a eliminar", TodoListId = 1L };
            context.TodoList.Add(todoList);
            context.TodoItems.Add(todoItem);
            await context.SaveChangesAsync();

            var controller = new TodoItemsController(context);

            var result = await controller.DeleteTodoItem(1L, 1L);

            Assert.IsType<NoContentResult>(result);
            var deletedItem = await context.TodoItems.FindAsync(1L);
            Assert.Null(deletedItem);
        }


    }

}
