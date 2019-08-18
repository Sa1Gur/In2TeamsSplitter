using In2TeamsSplitter.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace In2TeamsSplitter.ViewModels
{
    class TeamMatesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetPropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        public readonly SQLiteConnection conn;

        public ObservableCollection<TeamMateItem> TeamMateSquad { get; private set; }

        public static Lazy<TeamMatesViewModel> Instance = new Lazy<TeamMatesViewModel>();

        public TeamMatesViewModel()
        {
            AddNewTeamMateCommand = new Command(() => AddNewTeamMate());
            SplitCommand = new Command(Split);

            conn = new SQLiteConnection($@"{FileSystem.AppDataDirectory}/teammates.db3");
            TeamMateSquad = new ObservableCollection<TeamMateItem>(conn.Table<TeamMateItem>().ToList());
        }

        public Command AddNewTeamMateCommand { get; }
        public Command SplitCommand { get; }

        private void AddNewTeamMate()
        {
            try
            {
                if (string.IsNullOrEmpty(AddName)) throw new Exception("Valid name required");

                int _ = conn.Insert(new TeamMateItem { Name = AddName, Level = AddLevel });
                TeamMateSquad.Add(new TeamMateItem { Name = AddName, Level = AddLevel });

                AddName = string.Empty;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("error", $"Failed to add {AddName}. Error: {ex.Message}", "Ok");
            }
        }

        void Split() => In2TeamsSplitter.Split.Splitter(TeamMateSquad);

        private string _addName = string.Empty;
        public string AddName
        {
            get => _addName;
            set => SetPropertyValue(ref _addName, value);
        }

        private uint _addLevel = uint.MinValue;
        public uint AddLevel
        {
            get => _addLevel;
            set => SetPropertyValue(ref _addLevel, value);
        }
    }
}
