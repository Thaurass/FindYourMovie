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
            ObservableCollection<Movie> FinedMovies = service.SearchMovies(name, genre, actorName);
            return FillActorsNames(FinedMovies);

        }
        public ObservableCollection<Movie> Get(string databasePath)
        { return []; }

        #region FillActorsNames Method
        public ObservableCollection<Movie> FillActorsNames(ObservableCollection<Movie> movies)
        {
            if (movies == null || movies.Count == 0)
            {
                return new ObservableCollection<Movie>();
            }

            var updatedMovies = new ObservableCollection<Movie>();

            foreach (var movie in movies)
            {
                var updatedMovie = new Movie(movie.MovieId, movie.Name, movie.Genre)
                {
                    Actors = movie.Actors
                };
                if (updatedMovie.Actors != null && updatedMovie.Actors.Count > 0)
                {
                    updatedMovie.ActorsNames = string.Join(", ", updatedMovie.Actors.Select(actor => actor.Name));
                }

                updatedMovies.Add(updatedMovie);
            }

            return updatedMovies;
        }
        #endregion

    }
}
