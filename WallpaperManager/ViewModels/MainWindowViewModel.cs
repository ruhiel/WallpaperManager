using Reactive.Bindings;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using WallpaperManager.Models;

namespace WallpaperManager.ViewModels
{
    public class MainWindowViewModel
    {
        public ReactiveCollection<string> MonitorList { get; set; } = new ReactiveCollection<string>();

        public ReactiveProperty<uint> SelectedIndex { get; set; } = new ReactiveProperty<uint>(0);

        public ReactiveProperty<string?> MyImage { get; set; } = new ReactiveProperty<string?>();

        public ReactiveCommand<DragEventArgs> DropCommand { get; } = new ReactiveCommand<DragEventArgs>();

        public MainWindowViewModel()
        {
            var pDesktopWallpaper = new IDesktopWallpaper();

            var cnt = pDesktopWallpaper.GetMonitorDevicePathCount();

            for (uint i = 0; i < cnt; i++)
            {
                MonitorList.Add($"ディスプレイ{i + 1}");
            }

            Marshal.ReleaseComObject(pDesktopWallpaper);

            SelectedIndex.Subscribe(i =>
            {
                var pDesktopWallpaper = new IDesktopWallpaper();

                var monitorID = pDesktopWallpaper.GetMonitorDevicePathAt(i);

                var path = pDesktopWallpaper.GetWallpaper(monitorID);

                MyImage.Value = path;

                Marshal.ReleaseComObject(pDesktopWallpaper);
            });

            DropCommand.Subscribe(e =>
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Any())
                {
                    var path = files.First();

                    var pDesktopWallpaper = new IDesktopWallpaper();

                    var monitorID = pDesktopWallpaper.GetMonitorDevicePathAt(SelectedIndex.Value);

                    pDesktopWallpaper.SetWallpaper(monitorID, path);

                    MyImage.Value = path;

                    Marshal.ReleaseComObject(pDesktopWallpaper);
                }
            });
        }
    }
}
