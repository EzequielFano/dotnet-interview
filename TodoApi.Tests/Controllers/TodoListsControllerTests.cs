// File: TodoApi.Tests/TodoListsControllerTests.cs
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
    public class TodoListsControllerTests
    {
        [Fact]
        public async Task GetTodoLists_ReturnsOkResult_WithListOfTodoLists()
        {
            // Arrange
            var mockService = new Mock<ITodoService>();
            var testTodoLists = new List<TodoList>
            {
                new TodoList { Id = 1, Name = "Test List 1", Status = TodoListStatus.Created },
                new TodoList { Id = 2, Name = "Test List 2", Status = TodoListStatus.InProgress }
            };
            mockService.Setup(s => s.GetTodoListsAsync()).ReturnsAsync(testTodoLists);
            var controller = new TodoListsController(mockService.Object);

            // Act
            var result = await controller.GetTodoLists();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var lists = Assert.IsAssignableFrom<IEnumerable<TodoList>>(okResult.Value);
            Assert.NotEmpty(lists);
        }

        [Fact]
        public async Task GetTodoList_ReturnsNotFound_WhenListDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ITodoService>();
            mockService.Setup(s => s.GetTodoListAsync(It.IsAny<long>())).ReturnsAsync((TodoList)null);
            var controller = new TodoListsController(mockService.Object);

            // Act
            var result = await controller.GetTodoList(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("TodoList not found", notFoundResult.Value);
        }
    }
}
