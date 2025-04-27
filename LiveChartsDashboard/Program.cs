using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using DashboardApp;

namespace LiveChartsDashboard
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs, or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            // Инициализация базы данных перед запуском Avalonia-приложения
            InitializeDatabase();

            // Запуск Avalonia-приложения
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();

        // Метод для инициализации базы данных
        private static void InitializeDatabase()
        {
            // Путь к базе данных
            var databasePath = Path.Combine(AppContext.BaseDirectory, "database.db");

            // Создаем базу данных
            DatabaseInitializer.InitializeDatabase(databasePath);

            Console.WriteLine("Инициализация базы данных завершена.");
        }

        // Метод для тестирования функционала работы с базой данных
        private static async Task TestDatabaseService()
        {
            var databaseService = new DatabaseService();

            Console.WriteLine("Тестирование DatabaseService...");

            // Добавление новой активности
            var newActivity = new Activity
            {
                Date = DateTime.Now,
                HoursWorked = 8,
                MachineName = "TestMachine"
            };

            Console.WriteLine("Добавление новой активности...");
            await databaseService.AddActivityAsync(newActivity);
            Console.WriteLine("Активность добавлена.");

            // Получение всех активностей
            Console.WriteLine("Получение всех активностей...");
            var activities = await databaseService.GetActivitiesAsync();
            Console.WriteLine("Список активностей:");
            foreach (var activity in activities)
            {
                Console.WriteLine($"{activity.Id} - {activity.Date} - {activity.HoursWorked} - {activity.MachineName}");
            }

            // Обновление активности
            if (activities.Count > 0)
            {
                var firstActivity = activities[0];
                firstActivity.HoursWorked = 10;
                firstActivity.MachineName = "UpdatedMachine";

                Console.WriteLine("Обновление первой активности...");
                await databaseService.UpdateActivityAsync(firstActivity);
                Console.WriteLine("Активность обновлена.");
            }

            // Удаление активности
            if (activities.Count > 0)
            {
                var lastActivityId = activities[^1].Id; // Получаем ID последней активности
                Console.WriteLine($"Удаление активности с ID: {lastActivityId}...");
                await databaseService.DeleteActivityAsync(lastActivityId);
                Console.WriteLine("Активность удалена.");
            }

            Console.WriteLine("Тестирование завершено.");
        }
    }

    public static class DatabaseInitializer
    {
        public static void InitializeDatabase(string databasePath)
        {
            // Проверяем, существует ли файл базы данных
            if (!File.Exists(databasePath))
            {
                // Создаем файл базы данных SQLite
                using (var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={databasePath}"))
                {
                    connection.Open();

                    // Создаем таблицы в базе данных
                    var createTableCommand = connection.CreateCommand();
                    createTableCommand.CommandText = @"
                        CREATE TABLE Activity (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT NOT NULL,
                            HoursWorked REAL NOT NULL,
                            MachineName TEXT NOT NULL
                        );";
                    createTableCommand.ExecuteNonQuery();
                }

                Console.WriteLine($"База данных создана по пути: {databasePath}");
            }
            else
            {
                Console.WriteLine($"База данных уже существует по пути: {databasePath}");
            }
        }
    }
}