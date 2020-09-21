﻿using System.Windows;
using System.Windows.Controls;
using WeatherApp.WPF.Views;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ContentControl.Content = new ActualWeather();
        }
    }
}
