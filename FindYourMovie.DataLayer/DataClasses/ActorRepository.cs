using Common.Library.Interfaces;
using FindYourMovie.EntityLayer.EntityClasses;
using System.Collections.ObjectModel;
using Common.DataBase;

namespace FindYourMovie.DataLayer.DataClasses
{
    public class ActorRepository : IRepository<Actor>
    {
        public ObservableCollection<Actor> Search(string databasePath, string name = null, string genre = null, string actorName = null)
        { return []; }

        public ObservableCollection<Actor> Get(string databasePath)
        {
            ActorService service = new(databasePath);
            return service.GetAllActors();
        }
    }
}
