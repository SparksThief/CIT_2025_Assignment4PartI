using System;
using Npgsql;

namespace Assignment4
{
    public static class ADO
    {
        public static void PrintCategories()
        {
            var connectionString = "host=localhost;db=northwind;uid=postgres;pwd=admin";

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM categories", connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetInt32(0)} {reader.GetString(1)}");
            }
        }
    }
}