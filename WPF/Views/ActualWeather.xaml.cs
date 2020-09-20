using System.Windows.Controls;
using WeatherApp.WPF.ViewModels;

namespace WeatherApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for ActualWeather.xaml
    /// </summary>
    public partial class ActualWeather : UserControl
    {
        public ActualWeather()
        {
            DataContext = new ActualWeatherViewModel();
            InitializeComponent();
        }
    }
}
