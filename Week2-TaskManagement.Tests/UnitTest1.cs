using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Week2_TaskManagement.Data;
using Week2_TaskManagement.Models;

namespace Week2_TaskManagement.Tests
{
    public class UnitTest1 : IDisposable
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ITaskRepository _taskRepository;
        private readonly List<int> _createdTaskIds = new();

        public UnitTest1()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<UnitTest1>()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Строка подключения для тестов не настроена в user secrets.");
            }

            _connectionFactory = new SqlConnectionFactory(connectionString);
            _taskRepository = new SQLTaskRepository(_connectionFactory);
        }

        [Fact]
        public async Task AddAsync_ShouldCreateNewTask_AndReturnId()
        {
            var task = new AppTask(0, "Test Task", "Test Description", false, DateTime.Now);

            var newId = await _taskRepository.AddAsync(task);
            _createdTaskIds.Add(newId);

            newId.Should().BeGreaterThan(0);
            var createdTask = await _taskRepository.GetByIdAsync(newId);
            createdTask.Should().NotBeNull();
            createdTask.Title.Should().Be("Test Task");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            var result = await _taskRepository.GetByIdAsync(-9999);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTasks()
        {
            var id1 = await _taskRepository.AddAsync(new AppTask(0, "Task 1", "Desc 1", false, DateTime.Now));
            var id2 = await _taskRepository.AddAsync(new AppTask(0, "Task 2", "Desc 2", true, DateTime.Now));
            _createdTaskIds.AddRange(new[] { id1, id2 });

            var tasks = await _taskRepository.GetAllAsync();

            tasks.Should().NotBeNull();
            tasks.Should().Contain(t => t.Id == id1);
            tasks.Should().Contain(t => t.Id == id2);
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_ShouldChangeIsCompleted()
        {
            var initialTask = new AppTask(0, "Task to Update", "Desc", false, DateTime.Now);
            var id = await _taskRepository.AddAsync(initialTask);
            _createdTaskIds.Add(id);

            var taskToUpdate = new AppTask(id, initialTask.Title, initialTask.Description, true, initialTask.CreatedAt);

            var result = await _taskRepository.UpdateTaskStatusAsync(taskToUpdate);

            result.Should().BeTrue();
            var updatedTask = await _taskRepository.GetByIdAsync(id);
            updatedTask!.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_ShouldReturnFalse_WhenTaskDoesNotExist()
        {
            var fakeTask = new AppTask(-1, "Fake", "Fake", true, DateTime.Now);

            var result = await _taskRepository.UpdateTaskStatusAsync(fakeTask);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveTask()
        {
            var id = await _taskRepository.AddAsync(new AppTask(0, "Task to Delete", "Desc", false, DateTime.Now));

            var result = await _taskRepository.DeleteAsync(id);

            result.Should().BeTrue();
            var deletedTask = await _taskRepository.GetByIdAsync(id);
            deletedTask.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenTaskDoesNotExist()
        {
            var result = await _taskRepository.DeleteAsync(-9999);

            result.Should().BeFalse();
        }

        public void Dispose()
        {
            foreach (var id in _createdTaskIds)
            {
                _taskRepository.DeleteAsync(id).Wait();
            }
        }
    }
}