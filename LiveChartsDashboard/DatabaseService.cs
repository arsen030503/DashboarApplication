using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DashboardApp
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            // Замените параметры на ваши данные
            _connectionString = "Host=localhost;Database=dashboard_db;Username=postgres;Password=1337;";
        }

        public async Task<List<Activity>> GetActivitiesAsync()
        {
            var activities = new List<Activity>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("SELECT id, date, hours_worked, machine_name FROM activity", connection))
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
            catch (SqlException ex)
            {
                Console.WriteLine($"Ошибка SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
            }

            return activities;
        }

        public async Task AddActivityAsync(Activity activity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "INSERT INTO activity (date, hours_worked, machine_name) VALUES (@date, @hoursWorked, @machineName)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@date", activity.Date);
                        command.Parameters.AddWithValue("@hoursWorked", activity.HoursWorked);
                        command.Parameters.AddWithValue("@machineName", activity.MachineName);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Ошибка SQL при добавлении активности: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неизвестная ошибка при добавлении активности: {ex.Message}");
            }
        }

        public async Task UpdateActivityAsync(Activity activity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "UPDATE activity SET date = @date, hours_worked = @hoursWorked, machine_name = @machineName WHERE id = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", activity.Id);
                        command.Parameters.AddWithValue("@date", activity.Date);
                        command.Parameters.AddWithValue("@hoursWorked", activity.HoursWorked);
                        command.Parameters.AddWithValue("@machineName", activity.MachineName);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Ошибка SQL при обновлении активности: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неизвестная ошибка при обновлении активности: {ex.Message}");
            }
        }

        public async Task DeleteActivityAsync(int activityId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "DELETE FROM activity WHERE id = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", activityId);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Ошибка SQL при удалении активности: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неизвестная ошибка при удалении активности: {ex.Message}");
            }
        }
    }

    public class Activity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int HoursWorked { get; set; }
        public string MachineName { get; set; }
        public string Name { get; set; }
        public double Hours { get; set; }
        public string Category { get; set; }
    }
}