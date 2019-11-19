using System.ComponentModel;

namespace WebWindows.ViewModelExample
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public void ViewReady() => OnViewReady();

        protected virtual void OnViewReady() { }

        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
