// File: TodoApi.Tests/TodoServiceTests.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TodoApi.Dtos;
using TodoApi.Models;
using TodoApi.Services;
using Xunit;

namespace TodoApi.Tests
{
    public class TodoServiceTests
    {
        private static DbContextOptions<TodoContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task CreateTodoList_ShouldReturnCreatedList()
        {
            var options = GetInMemoryOptions();
            using var context = new TodoContext(options);
            var service = new TodoService(context);
            var createDto = new CreateTodoList { Name = "Service Test List" };

            var todoList = await service.CreateTodoListAsync(createDto);

            Assert.NotNull(todoList);
            Assert.Equal("Service Test List", todoList.Name);
            Assert.Equal(TodoListStatus.Created, todoList.Status);
        }

        [Fact]
        public async Task UpdateTodoItem_StatusChange_ShouldUpdateTodoListStatus()
        {
            var options = GetInMemoryOptions();
            using var context = new TodoContext(options);
            var service = new TodoService(context);

            // Create a list and two items.
            var list = await service.CreateTodoListAsync(new CreateTodoList { Name = "Test List" });

            var item1 = new TodoItem
            {
                ItemName = "Item1",
                Description = "Test",
                Status = TodoItemStatus.Created,
                TodoListId = list.Id
            };
            var item2 = new TodoItem
            {
                ItemName = "Item2",
                Description = "Test",
                Status = TodoItemStatus.Created,
                TodoListId = list.Id
            };


            await service.CreateTodoItemAsync(list.Id, item1);
            await service.CreateTodoItemAsync(list.Id, item2);

            // Change status of item1 to InProgress.
            item1.Status = TodoItemStatus.InProgress;
            await service.UpdateTodoItemAsync(list.Id, item1);
            var updatedList = await service.GetTodoListAsync(list.Id);
            Assert.Equal(TodoListStatus.InProgress, updatedList!.Status);
            Assert.NotNull(updatedList.CreatedDate);

            // Change both items to Completed.
            item2.Status = TodoItemStatus.Completed;
            await service.UpdateTodoItemAsync(list.Id, item2);
            item1.Status = TodoItemStatus.Completed;
            await service.UpdateTodoItemAsync(list.Id, item1);
            updatedList = await service.GetTodoListAsync(list.Id);
            Assert.Equal(TodoListStatus.Completed, updatedList!.Status);
            Assert.NotNull(updatedList.ClosedDate);
            Assert.True(updatedList.Days.HasValue);
        }
    }
}
