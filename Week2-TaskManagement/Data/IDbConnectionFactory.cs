using System.Data;

namespace Week2_TaskManagement.Data
{
    internal interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
