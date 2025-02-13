using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;

namespace Common.DataBase;

public class DatabaseConnection
{
    protected string DatabasePath { get; set; }

    public DatabaseConnection(string databasePath)
    {
        DatabasePath = databasePath;
    }

    protected ObservableCollection<T> ExecuteQuery<T>(
        string query,
        Func<SqliteDataReader, ObservableCollection<T>> mapFunction,
        params (string parameterName, string value)[] parameters)
    {
        try
        {
            using (var connection = new SqliteConnection($"Data Source={DatabasePath}"))
            {
                connection.Open();

                using (var command = new SqliteCommand(query, connection))
                {
                    foreach (var (parameterName, value) in parameters)
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            command.Parameters.AddWithValue($"@{parameterName}", $"%{value}%");
                        }
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        return mapFunction(reader); 
                    }
                }
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Ошибка SQLite: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Общая ошибка: {ex.Message}");
        }

        return new ObservableCollection<T>();
    }
}