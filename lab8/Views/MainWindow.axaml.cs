using System;
using System.Reflection;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using lab8.Models;
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
            this.FindControl<Button>("AddPlanned").Click += ClickEventAddPlanned;
            this.FindControl<Button>("AddInWork").Click += ClickEventAddInWork;
            this.FindControl<Button>("AddCompleted").Click += ClickEventAddCompleted;
        }
        private async void ClickEventNew(object sender, RoutedEventArgs e)
        {
            this.DataContext = new MainWindowViewModel();
        }
        private async void ClickEventSave(object sender, RoutedEventArgs e)
        {
            string? path;
            var taskPath = new SaveFileDialog()
            {
                Title = "Save file",
                Filters = new List<FileDialogFilter>()
            };
            taskPath.Filters.Add(new FileDialogFilter() { Name = "Таблица задач (*.tot)", Extensions = { "tot" } });

            path = await taskPath.ShowAsync((Window)this.VisualRoot);

            if (path is not null)
            {
                var itemsPlanned = (this.DataContext as MainWindowViewModel).ItemsPlanned;
                var itemsInWork = (this.DataContext as MainWindowViewModel).ItemsInWork;
                var itemsCompleted = (this.DataContext as MainWindowViewModel).ItemsCompleted;
                using (var stream = File.Open(path, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                    {
                        writer.Write(itemsPlanned.Count);
                        foreach (var item in itemsPlanned)
                        {
                        }
                    }
                }
            }
        }
        private async void ClickEventLoad(object sender, RoutedEventArgs e)
        {
            string? path;
            var taskPath = new OpenFileDialog()
            {
                Title = "Open file",
                Filters = new List<FileDialogFilter>()
            };
            taskPath.Filters.Add(new FileDialogFilter() { Name = "Таблица задач (*.tot)", Extensions = { "tot" } });

            string[]? pathArray = await taskPath.ShowAsync((Window)this.VisualRoot);
            path = pathArray is null ? null : string.Join(@"\", pathArray);

            if (path is not null)
            {
                var itemsPlanned = (this.DataContext as MainWindowViewModel).ItemsPlanned;
                var itemsInWork = (this.DataContext as MainWindowViewModel).ItemsInWork;
                var itemsCompleted = (this.DataContext as MainWindowViewModel).ItemsCompleted;
                using (var stream = File.Open(path, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        aspectRatio = reader.ReadSingle();
                        tempDirectory = reader.ReadString();
                        autoSaveTime = reader.ReadInt32();
                        showStatusBar = reader.ReadBoolean();
                    }
                }
            }
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
        private async void ClickEventAddPlanned(object sender, RoutedEventArgs e)
        {
            AddPlanNewWindow("Planned");
        }
        private async void ClickEventAddInWork(object sender, RoutedEventArgs e)
        {
            AddPlanNewWindow("InWork");
        }
        private async void ClickEventAddCompleted(object sender, RoutedEventArgs e)
        {
            AddPlanNewWindow("Completed");
        }
        private async void AddPlanNewWindow(string type)
        {
            var window = new AddPlanView();
            window.Icon = new WindowIcon
                (new Bitmap(AvaloniaLocator
                .Current.GetService<IAssetLoader>()
                .Open(
                    new Uri(
                    $"avares://" +
                    $"{Assembly.GetEntryAssembly().GetName().Name}" +
                    $"/Assets/avalonia-logo-but-better.ico"
                ))));
            var item = await window.ShowDialog<Plan?>((Window)this.VisualRoot);
            if (item != null)
            {
                var context = this.DataContext as MainWindowViewModel;
                switch (type)
                {
                    case "Planned":
                        context.ItemsPlanned.Add(item);
                        break;
                    case "InWork":
                        context.ItemsInWork.Add(item);
                        break;
                    case "Completed":
                        context.ItemsCompleted.Add(item);
                        break;
                }
            }
        }
    }
}
