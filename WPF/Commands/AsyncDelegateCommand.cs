using System;
using System.Threading.Tasks;

namespace WeatherApp.WPF.Commands
{
    public class AsyncDelegateCommand : IAsyncCommand
    {
        private readonly Func<Task> _execute;
        private readonly Predicate<object> _canExecute;

        public AsyncDelegateCommand(Func<Task> execute) : this(execute, null) { }
        public AsyncDelegateCommand(Func<Task> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
        public Task ExecuteAsync(object parameter)
        {
            return _execute();
        }
        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public event EventHandler CanExecuteChanged;

        public void InvokeCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
