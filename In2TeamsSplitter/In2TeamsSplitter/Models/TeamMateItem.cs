using In2TeamsSplitter.ViewModels;
using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace In2TeamsSplitter.Models
{
    [Table("teammates")]
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

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string _name;

        [MaxLength(250), Unique]
        public string Name
        {
            get => _name;
            set => SetPropertyValue(ref _name, value);
        }

        private uint _level;

        [MaxLength(10)]
        public uint Level
        {
            get => _level;
            set => SetPropertyValue(ref _level, value);
        }

        public Command RemoveCommand => new Command(Remove);

        private void Remove()
        {
            TeamMatesViewModel.Instance.Value.conn.Delete(this);
            TeamMatesViewModel.Instance.Value.TeamMateSquad.Remove(this);
        }
    }
}
