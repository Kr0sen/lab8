using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using lab8.ViewModels;

namespace lab8.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.FindControl<MenuItem>("New").Click += ClickEventNew;
            this.FindControl<MenuItem>("Save").Click += ClickEventSave;
            this.FindControl<MenuItem>("Load").Click += ClickEventLoad;
            this.FindControl<MenuItem>("Exit").Click += ClickEventExit;
            this.FindControl<MenuItem>("About").Click += ClickEventAbout;
        }
        private async void ClickEventNew(object sender, RoutedEventArgs e)
        {
            this.DataContext = new MainWindowViewModel();
        }
        private async void ClickEventSave(object sender, RoutedEventArgs e)
        {
        }
        private async void ClickEventLoad(object sender, RoutedEventArgs e)
        {
        }
        private async void ClickEventExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private async void ClickEventAbout(object? sender, RoutedEventArgs e)
        {
            var window = new About();
            window.Icon = new WindowIcon
                (new Bitmap(AvaloniaLocator
                .Current.GetService<IAssetLoader>()
                .Open(
                    new Uri(
                    $"avares://" +
                    $"{Assembly.GetEntryAssembly().GetName().Name}" +
                    $"/Assets/avalonia-logo-but-better.ico"
                ))));
            await window.ShowDialog((Window)this.VisualRoot);
        }
    }
}
