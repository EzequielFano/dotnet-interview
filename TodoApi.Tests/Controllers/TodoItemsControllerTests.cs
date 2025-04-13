// File: TodoApi.Tests/TodoItemsControllerTests.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Services;
using Xunit;

namespace TodoApi.Tests
{
    public class TodoItemsControllerTests
    {
        [Fact]
        public async Task GetTodoItems_ReturnsOkResult_WithListOfTodoItems()
        {
            // Arrange
            var mockService = new Mock<ITodoService>();
            long todoListId = 1;
            var testTodoItems = new List<TodoItem>
            {
                new TodoItem { Id = 1, ItemName = "Item 1", Status = TodoItemStatus.Created, TodoListId = todoListId },
                new TodoItem { Id = 2, ItemName = "Item 2", Status = TodoItemStatus.InProgress, TodoListId = todoListId }
            };
            mockService.Setup(s => s.GetTodoItemsAsync(todoListId)).ReturnsAsync(testTodoItems);
            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.GetTodoItems(todoListId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(okResult.Value);
            Assert.NotEmpty(items);
        }

        [Fact]
        public async Task GetTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ITodoService>();
            mockService.Setup(s => s.GetTodoItemAsync(It.IsAny<long>(), It.IsAny<long>())).ReturnsAsync((TodoItem)null);
            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.GetTodoItem(1, 999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("TodoItem not found", notFoundResult.Value);
        }
    }
}
