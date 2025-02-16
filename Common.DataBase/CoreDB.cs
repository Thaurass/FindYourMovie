using Microsoft.Data.Sqlite;

namespace Common.DataBase;
public class CoreDB
{
    public static void CreateAndFillDB(string databasePath)
    {
        try
        {
            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();

                string createMoviesTableCommand = @"
                    CREATE TABLE IF NOT EXISTS Movies (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Genre TEXT NOT NULL
                    );";
                ExecuteCommand(connection, createMoviesTableCommand);

                string createActorsTableCommand = @"
                    CREATE TABLE IF NOT EXISTS Actors (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL
                    );";
                ExecuteCommand(connection, createActorsTableCommand);

                string createMovieActorTableCommand = @"
                    CREATE TABLE IF NOT EXISTS MovieActor (
                        MovieId INTEGER NOT NULL,
                        ActorId INTEGER NOT NULL,
                        PRIMARY KEY (MovieId, ActorId)
                    );";
                ExecuteCommand(connection, createMovieActorTableCommand);

                string createIndexCommand = @"
                    CREATE INDEX IF NOT EXISTS idx_movie_actor ON MovieActor (MovieId, ActorId);";
                ExecuteCommand(connection, createIndexCommand);

                PopulateDatabase(databasePath);
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
    }

    private static void PopulateDatabase(string databasePath)
    {
        try
        {
            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();
                ClearTables(connection);

                string[] movieNames = { "Интерстеллар", "Матрица", "Властелин колец", "Аватар", "Звездные войны",
                                    "Гравитация", "Дюна", "Титаник", "Шерлок Холмс", "Пираты Карибского моря" };
                string[] movieGenres = { "Научная фантастика", "Научная фантастика", "Фэнтези", "Фантастика", "Фантастика",
                                     "Научная фантастика", "Фантастика", "Драма", "Детектив", "Приключения" };
                string[] actorNames = { "Мэттью МакКонахи", "Киану Ривз", "Иэн МакКеллен", "Самюэль Л. Джексон", "Марк Хэмилл",
                                    "Сандра Буллок", "Тимоти Шаламе", "Леонардо ДиКаприо", "Роберт Дауни мл.", "Джонни Депп" };

                Random random = new Random();

                foreach (var (name, genre) in movieNames.Zip(movieGenres, (n, g) => (n, g)))
                {
                    string insertMovieCommand = "INSERT OR IGNORE INTO Movies (Name, Genre) VALUES (@name, @genre);";
                    ExecuteCommandWithParams(connection, insertMovieCommand, new Dictionary<string, object>
                    {
                        { "@name", name },
                        { "@genre", genre }
                    });
                }

                foreach (var name in actorNames)
                {
                    string insertActorCommand = "INSERT OR IGNORE INTO Actors (Name) VALUES (@name);";
                    ExecuteCommandWithParams(connection, insertActorCommand, new Dictionary<string, object>
                    {
                        { "@name", name }
                    });
                }

                for (int i = 1; i <= movieNames.Length; i++)
                {
                    int actorsCount = random.Next(2, 5);
                    HashSet<int> assignedActors = new HashSet<int>();
                    while (assignedActors.Count < actorsCount)
                    {
                        int actorId = random.Next(1, actorNames.Length + 1);
                        if (!assignedActors.Contains(actorId))
                        {
                            assignedActors.Add(actorId);
                            if (!CheckIfLinkExists(connection, i, actorId))
                            {
                                string insertMovieActorCommand = "INSERT INTO MovieActor (MovieId, ActorId) VALUES (@movieId, @actorId);";
                                ExecuteCommandWithParams(connection, insertMovieActorCommand, new Dictionary<string, object>
                                {
                                    { "@movieId", i },
                                    { "@actorId", actorId }
                                });
                            }
                        }
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
    }

    private static void ClearTables(SqliteConnection connection)
    {
        string[] tables = { "MovieActor", "Movies", "Actors" };
        foreach (string table in tables)
        {
            ExecuteCommand(connection, $"DELETE FROM {table};");
            ExecuteCommand(connection, $"DELETE FROM sqlite_sequence WHERE name = '{table}';");
        }

        ExecuteCommand(connection, "VACUUM;");
    }

    private static bool CheckIfLinkExists(SqliteConnection connection, int movieId, int actorId)
    {
        string query = "SELECT 1 FROM MovieActor WHERE MovieId = @movieId AND ActorId = @actorId;";
        using (var command = new SqliteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@movieId", movieId);
            command.Parameters.AddWithValue("@actorId", actorId);
            return command.ExecuteScalar() != null;
        }
    }

    private static void ExecuteCommand(SqliteConnection connection, string commandText)
    {
        using (var command = new SqliteCommand(commandText, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    private static void ExecuteCommandWithParams(SqliteConnection connection, string commandText, Dictionary<string, object> parameters)
    {
        using (var command = new SqliteCommand(commandText, connection))
        {
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }
            command.ExecuteNonQuery();
        }
    }
}
