using AutoHome.Universal.ViewModels;
using Windows.UI.Xaml.Controls;

namespace AutoHome.Universal.Views
{
    public sealed partial class SwitchesPage : Page
    {
        public SwitchesPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
        }

        // strongly-typed view models enable x:bind
        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;
    }
}
