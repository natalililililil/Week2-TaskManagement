namespace Week2_TaskManagement.Validation
{
    public class TaskValidator : ITaskValidator
    {
        private const int MaxTitleLength = 100;
        private const int MaxDescriptionLength = 500;

        public void ValidateTaskTitle(string title)
        {
            ValidateNonEmpty(title, MaxTitleLength, "Заголовок");
        }

        public void ValidateTaskDescription(string description)
        {
            ValidateNonEmpty(description, MaxDescriptionLength, "Описание");
        }

        public int ValidateTaskId(string input)
        {
            if (!int.TryParse(input, out int id))
                throw new ArgumentException("Неверный ID");

            return id;
        }

        private void ValidateNonEmpty(string value, int maxLength, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} не может быть пустым");
            if (value.Length > maxLength)
                throw new ArgumentException($"{fieldName} не может быть длиннее {maxLength} символов");
        }
    }
}
