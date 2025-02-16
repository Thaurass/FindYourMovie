using System.Collections.ObjectModel;
using Common.DataBase;
using FindYourMovie.DataLayer.DataClasses;
using FindYourMovie.EntityLayer.EntityClasses;

namespace FindYourMovie.Tests;

[TestFixture]
public class ActorRepositoryTests
{
    private string _testDatabasePath = "test_movie_database.db";
    private ActorRepository _actorRepository;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Создание и заполнение тестовой базы данных
        CoreDB.CreateAndFillDB(_testDatabasePath);
        _actorRepository = new ActorRepository();
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
    public void Get_ReturnsAllActors()
    {
        // Act
        ObservableCollection<Actor> result = _actorRepository.Get(_testDatabasePath);

        // Assert
        Assert.That(result, Is.Not.Null, "Результат не должен быть null.");
        Assert.That(result.Count, Is.GreaterThan(0), "Должны быть найдены актеры.");

        foreach (var actor in result)
        {
            Assert.That(string.IsNullOrEmpty(actor.Name), Is.False, "Имя актера должно быть заполнено.");
        }
    }

    [Test]
    public void Search_AlwaysReturnsEmptyCollection()
    {
        // Arrange
        string searchName = "Мэттью МакКонахи";

        // Act
        ObservableCollection<Actor> result = _actorRepository.Search(_testDatabasePath, name: searchName);

        // Assert
        Assert.That(result, Is.Not.Null, "Результат не должен быть null.");
        Assert.That(result.Count, Is.EqualTo(0), "Метод Search всегда возвращает пустую коллекцию.");
    }
}