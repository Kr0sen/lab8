﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.IO;
using System.Reflection;

namespace lab8.Models
{
    public class Plan : INotifyPropertyChanged
    {
        public Plan(string n = "", string d = "", string p = "")
        {
            Name = n;
            Description = d;
            if (p.Length > 0)
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                string assemblyName = Assembly.GetEntryAssembly().GetName().Name;
                var uri = new Uri($"avares://{assemblyName}{p}");
                var bitmap = new Bitmap(assets.Open(uri));
                Image = bitmap;
            }
        }
        string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }
        string _description = "";
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                NotifyPropertyChanged(nameof(Description));
            }
        }
        Bitmap? _image = null;
        public Bitmap? Image
        {
            get => _image;
            set
            {
                _image = value;
                NotifyPropertyChanged(nameof(Image));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
