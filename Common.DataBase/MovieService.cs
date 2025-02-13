using FindYourMovie.EntityLayer.EntityClasses;
using System.Collections.ObjectModel;

namespace Common.DataBase;

public class MovieService : DatabaseConnection
{
    public MovieService(string databasePath) : base(databasePath) { }

    // Поиск фильмов по имени, жанру или актеру
    public ObservableCollection<Movie> SearchMovies(string name = null, string genre = null, string actorName = null)
    {
        string query = @"
        SELECT m.Id AS MovieId, m.Name AS MovieName, m.Genre, a.Id AS ActorId, a.Name AS ActorName
        FROM Movies m
        LEFT JOIN MovieActor ma ON m.Id = ma.MovieId
        LEFT JOIN Actors a ON ma.ActorId = a.Id
        WHERE 1=1";

        // Добавление условий поиска
        var parameters = new List<(string parameterName, string value)>();
        if (!string.IsNullOrEmpty(name)) { query += " AND m.Name LIKE @name"; parameters.Add(("name", name)); }
        if (!string.IsNullOrEmpty(genre)) { query += " AND m.Genre LIKE @genre"; parameters.Add(("genre", genre)); }
        if (!string.IsNullOrEmpty(actorName)) { query += " AND a.Name LIKE @actorName"; parameters.Add(("actorName", actorName)); }

        return ExecuteQuery(query, reader =>
        {
            // Создаем коллекцию для хранения результатов
            ObservableCollection<Movie> result = new ObservableCollection<Movie>();

            while (reader.Read())
            {
                int movieId = Convert.ToInt32(reader["MovieId"]);
                string movieName = reader["MovieName"].ToString();
                string movieGenre = reader["Genre"].ToString();

                // Проверяем, существует ли уже такой фильм в коллекции
                Movie currentMovie = result.FirstOrDefault(m => m.MovieId == movieId);

                if (currentMovie == null)
                {
                    // Если нет, создаем новый объект Movie
                    currentMovie = new Movie(movieId, movieName, movieGenre)
                    {
                        Actors = new ObservableCollection<Actor>() // Инициализируем коллекцию актеров
                    };
                    result.Add(currentMovie);
                }

                // Добавляем актера к текущему фильму
                if (!reader.IsDBNull(reader.GetOrdinal("ActorId")) && !reader.IsDBNull(reader.GetOrdinal("ActorName")))
                {
                    int actorId = Convert.ToInt32(reader["ActorId"]);
                    string actorNameDb = reader["ActorName"].ToString();

                    // Проверяем, чтобы не добавлять дубликаты актеров
                    if (!currentMovie.Actors.Any(a => a.ActorId == actorId))
                    {
                        currentMovie.Actors.Add(new Actor(actorId, actorNameDb));
                    }
                }
            }

            return result; // Возвращаем коллекцию
        }, parameters.ToArray());
    }
}