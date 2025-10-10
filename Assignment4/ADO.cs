using System;
using Npgsql;

var connectionString = "Host=localhost;Username=postgres;Password=admin;Database=Northwind";

var connection = new NpgsqlConnection(connectionString);
connection.Open();

var cmd = new NpgsqlCommand();
cmd.Connection = connection;
cmd.CommandText = "SELECT * FROM categories";

var reader = cmd.ExecuteReader();
while (reader.Read())
{
    Console.WriteLine($"reader.GetInt32(0) {reader.GetInt32(0)}");
}