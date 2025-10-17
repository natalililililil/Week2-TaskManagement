namespace Week2_TaskManagement.Validation
{
    public interface ITaskValidator
    {
        void ValidateTaskTitle(string title);
        void ValidateTaskDescription(string description);
        int ValidateTaskId(string input);
    }
}
