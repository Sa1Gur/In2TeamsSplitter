using In2TeamsSplitter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace In2TeamsSplitter.ViewModels
{
    class TeamMatesViewModel
    {
        public ObservableCollection<TeamMateItem> TeamMateSquad { get; private set; }

        public static Lazy<TeamMatesViewModel> Instance = new Lazy<TeamMatesViewModel>();

        public TeamMatesViewModel()
        {            
            TeamMateSquad = new ObservableCollection<TeamMateItem>(new List<TeamMateItem>()
            {
                new TeamMateItem()
                {
                    Name = "First TeamMate",
                    Level = 0
                },
                new TeamMateItem()
                {
                    Name = "Second TeamMate",
                    Level = 1
                },
                new TeamMateItem()
                {
                    Name = "Third TeamMate",
                    Level = 2
                }
            });
        }
    }
}
