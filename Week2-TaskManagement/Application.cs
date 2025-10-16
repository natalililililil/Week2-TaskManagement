using Week2_TaskManagement.Data;
using Week2_TaskManagement.Models;
using Week2_TaskManagement.Validation;

namespace Week2_TaskManagement
{
    internal class Application
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskValidator _validator;

        public Application(ITaskRepository taskRepository, ITaskValidator validator)
        {
            _taskRepository = taskRepository;
            _validator = validator;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("-----Менеджер задач------");

            while (true)
            {
                Console.WriteLine("\n1. Показать все задачи");
                Console.WriteLine("2. Показать конкретную задачу:");
                Console.WriteLine("3. Добавить задачу");
                Console.WriteLine("4. Завершить задачу");
                Console.WriteLine("5. Удалить задачу");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите действие: ");
                var choice = Console.ReadLine();

                Console.Clear();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await ShowAllTasksAsync();
                            break;
                        case "2":
                            await ShowByIdTaskAsync();
                            break;
                        case "3":
                            await AddNewTaskAsync();
                            break;
                        case "4":
                            await CompleteTaskAsync();
                            break;
                        case "5":
                            await DeleteTaskAsync();
                            break;
                        case "6":
                            return;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка ввода: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Операция не может быть выполнена: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
                }
            }
        }      

        private async Task ShowAllTasksAsync()
        {
            Console.WriteLine("-----Список всех задач-----");
            var tasks = await _taskRepository.GetAllAsync();

            if (!tasks.Any())
            {
                Console.WriteLine("Нет задач для отображения. Выбериите пункт 3 для добавления новой задачи");
                return;
            }

            foreach (var task in tasks)
                DisplayTask(task);
        }

        private async Task ShowByIdTaskAsync()
        {
            int id = ReadTaskId();
            var task = await _taskRepository.GetByIdAsync(id) ?? throw new InvalidOperationException("Задача с таким ID не найдена.");

            DisplayTask(task);
        }

        private void DisplayTask(AppTask task)
        {
            string status = task.IsCompleted ? "Выполнено" : "Не завершено";
            Console.WriteLine($"\t[{task.Id}] {task.Title}");
            Console.WriteLine($"\tОписание: {task.Description}");
            Console.WriteLine($"\tСтатус: {status}");
            Console.WriteLine($"\tСоздана: {task.CreatedAt:dd.MM.yyyy HH:mm}");
            Console.WriteLine();
        }

        private async Task AddNewTaskAsync()
        {
            string title = ReadValidatedString(_validator.ValidateTaskTitle, "Введите заголовок задачи: ");
            string description = ReadValidatedString(_validator.ValidateTaskDescription, "Введите описание задачи: ");

            var newTask = new AppTask(0, title, description, false, DateTime.Now);
            var newId = await _taskRepository.AddAsync(newTask);
            Console.WriteLine($"Задача добавлена с ID: {newId}");
        }

        private async Task CompleteTaskAsync()
        {
            var id = ReadTaskId();
            var task = await _taskRepository.GetByIdAsync(id) ?? throw new InvalidOperationException("Задача не найдена");

            var updatedTask = task with { IsCompleted = true };
            var success = await _taskRepository.UpdateTaskStatusAsync(updatedTask);
            if (!success)
                throw new Exception("Не удалось обновить статус задачи");

            Console.WriteLine("Задача отмечена как завершённая");
        }

        private async Task DeleteTaskAsync()
        {
            int id = ReadTaskId();
            var success = await _taskRepository.DeleteAsync(id);

            if (!success)
                throw new InvalidOperationException("Не удалось удалить задачу или она не найдена");

            Console.WriteLine("Задача успешно удалена");
        }

        private string ReadValidatedString(Action<string> validateAction, string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                try
                {
                    validateAction(input);
                    return input;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private int ReadTaskId()
        {
            while (true)
            {
                Console.Write("Введите ID задачи: ");
                string input = Console.ReadLine();
                try
                {
                    return _validator.ValidateTaskId(input);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}