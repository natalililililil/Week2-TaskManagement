using Week2_TaskManagement.Models;

namespace Week2_TaskManagement.Validation
{
    public class TaskValidator : ITaskValidator
    {
        private const int MaxTitleLength = 100;
        private const int MaxDescriptionLength = 500;

        public void ValidateForInsert(AppTask task)
        {
            ValidateTaskFields(task);
        }

        public void ValidateForUpdate(AppTask task)
        {
            if (task.Id <= 0)
                throw new ArgumentException("ID задачи должен быть положительным числом");

            ValidateTaskFields(task);
        }

        public int ValidateTaskId(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("ID задачи не может быть пустым");

            if (!int.TryParse(input, out int id))
                throw new ArgumentException("Неверный ID задачи");

            if (id <= 0)
                throw new ArgumentException("ID задачи должен быть положительным числом");

            return id;
        }

        private void ValidateTaskFields(AppTask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), "Задача не может быть null");

            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Заголовок задачи не может быть пустым");
            if (task.Title.Length > MaxTitleLength)
                throw new ArgumentException($"Заголовок не может быть длиннее {MaxTitleLength} символов");

            if (string.IsNullOrWhiteSpace(task.Description))
                throw new ArgumentException("Описание задачи не может быть пустым");
            if (task.Description.Length > MaxDescriptionLength)
                throw new ArgumentException($"Описание не может быть длиннее {MaxDescriptionLength} символов");
        }
    }
}