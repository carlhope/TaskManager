using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Repositories;
using TaskManager.Services;

namespace TaskManager.Tests.TaskServiceTests
{
    public class MarkCompleteTests
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
        public void MarkTaskAsComplete_ValidIndex_ShouldReturnTrue()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { Title = "Incomplete", IsComplete = false }
            };
            _mockRepository.Setup(r => r.LoadTasks()).Returns(tasks);
            _mockRepository.Setup(r => r.SaveTasks(It.IsAny<List<TaskItem>>())).Returns(true);
            // Act
            var result = _service.MarkTaskAsComplete(0);
            // Assert
            result.Should().BeTrue();
            tasks[0].IsComplete.Should().BeTrue();
        }

        [Test]
        public void MarkTaskAsComplete_InvalidIndex_ShouldReturnFalse()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { Title = "Only one", IsComplete = false }
            };
            _mockRepository.Setup(r => r.LoadTasks()).Returns(tasks);
            // Act
            var result = _service.MarkTaskAsComplete(5);
            // Assert
            result.Should().BeFalse();
            _mockRepository.Verify(r => r.SaveTasks(It.IsAny<List<TaskItem>>()), Times.Never);
        }

        [Test]
        public void MarkTaskAsComplete_SaveFails_ShouldReturnFalse()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { Title = "Try to complete", IsComplete = false }
            };
            _mockRepository.Setup(r => r.LoadTasks()).Returns(tasks);
            _mockRepository.Setup(r => r.SaveTasks(It.IsAny<List<TaskItem>>())).Returns(false);
            // Act
            var result = _service.MarkTaskAsComplete(0);
            // Assert
            result.Should().BeFalse();
            tasks[0].IsComplete.Should().BeTrue();
        }

        [Test]
        public void MarkTaskAsComplete_AlreadyComplete_ShouldStillReturnTrue()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { Title = "Already done", IsComplete = true }
            };
            _mockRepository.Setup(r => r.LoadTasks()).Returns(tasks);
            _mockRepository.Setup(r => r.SaveTasks(It.IsAny<List<TaskItem>>())).Returns(true);
            // Act
            var result = _service.MarkTaskAsComplete(0);
            // Assert
            result.Should().BeTrue();
            tasks[0].IsComplete.Should().BeTrue();
        }
    }
}
