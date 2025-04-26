using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DashboardApp
{
    public class DatabaseService
    {
        private const string ConnectionString = "Your SQL Server Connection String";

        public DataTable GetData(string query)
        {
            using var connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(query, connection);
            var adapter = new SqlDataAdapter(command);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}