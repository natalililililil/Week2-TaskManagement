using Dapper;
using Week2_TaskManagement.Models;

namespace Week2_TaskManagement.Data
{
    public class SQLTaskRepository : ITaskRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public SQLTaskRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory), "Отсутвует подключения к базе данных");
        }
        public async Task<int> AddAsync(AppTask task)
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = @"INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt) VALUES (@Title, @Description, @IsCompleted, @CreatedAt); 
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";
            return await connection.ExecuteScalarAsync<int>(query, new
            {
                task.Title,
                task.Description,
                task.IsCompleted,
                task.CreatedAt
            });
        }

        public async Task<IEnumerable<AppTask>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = "SELECT * FROM Tasks";
            return await connection.QueryAsync<AppTask>(query);
        }

        public async Task<AppTask?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = "SELECT * FROM Tasks WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<AppTask>(query, new { Id = id });
        }

        public async Task<bool> UpdateTaskStatusAsync(AppTask task)
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = "UPDATE Tasks SET IsCompleted = @IsCompleted WHERE Id = @Id";
            return await connection.ExecuteAsync(query, new { task.IsCompleted, task.Id }) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = "DELETE FROM Tasks WHERE Id = @Id";
            return await connection.ExecuteAsync(query, new { Id = id }) > 0;
        }
    }
}
