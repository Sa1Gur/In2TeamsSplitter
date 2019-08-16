using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace In2TeamsSplitter.Models
{
    class TeamMateItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetPropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetPropertyValue(ref _name, value);
        }

        private uint _level;

        public uint Level
        {
            get => _level;
            set => SetPropertyValue(ref _level, value);
        }
    }
}
