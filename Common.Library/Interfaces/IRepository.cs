
using System.Collections.ObjectModel;

namespace Common.Library.Interfaces;
public interface IRepository<TEntity>
{
    ObservableCollection<TEntity> Search(string databasePath, string name = null, string genre = null, string actorName = null);
    
    ObservableCollection<TEntity> Get(string databasePath);

    TEntity? Get(string databasePath, int id);
}
