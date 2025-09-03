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
    public class ModifyTaskTests
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
        public void ModifyTask_ValidIndexAndInput_ShouldReturnTrue()
        {
            // Arrange
            var randomId = Guid.NewGuid();
            var tasks = new List<TaskItem>
            {
                new TaskItem {Id = randomId, Title = "Old Title", Description = "Old Description", DueDate = DateOnly.FromDateTime(DateTime.Today) }
            };
            _mockRepository.Setup(r => r.LoadTasks()).Returns(tasks);
            _mockRepository.Setup(r => r.SaveTasks(It.IsAny<List<TaskItem>>())).Returns(true);
            // Act
            var result = _service.ModifyTask(randomId, "New Title", "New Description", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), Priority.Medium);
            // Assert
            result.Should().BeTrue();
            tasks[0].Title.Should().Be("New Title");
            tasks[0].Description.Should().Be("New Description");
            tasks[0].DueDate.Should().Be(DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
        }

        [Test]
        public void ModifyTask_InvalidIndex_ShouldReturnFalse()
        {
            // Arrange
            var tasks = new List<TaskItem>();
            _mockRepository.Setup(r => r.LoadTasks()).Returns(tasks);
            // Act
            var result = _service.ModifyTask(Guid.NewGuid(), "New Title", "New Description", DateOnly.FromDateTime(DateTime.Today), Priority.Medium);
            // Assert
            result.Should().BeFalse();
            _mockRepository.Verify(r => r.SaveTasks(It.IsAny<List<TaskItem>>()), Times.Never);
        }

        [Test]
        public void ModifyTask_EmptyTitle_ShouldReturnFalse()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { Title = "Old", Description = "Old", DueDate = DateOnly.FromDateTime(DateTime.Today) }
            };
            _mockRepository.Setup(r => r.LoadTasks()).Returns(tasks);
            // Act
            var result = _service.ModifyTask(Guid.NewGuid(), "", "New Desc", DateOnly.FromDateTime(DateTime.Today), Priority.Medium);
            // Assert
            result.Should().BeFalse();
            _mockRepository.Verify(r => r.SaveTasks(It.IsAny<List<TaskItem>>()), Times.Never);
        }

        [Test]
        public void ModifyTask_SaveFails_ShouldReturnFalse()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { Title = "Old title", Description = "Old description", DueDate = DateOnly.FromDateTime(DateTime.Today) }
            };
            _mockRepository.Setup(r => r.LoadTasks()).Returns(tasks);
            _mockRepository.Setup(r => r.SaveTasks(It.IsAny<List<TaskItem>>())).Returns(false);
            // Act
            var result = _service.ModifyTask(Guid.NewGuid(), "New title", "New description", DateOnly.FromDateTime(DateTime.Today), Priority.Medium);
            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void ModifyTask_WhitespaceTitle_ShouldReturnFalse()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { Title = "Old title", Description = "Old description", DueDate = DateOnly.FromDateTime(DateTime.Today) }
            };
            _mockRepository.Setup(r => r.LoadTasks()).Returns(tasks);
            // Act
            var result = _service.ModifyTask(Guid.NewGuid(), "   ", "New description", DateOnly.FromDateTime(DateTime.Today),Priority.Medium);
            // Assert
            result.Should().BeFalse();
            _mockRepository.Verify(r => r.SaveTasks(It.IsAny<List<TaskItem>>()), Times.Never);
        }
    }
}
