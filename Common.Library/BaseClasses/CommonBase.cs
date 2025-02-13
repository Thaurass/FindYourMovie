
using System.ComponentModel;

namespace Common.Library.BaseClasses;

public abstract class CommonBase : INotifyPropertyChanged
{
    #region Constructor
    public CommonBase()
    {
        Init();
    }
    #endregion
    #region Init Method
    public virtual void Init()
    {
    }
    #endregion
    #region RaisePropertyChanged Method
    public event PropertyChangedEventHandler? PropertyChanged;

    public virtual void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
