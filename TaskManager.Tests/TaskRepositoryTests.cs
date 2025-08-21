using FluentAssertions;
using System.Text.Json;
using TaskManager.Models;
using TaskManager.Repositories;

namespace TaskManager.Tests
{
    public class TaskRepositoryTests
    {
        private string _testFilePath;
        private TaskRepository _repository;

        [SetUp]
        public void Setup()
        {
            _testFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            _repository = new TaskRepository(_testFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);
        }
        [Test]
        public void LoadTasks_FileDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            // Act
            var result = _repository.LoadTasks();
            //Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
        [Test]
        public void LoadTasks_FileExists_ReturnsDeserializedTasks()
        {
            // Arrange
            var expectedTasks = new List<TaskItem>
                {
                    new TaskItem { Title = "Write tests", IsComplete = false },
                    new TaskItem { Title = "Refactor repo", IsComplete = true }
                };
            var json = JsonSerializer.Serialize(expectedTasks);
            File.WriteAllText(_testFilePath, json);

            // Act
            var loadedTasks = _repository.LoadTasks();

            // Assert
            loadedTasks.Should().BeEquivalentTo(expectedTasks);
        }
        [Test]
        public void LoadTasks_FileContainsMalformedJson_ReturnsEmptyList()
        {
            // Arrange
            var malformedJson = "{ \"Title\": \"Missing closing brace\" ";
            File.WriteAllText(_testFilePath, malformedJson);

            // Act
            var result = _repository.LoadTasks();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
        [Test]
        public void SaveTasks_ValidTasks_CreatesJsonFile()
        {
            // Arrange
            var tasksToSave = new List<TaskItem>
                {
                    new TaskItem { Title = "Interview prep", IsComplete = false }
                };
            // Act
            _repository.SaveTasks(tasksToSave);
            var json = File.ReadAllText(_testFilePath);
            var deserialized = JsonSerializer.Deserialize<List<TaskItem>>(json);
            // Assert
            File.Exists(_testFilePath).Should().BeTrue();
            deserialized.Should().BeEquivalentTo(tasksToSave);
        }
        [Test]
        public void SaveTasks_TasksWithEmptyTitle_ReturnsFalse()
        {
            // Arrange
            var tasksToSave = new List<TaskItem>
                {
                    new TaskItem { Title = "", IsComplete = false }
                };
            // Act

            var result = _repository.SaveTasks(tasksToSave);
            // Assert

            result.Should().BeFalse();
        }

    }
}
