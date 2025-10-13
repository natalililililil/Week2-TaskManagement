using System.Data;

namespace Week2_TaskManagement.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
