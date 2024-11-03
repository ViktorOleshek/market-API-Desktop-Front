using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wpf.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        private int _id;

        protected BaseModel()
        {
            this.Id = 0;
        }

        protected BaseModel(int id)
        {
            this.Id = id;
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
