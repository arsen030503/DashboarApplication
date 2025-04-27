using System.IO;
using Microsoft.Data.Sqlite;

public class DatabaseInitializer
{
    public static void InitializeDatabase(string databasePath)
    {
        // Проверяем, существует ли файл базы данных
        if (!File.Exists(databasePath))
        {
            // Создаем файл базы данных SQLite
            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
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

            System.Console.WriteLine($"База данных создана по пути: {databasePath}");
        }
        else
        {
            System.Console.WriteLine($"База данных уже существует по пути: {databasePath}");
        }
    }
}