using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace In2TeamsSplitter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NameAndRating : Grid
    {
        public NameAndRating()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(NameAndRating), defaultBindingMode: BindingMode.TwoWay);

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public static readonly BindableProperty LevelProperty = BindableProperty.Create(nameof(Level), typeof(int), typeof(NameAndRating), defaultBindingMode: BindingMode.TwoWay);

        public int Level
        {
            get => (int)GetValue(LevelProperty);
            set => SetValue(LevelProperty, value);
        }
    }
}