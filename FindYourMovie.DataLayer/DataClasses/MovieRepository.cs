using Common.Library.Interfaces;
using Common.DataBase;
using FindYourMovie.EntityLayer.EntityClasses;
using System.Collections.ObjectModel;

namespace FindYourMovie.DataLayer.DataClasses
{
    public partial class MovieRepository : IRepository<Movie>
    {
        public ObservableCollection<Movie> Search(string databasePath, string name = null, string genre = null, string actorName = null)
        {
            MovieService service = new(databasePath);
            return service.SearchMovies(name, genre, actorName);

        }
        public ObservableCollection<Movie> Get(string databasePath)
        { return []; }

        public Movie? Get(string databasePath, int id)
        {
            return Get(databasePath).Where(row => row.MovieId == id).FirstOrDefault();
        }

        

    }
}
