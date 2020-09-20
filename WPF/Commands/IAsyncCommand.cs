using System.Threading.Tasks;
using System.Windows.Input;

namespace WeatherApp.WPF.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
