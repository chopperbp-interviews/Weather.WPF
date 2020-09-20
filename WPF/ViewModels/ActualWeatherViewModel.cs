using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.WPF.Commands;
using WeatherApp.WPF.Models;

namespace WeatherApp.WPF.ViewModels
{
    public class ActualWeatherViewModel : ViewModelBase
    {
        private string apiUrl = "https://api.openweathermap.org/data/2.5/onecall?lat=47.497913&lon=19.040236&exclude=minutely,daily&units=metric&appid={apikey}";
        //https://openweathermap.org/api/one-call-api

        public ActualWeatherViewModel()
        {
            RefreshCommand = new AsyncDelegateCommand(async () => await GetForeCastCommand());
            RefreshCommand.Execute(null);
        }
        public IAsyncCommand RefreshCommand { get; }
        public bool IsLoading { get; private set; }
        private double _currentTemp;
        public double CurrentTemp
        {
            get => _currentTemp;
            private set
            {
                if (_currentTemp == value) return;
                _currentTemp = value;
                OnPropertyChanged();
            }
        }

        private double _currentHumidity;
        public double CurrentHumidity
        {
            get => _currentHumidity;
            private set
            {
                if (_currentHumidity == value) return;
                _currentHumidity = value;
                OnPropertyChanged();
            }
        }

        private double _currentPressure;
        public double CurrentPressure
        {
            get => _currentPressure;
            private set
            {
                if (_currentPressure == value) return;
                _currentPressure = value;
                OnPropertyChanged();
            }
        }
        private Dictionary<DateTime, double> _data;
        public Dictionary<DateTime, double> Data
        {
            get => _data;
            private set
            {
                if (_data == value) return;
                _data = value;
                OnPropertyChanged();
            }
        }
        private async Task GetForeCastCommand()
        {
            IsLoading = true;
            try
            {
                var forecast = await GetForecast();

                CurrentTemp = forecast.current.temp;
                CurrentHumidity = forecast.current.humidity;
                CurrentPressure = forecast.current.pressure;
                Data = forecast.hourly
                    .ToDictionary(
                    k => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                                .AddSeconds(k.dt)
                                .ToLocalTime(),
                    v => v.temp);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task<ForecastPOCO> GetForecast()
        {
            using (var client = new HttpClient())
            {
                var apikey = ConfigurationManager.AppSettings.Get("apikey");
                var response = await client.GetAsync(apiUrl.Replace("{apikey}",apikey));
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ForecastPOCO>(content);
            }
        }
    }
}

