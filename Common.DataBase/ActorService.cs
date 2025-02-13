using FindYourMovie.EntityLayer.EntityClasses;
using System.Collections.ObjectModel;

namespace Common.DataBase;

public class ActorService : DatabaseConnection
{
    public ActorService(string databasePath) : base(databasePath) { }

    // Получение всех актеров
    public ObservableCollection<Actor> GetAllActors()
    {
        string query = "SELECT Id, Name FROM Actors;";

        // Вызываем ExecuteQuery с явным указанием типа T и пустым массивом параметров
        return ExecuteQuery<Actor>(query, reader =>
        {
            ObservableCollection<Actor> result = new ObservableCollection<Actor>();

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["Id"]);
                string name = reader["Name"].ToString();
                result.Add(new Actor(id, name));
            }

            return result;
        }, Array.Empty<(string parameterName, string value)>()); // Передаем пустой массив параметров
    }
}