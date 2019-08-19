using In2TeamsSplitter.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace In2TeamsSplitter
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = TeamMatesViewModel.Instance.Value;
        }
    }
}
