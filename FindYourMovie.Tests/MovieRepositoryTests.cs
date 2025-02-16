using System.Collections.ObjectModel;
using Common.DataBase;
using FindYourMovie.DataLayer.DataClasses;
using FindYourMovie.EntityLayer.EntityClasses;

namespace FindYourMovie.Tests;

[TestFixture]
public class MovieRepositoryTests
{
    private string _testDatabasePath = "test_movie_database.db";
    private MovieRepository _movieRepository;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Создание и заполнение тестовой базы данных
        CoreDB.CreateAndFillDB(_testDatabasePath);
        _movieRepository = new MovieRepository();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        // Удаление тестовой базы данных после всех тестов
        if (File.Exists(_testDatabasePath))
        {
            File.Delete(_testDatabasePath);
        }
    }

    [Test]
    public void Search_By_Name_ReturnsCorrectMovies()
    {
        // Arrange
        string searchName = "Интерстеллар";

        // Act
        ObservableCollection<Movie> result = _movieRepository.Search(_testDatabasePath, name: searchName);

        // Assert
        Assert.That(result, Is.Not.Null, "Результат не должен быть null.");
        Assert.That(result.Count, Is.EqualTo(1), "Должен быть найден ровно один фильм.");

        Movie movie = result[0];
        Assert.That(movie.Name, Is.EqualTo("Интерстеллар"), "Название фильма не совпадает.");
        Assert.That(movie.Genre, Is.EqualTo("Научная фантастика"), "Жанр фильма не совпадает.");

        // Проверяем наличие актеров
        Assert.That(movie.Actors.Count, Is.GreaterThan(0), "У фильма должны быть актеры.");
        Assert.That(movie.Actors.Any(a => a.Name == "Мэттью МакКонахи"),
            Is.True, "Актер 'Мэттью МакКонахи' должен быть связан с фильмом.");

        // Проверяем заполненное поле ActorsNames
        Assert.That(string.IsNullOrEmpty(movie.ActorsNames), Is.False, "Поле ActorsNames должно быть заполнено.");
        Assert.That(movie.ActorsNames.Contains("Мэттью МакКонахи"),
            Is.True, "Поле ActorsNames должно содержать имя актера 'Мэттью МакКонахи'.");
    }

    [Test]
    public void Search_By_Genre_ReturnsCorrectMovies()
    {
        // Arrange
        string searchGenre = "Научная фантастика";

        // Act
        ObservableCollection<Movie> result = _movieRepository.Search(_testDatabasePath, genre: searchGenre);

        // Assert
        Assert.That(result, Is.Not.Null, "Результат не должен быть null.");
        Assert.That(result.Count, Is.GreaterThanOrEqualTo(2), "Должно быть найдено хотя бы два фильма.");

        foreach (var movie in result)
        {
            Assert.That(movie.Genre, Is.EqualTo("Научная фантастика"), "Жанр фильма должен быть 'Научная фантастика'.");
            Assert.That(string.IsNullOrEmpty(movie.ActorsNames), Is.False, "Поле ActorsNames должно быть заполнено.");
        }
    }

    [Test]
    public void Search_By_ActorName_ReturnsCorrectMovies()
    {
        // Arrange
        string searchActorName = "Мэттью МакКонахи";

        // Act
        ObservableCollection<Movie> result = _movieRepository.Search(_testDatabasePath, actorName: searchActorName);

        // Assert
        Assert.That(result, Is.Not.Null, "Результат не должен быть null.");
        Assert.That(result.Count, Is.GreaterThanOrEqualTo(1), "Должен быть найден хотя бы один фильм.");

        foreach (var movie in result)
        {
            Assert.That(movie.Actors.Any(a => a.Name == "Мэттью МакКонахи"),
                Is.True, $"Фильм '{movie.Name}' должен содержать актера 'Мэттью МакКонахи'.");
            Assert.That(string.IsNullOrEmpty(movie.ActorsNames), Is.False, "Поле ActorsNames должно быть заполнено.");
            Assert.That(movie.ActorsNames.Contains("Мэттью МакКонахи"),
                Is.True, "Поле ActorsNames должно содержать имя актера 'Мэттью МакКонахи'.");
        }
    }

    [Test]
    public void Search_NoResults_ReturnsEmptyCollection()
    {
        // Arrange
        string searchName = "НеexistentFilm";

        // Act
        ObservableCollection<Movie> result = _movieRepository.Search(_testDatabasePath, name: searchName);

        // Assert
        Assert.That(result, Is.Not.Null, "Результат не должен быть null.");
        Assert.That(result.Count, Is.EqualTo(0), "Должна быть возвращена пустая коллекция при отсутствии результатов.");
    }

    [Test]
    public void FillActorsNames_FillsActorsNamesProperty()
    {
        // Arrange
        var movies = new ObservableCollection<Movie>
        {
            new Movie(1, "Интерстеллар", "Научная фантастика")
            {
                Actors = new ObservableCollection<Actor>
                {
                    new Actor(1, "Мэттью МакКонахи"),
                    new Actor(2, "Энн Хэтэуэй")
                }
            },
            new Movie(2, "Матрица", "Научная фантастика")
            {
                Actors = new ObservableCollection<Actor>
                {
                    new Actor(3, "Киану Ривз"),
                    new Actor(4, "Кэрри-Энн Мосс")
                }
            }
        };

        ObservableCollection<Movie> updatedMovies = _movieRepository.FillActorsNames(movies);

        Assert.That(updatedMovies, Is.Not.Null, "Обновленная коллекция не должна быть null.");
        Assert.That(updatedMovies.Count, Is.EqualTo(2), "Количество фильмов в обновленной коллекции должно совпадать.");

        foreach (var movie in updatedMovies)
        {
            Assert.That(string.IsNullOrEmpty(movie.ActorsNames), Is.False, "Поле ActorsNames должно быть заполнено.");
            if (movie.Name == "Интерстеллар")
            {
                Assert.That(movie.ActorsNames.Contains("Мэттью МакКонахи"),
                    Is.True, "Поле ActorsNames должно содержать имя актера 'Мэттью МакКонахи'.");
                Assert.That(movie.ActorsNames.Contains("Энн Хэтэуэй"),
                    Is.True, "Поле ActorsNames должно содержать имя актера 'Энн Хэтэуэй'.");
            }
            else if (movie.Name == "Матрица")
            {
                Assert.That(movie.ActorsNames.Contains("Киану Ривз"),
                    Is.True, "Поле ActorsNames должно содержать имя актера 'Киану Ривз'.");
                Assert.That(movie.ActorsNames.Contains("Кэрри-Энн Мосс"),
                    Is.True, "Поле ActorsNames должно содержать имя актера 'Кэрри-Энн Мосс'.");
            }
        }
    }
}