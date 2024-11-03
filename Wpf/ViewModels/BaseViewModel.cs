using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace Wpf.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected const string ServiceUrl = "https://localhost:5001/";
        protected readonly HttpClient HttpClient;

        protected BaseViewModel()
        {
            HttpClient = new HttpClient { BaseAddress = new Uri(ServiceUrl) };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
