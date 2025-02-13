using Common.Library.BaseClasses;
using System.Collections.ObjectModel;

namespace FindYourMovie.EntityLayer.EntityClasses
{
    public class Movie : CommonBase
    {
        #region Constructor
        public Movie()
        {
        }
        public Movie(int id, string name, string genre)
        {
            _MovieId = id;
            _Name = name;
            _Genre = genre;
        }
        #endregion

        #region Private Variables
        private int _MovieId;
        private string _Name = string.Empty;
        private string _Genre = string.Empty;
        private ObservableCollection<Actor> _Actors = [];
        private string _ActorsNames = string.Empty;
        #endregion

        #region Public Properties
        public int MovieId
        {
            get { return _MovieId; }
            set
            {
                _MovieId = value;
                RaisePropertyChanged(nameof(MovieId));
            }
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }
        public string Genre
        {
            get { return _Genre; }
            set
            {
                _Genre = value;
                RaisePropertyChanged(nameof(Genre));
            }
        }

        public ObservableCollection<Actor> Actors { 
            get { return _Actors; }
            set
            {
                _Actors = value;
                RaisePropertyChanged(nameof(Actors));
            }
        }

        public string ActorsNames
        {
            get { return _ActorsNames; }
            set
            {
                _ActorsNames = value;
                RaisePropertyChanged(nameof(ActorsNames));
            }
        }
        #endregion
    }
}
