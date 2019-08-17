using In2TeamsSplitter.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace In2TeamsSplitter.ViewModels
{
    class TeamMatesViewModel
    {
        public readonly SQLiteConnection conn;

        public ObservableCollection<TeamMateItem> TeamMateSquad { get; private set; }

        public static Lazy<TeamMatesViewModel> Instance = new Lazy<TeamMatesViewModel>();

        public TeamMatesViewModel()
        {
            conn = new SQLiteConnection($@"{FileSystem.AppDataDirectory}/teammates.db3");
            TeamMateSquad = new ObservableCollection<TeamMateItem>(conn.Table<TeamMateItem>().ToList());
        }
    }
}
