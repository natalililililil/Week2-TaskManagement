using Week2_TaskManagement.Models;

namespace Week2_TaskManagement.Data
{
    internal interface ITaskRepository
    {
        Task<int> AddAsync(AppTask task);
        Task<AppTask?> GetByIdAsync(int id);
        Task<IEnumerable<AppTask>> GetAllAsync();
        Task<bool> UpdateTaskStatusAsync(AppTask task);
        Task<bool> DeleteAsync(int id);
    }
}
