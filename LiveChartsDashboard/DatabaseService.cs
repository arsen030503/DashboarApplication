using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

namespace DashboardApp
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            // Замените параметры на ваши данные
            _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1337;Database=dashboard_db";
        }

        public async Task<List<Activity>> GetActivitiesAsync()
        {
            var activities = new List<Activity>();

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand("SELECT id, date, hours_worked, machine_name FROM activity", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                activities.Add(new Activity
                                {
                                    Id = reader.GetInt32(0),
                                    Date = reader.GetDateTime(1),
                                    HoursWorked = reader.GetInt32(2),
                                    MachineName = reader.GetString(3)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении данных: {ex.Message}");
            }

            return activities;
        }

        public async Task AddActivityAsync(Activity activity)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "INSERT INTO activity (date, hours_worked, machine_name) VALUES (@date, @hoursWorked, @machineName)";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("date", activity.Date);
                        command.Parameters.AddWithValue("hoursWorked", activity.HoursWorked);
                        command.Parameters.AddWithValue("machineName", activity.MachineName);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении активности: {ex.Message}");
            }
        }
    }

    public class Activity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int HoursWorked { get; set; }
        public string MachineName { get; set; }
    }
}