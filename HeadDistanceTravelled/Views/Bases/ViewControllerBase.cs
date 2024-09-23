using BeatSaberMarkupLanguage.ViewControllers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HeadDistanceTravelled
{
    public abstract class ViewControllerBase : BSMLAutomaticViewController
    {
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string memberName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }
            field = value;
            this.OnPropertyChanged(new PropertyChangedEventArgs(memberName));
            return true;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }
    }
}
