using Common.Library.BaseClasses;

namespace FindYourMovie.EntityLayer.EntityClasses
{
    public class Actor : CommonBase
    {
        #region Constructor
        public Actor()
        {
        }
        public Actor(int id, string name)
        {
            _ActorId = id;
            _Name = name;
        }
        #endregion

        #region Private Variables
        private int _ActorId;
        private string _Name = string.Empty;
        #endregion

        #region Public Properties
        public int ActorId
        {
            get { return _ActorId; }
            set
            {
                _ActorId = value;
                RaisePropertyChanged(nameof(ActorId));
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
        #endregion
    }
}
