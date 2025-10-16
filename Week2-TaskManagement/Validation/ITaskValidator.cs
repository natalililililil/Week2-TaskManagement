using Week2_TaskManagement.Models;

namespace Week2_TaskManagement.Validation
{
    public interface ITaskValidator
    {
        void ValidateForInsert(AppTask task);
        void ValidateForUpdate(AppTask task);
        int ValidateTaskId(string input);
    }
}
