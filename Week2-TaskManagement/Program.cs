using Microsoft.Extensions.Configuration;
using Week2_TaskManagement;
using Week2_TaskManagement.Data;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");


if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Подключение с 'DefaultConnection' не найдено");
}

IDbConnectionFactory connectionFactory = new SqlConnectionFactory(connectionString);
ITaskRepository taskRepository = new SQLTaskRepository(connectionFactory);

var app = new Application(taskRepository);
await app.RunAsync();