using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Repositories;
using TaskManager.Services;

namespace TaskManager.Tests.TaskServiceTests
{
    public class AddTaskTests
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
        public void AddTask_WithValidInput_ShouldReturnTrue()
        {
            //Arrange
            var existingTasks = new List<TaskItem>();
            _mockRepository.Setup(r => r.LoadTasks()).Returns(existingTasks);
            _mockRepository.Setup(r => r.SaveTasks(It.IsAny<List<TaskItem>>())).Returns(true);
            //Act
            var result = _service.AddTask("Create unit tests", "Ensure code works as expected", DateOnly.FromDateTime(DateTime.Today));
            //Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.SaveTasks(It.Is<List<TaskItem>>(list =>
                list.Count == 1 &&
                list[0].Title == "Create unit tests" &&
                list[0].Description == "Ensure code works as expected"
            )));
        }

        [Test]
        public void AddTask_WithEmptyTitle_ShouldReturnFalse()
        {
            //Arrange
            //Act
            var result = _service.AddTask("", "No title", DateOnly.FromDateTime(DateTime.Today));
            //Assert
            result.Should().BeFalse();
            _mockRepository.Verify(r => r.SaveTasks(It.IsAny<List<TaskItem>>()), Times.Never);
        }

        [Test]
        public void AddTask_WithWhitespaceTitle_ShouldReturnFalse()
        {
            //Arrange
            //Act
            var result = _service.AddTask("   ", "Whitespace title", DateOnly.FromDateTime(DateTime.Today));
            //Assert
            result.Should().BeFalse();
            _mockRepository.Verify(r => r.SaveTasks(It.IsAny<List<TaskItem>>()), Times.Never);
        }

        [Test]
        public void AddTask_WhenSaveFails_ShouldReturnFalse()
        {
            //Arrange
            var existingTasks = new List<TaskItem>();
            _mockRepository.Setup(r => r.LoadTasks()).Returns(existingTasks);
            _mockRepository.Setup(r => r.SaveTasks(It.IsAny<List<TaskItem>>())).Returns(false);
            //Act
            var result = _service.AddTask("Failing Task", "Should fail", DateOnly.FromDateTime(DateTime.Today));
            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void AddTask_ShouldAppendToExistingTasks()
        {
            // Arrange
            var existingTasks = new List<TaskItem>
            {
                new TaskItem { Title = "Existing", Description = "Already there" }
            };
            _mockRepository.Setup(r => r.LoadTasks()).Returns(existingTasks);
            _mockRepository.Setup(r => r.SaveTasks(It.IsAny<List<TaskItem>>())).Returns(true);
            // Act
            var result = _service.AddTask("New Task", "Task to be added", DateOnly.FromDateTime(DateTime.Today));
            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.SaveTasks(It.Is<List<TaskItem>>(list =>
                list.Count == 2 &&
                list.Exists(t => t.Title == "New Task")
            )));
        }
    }
}
