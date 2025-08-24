using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Repositories;
using TaskManager.Services;

namespace TaskManager.Tests.TaskServiceTests
{
    public class ListTaskTests
    {
        private Mock<ITaskRepository> _mockRepository;
        private TaskService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _service = new TaskService(_mockRepository.Object);
        }

        [Test]
        public void ListTasks_ShouldReturnSplitLists()
        {
            // Arrange
            var tasks = new List<TaskItem>
                {
                    new TaskItem { Title = "Task 1", IsComplete = false },
                    new TaskItem { Title = "Task 2", IsComplete = true },
                    new TaskItem { Title = "Task 3", IsComplete = false }
                };
            _mockRepository.Setup(r => r.LoadTasks()).Returns(tasks);
            // Act
            var (pending, completed) = _service.ListTasks();
            // Assert
            pending.Should().HaveCount(2);
            completed.Should().HaveCount(1);

            pending.Should().ContainSingle(t => t.Title == "Task 1");
            pending.Should().ContainSingle(t => t.Title == "Task 3");
            completed.Should().ContainSingle(t => t.Title == "Task 2");
        }

        [Test]
        public void ListTasks_EmptyList_ShouldReturnEmptyLists()
        {
            // Arrange
            _mockRepository.Setup(r => r.LoadTasks()).Returns(new List<TaskItem>());
            // Act
            var (pending, completed) = _service.ListTasks();
            // Assert
            pending.Should().BeEmpty();
            completed.Should().BeEmpty();
        }

    }
}
