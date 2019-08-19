using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static In2TeamsSplitter.Split;

namespace In2TeamsSplitter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        public ObservableCollection<Player> Team1 { get; set; }
        public ObservableCollection<Player> Team2 { get; set; }

        public ResultPage(Player[] t1, Player[] t2)
        {
            Team1 = new ObservableCollection<Player>(t1);
            Team2 = new ObservableCollection<Player>(t2);

            InitializeComponent();
        }
    }
}